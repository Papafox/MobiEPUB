//------------------------------------------------------------------------------------
//    This file is part of MobiEPUB.
//
//    MobiEPUB is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    MobiEPUB is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with MobiEPUB.  If not, see <http://www.gnu.org/licenses/>.
//------------------------------------------------------------------------------------
//    Copyright 2012, Matthew Donald
//------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MobiEPUB
{
    public partial class MainForm : Form
    {
        private const float ROWSIZE = 28.0F;

        private Settings m_Settings;
        private Ebook ebook;

        public MainForm()
        {
            InitializeComponent();
            m_Settings = new Settings();
        }

        private void aboutMobiEPUBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm(m_Settings);
            DialogResult res = settings.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();

            // Restore the main form to the last location
            this.Size = m_Settings.Size;
            this.Location = m_Settings.Location;

            // Make sure form cannot be samller than pubControlPanel;
            this.MinimumSize = new Size(pubControlPanel.Width, pubControlPanel.Height + toolStrip.Height + menuStrip.Height);

            // Initialize the various panels
            projectPanel_Load(sender, e);

            // Initially show the project view.  Hide the others.
            SelectPanel(projectPanel);

            this.ResumeLayout();
        }

        private void SelectPanel(Panel p)
        {
            if (p == buildPanel)
            {
                documentPanel.Visible = false;
                buildPanel.Visible = true;
                projectPanel.Visible = false;
            }
            else if (p == documentPanel)
            {
                documentPanel.Visible = true;
                buildPanel.Visible = false;
                projectPanel.Visible = false;
            }
            else if (p == projectPanel)
            {
                documentPanel.Visible = false;
                buildPanel.Visible = false;
                projectPanel.Visible = true;
            }
        }

        private void projectPanel_Load(object sender, EventArgs e)
        {
            projectPanel_SizeChanged(sender, e);
            pubFolderText.Text = m_Settings.DefaultDir;

            // Encodings
            String defEnc = m_Settings.DefaultEncoding;
            pubEncodingCombo.Items.AddRange(m_Settings.AvailableEncodings);
            pubEncodingCombo.SelectedIndex = pubEncodingCombo.Items.IndexOf(defEnc);

            // Languages
            String deflang = m_Settings.DefaultLanguage;
            pubLanguageCombo.Items.AddRange(m_Settings.AvailableLanguages);
            pubLanguageCombo.SelectedIndex = pubLanguageCombo.Items.IndexOf(deflang);
        }

        private void documentPanel_Load(object sender, EventArgs e)
        {

            int visibleRows = docFileTablePanel.Height / (int)ROWSIZE;
            docFileTablePanel.VerticalScroll.LargeChange = (visibleRows - 2) * (int)ROWSIZE;
            docFileTablePanel.VerticalScroll.SmallChange = (int)ROWSIZE;
            docFileTablePanel.VerticalScroll.Value = 0;

            int col0Width = (int)docFileTablePanel.ColumnStyles[0].Width;
            int col1Width = (int)docFileTablePanel.ColumnStyles[1].Width;
            int textWidth = docFileTablePanel.Width - (col0Width + col1Width) - 20;
            docFileTablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

            loadProgressBar.Visible = true;
            loadProgressBar.Minimum = 0;
            loadProgressBar.Maximum = ebook.Documents.Length;

            docFileTablePanel.SuspendLayout();
            documentPanel_SizeChanged(sender, e);
            RemoveRows();

            foreach (DocumentFile doc in ebook.Documents)
            {
                int row = AddTableRow();
                docFileTablePanel.VerticalScroll.Enabled = (row > visibleRows);

                PictureBox pic = new PictureBox();
                pic.Size = new Size(30, 30);
                pic.Padding = new Padding(2);
                pic.Image = Properties.Resources.ebook_icon;
                using (Graphics g = pic.CreateGraphics())
                {
                    g.DrawImage(Properties.Resources.ebook_icon, 0, 0);
                }
                pic.Anchor = AnchorStyles.Left;
                pic.Click += DocFileTableRowSelect;
                docFileTablePanel.Controls.Add(pic, 0, row);
                pic.BringToFront();

                Label l = new Label();
                l.Text = doc.ItemID;
                l.Anchor = AnchorStyles.Left;
                l.Padding = new Padding(0, 4, 0, 0);
                l.TextAlign = ContentAlignment.MiddleLeft;
                l.Click += DocFileTableRowSelect;
                docFileTablePanel.Controls.Add(l, 1, row);

                // TextBox text = new TextBox();
                Label text = new Label();
                text.AutoSize = false;
                text.Padding = new Padding(0, 4, 0, 0);
                text.Width = textWidth;
                text.Text = doc.Filename;
                text.Anchor = AnchorStyles.Left;
                docFileTablePanel.Controls.Add(text, 2, row);

                loadProgressBar.Value = row;
            }

            docFileTablePanel.VerticalScroll.Maximum = docFileTablePanel.RowCount * (int)ROWSIZE;
            docFileTablePanel.ResumeLayout();
            docFileTablePanel.PerformLayout();
            loadProgressBar.Visible = false;
        }

        private int AddTableRow()
        {
            int result = docFileTablePanel.RowCount++;
            RowStyle style = new RowStyle(SizeType.Absolute, ROWSIZE);
            docFileTablePanel.RowStyles.Add(style);
            return result;
        }

        // Clean out the table panel of any existing data
        private void RemoveRows()
        {
            // Remove all existing controls
            docFileTablePanel.Controls.Clear();

            // Delete each of the existing row. Note doesn't remove controls.
            for (int row = docFileTablePanel.RowCount - 1; row >= 0; row--)
            {
                docFileTablePanel.RowStyles.RemoveAt(row);
            }

            // Make sure rowcount also cleared.  Deleting rowstyle doesn't do this.
            docFileTablePanel.RowCount = 0;
        }

        private void DocFileTableRowSelect(Object sender, EventArgs e)
        {
            int row = docFileTablePanel.GetRow((Control)sender);
            Control control = (Control)sender;
            control.Select();

        }

        private void projectPanel_SizeChanged(object sender, EventArgs e)
        {
            pubControlPanel.Top = 0;
            pubControlPanel.Left = (projectPanel.Width - pubControlPanel.Width) / 2;
            pubControlPanel.Height = projectPanel.Height;
        }

        private void documentPanel_SizeChanged(object sender, EventArgs e)
        {
            documentPanel.SuspendLayout();
            docControlPanel.Top = 0;
            docControlPanel.Left = (documentPanel.Width - docControlPanel.Width) / 2;
            docControlPanel.Height = documentPanel.Height;
            fileTabMainPanel.Height = docFilesTab.Height - fileTabTopPanel.Height - 4;
            documentPanel.ResumeLayout();
            documentPanel.PerformLayout();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Check is form location/size was changed by the user
            if (this.Size != m_Settings.Size)
                m_Settings.Size = this.Size;

            if (this.Location != m_Settings.Location)
                m_Settings.Location = this.Location;

            // Save if changed
            if (m_Settings.Changed)
                m_Settings.Save();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            projectPanel.Visible = false;
            buildPanel.Visible = false;
            documentPanel.Visible = true;
        }

        private void pubDirButton_Click(object sender, EventArgs e)
        {
            //folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyDocuments;
            //folderBrowserDialog.RootFolder = m_Settings.DefaultDir;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                pubFolderText.Text = folderBrowserDialog.SelectedPath;
        }

        private void projectPanelText_TextChanged(object sender, EventArgs e)
        {
            bool ok = true;

            ok &= pubFilenameText.Text.Length > 0;
            ok &= pubFolderText.Text.Length > 0;
            ok &= pubLanguageCombo.Text.Length > 0;
            ok &= pubEncodingCombo.Text.Length > 0;

            pubCreateButton.Enabled = ok;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = m_Settings.DefaultDir;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                String filename = openFileDialog.FileName;
                String ext = Path.GetExtension(filename).ToLower();
                if (ext == ".epub")
                {
                    ebook = new EPUBebook(filename);
                }
                if (ext == ".prc")
                {
                    ebook = new PRCebook(filename);
                }
                SelectPanel(documentPanel);
                documentPanel_Load(sender, e);
            }
        }

    }
}
