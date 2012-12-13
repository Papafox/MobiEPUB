﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MobiEPUB
{
    public partial class MainForm : Form
    {
        private Settings m_Settings;

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
            // Restore the main form to the last location
            this.Size = m_Settings.Size;
            this.Location = m_Settings.Location;

            // Make sure form cannot be samller than pubControlPanel;
            this.MinimumSize = new Size(pubControlPanel.Width, pubControlPanel.Height + toolStrip.Height + menuStrip.Height);

            // Initialize the various panels
            projectPanel_Load(sender, e);

            // Initially show the project view.  Hide the others.
            documentPanel.Visible = false;
            buildPanel.Visible = false;
            projectPanel.Visible = true;
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

        private void projectPanel_SizeChanged(object sender, EventArgs e)
        {
            pubControlPanel.Top = 0;
            pubControlPanel.Left = (projectPanel.Width - pubControlPanel.Width) / 2;
            pubControlPanel.Height = projectPanel.Height;
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

    }
}