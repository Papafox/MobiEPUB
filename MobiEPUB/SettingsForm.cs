using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MobiEPUB
{
    public partial class SettingsForm : Form
    {
        private Settings m_Settings;
        private bool     m_Changed = false;

        public SettingsForm(Settings s)
        {
            InitializeComponent();
            m_Settings = s;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            // Place the version in the title
            this.Text += String.Format(" (Version {0})", AssemblyVersion());

            // Get the default directory
            textDefaultDir.Text = m_Settings.DefaultDir;

            // Get a list of supported languages and select the specified default
            comboDefaultLang.Items.AddRange(m_Settings.AvailableLanguages);
            comboDefaultLang.SelectedIndex = comboDefaultLang.Items.IndexOf(m_Settings.DefaultLanguage);
            if (comboDefaultLang.SelectedIndex == -1)
                comboDefaultLang.SelectedIndex = 0;

            // Get a list of supported encoding and select the default.
            comboDefaultEnc.Items.AddRange(m_Settings.AvailableEncodings);
            comboDefaultEnc.SelectedIndex = comboDefaultEnc.Items.IndexOf(m_Settings.DefaultEncoding);
            if (comboDefaultEnc.SelectedIndex == -1)
                comboDefaultEnc.SelectedIndex = 0;

            okBtn.Focus();
        }

        private void Text_Changed(object sender, EventArgs e)
        {
            m_Changed = true;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (m_Changed)
            {
                m_Settings.DefaultDir = textDefaultDir.Text;
                m_Settings.DefaultLanguage = comboDefaultLang.SelectedText;
                m_Settings.DefaultEncoding = comboDefaultEnc.SelectedText;
                m_Settings.Save();
            }
        }

        private string AssemblyVersion()
        {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

    }
}
