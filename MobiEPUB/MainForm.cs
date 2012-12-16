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
            //documentPanel_Load(sender, e);

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
            int col0Width =  (int)docFileTablePanel.ColumnStyles[0].Width;
            int col2Width =  (int)docFileTablePanel.ColumnStyles[0].Width;
            int textWidth = docFileTablePanel.Width - (col0Width + col2Width);
            docFileTablePanel.RowCount = 0;

            this.SuspendLayout();
            documentPanel_SizeChanged(sender, e);
            docFileTablePanel.RowCount = 0;
            foreach (DocumentFile doc in ebook.Documents)
            {
                int row = AddTableRow();

                PictureBox pic = new PictureBox();
                pic.Image = Properties.Resources.ebook_icon;
                pic.Anchor = AnchorStyles.Left;
                docFileTablePanel.Controls.Add(pic, 0, row);

                TextBox text = new TextBox();
                text.Width = textWidth;
                text.Text = doc.Filename;
                text.Anchor = AnchorStyles.Left;
                docFileTablePanel.Controls.Add(text, 1, row);

                Label l = new Label();
                l.Text = doc.ItemID;
                l.Anchor = AnchorStyles.Left;
                l.TextAlign = ContentAlignment.MiddleLeft;
                docFileTablePanel.Controls.Add(l, 2, row);

            }
            this.ResumeLayout();
        }

        private int AddTableRow()
        {
            int result = docFileTablePanel.RowCount++;
            RowStyle style = new RowStyle(SizeType.Absolute, 20.0F);
            docFileTablePanel.RowStyles.Add(style);
            return result;
        }

        private void projectPanel_SizeChanged(object sender, EventArgs e)
        {
            pubControlPanel.Top = 0;
            pubControlPanel.Left = (projectPanel.Width - pubControlPanel.Width) / 2;
            pubControlPanel.Height = projectPanel.Height;
        }

        private void documentPanel_SizeChanged(object sender, EventArgs e)
        {
            docControlPanel.Top = 0;
            docControlPanel.Left = (documentPanel.Width - docControlPanel.Width) / 2;
            docControlPanel.Height = documentPanel.Height;
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
