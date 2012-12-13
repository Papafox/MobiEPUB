using System;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Globalization;
using System.Collections;
using System.Windows.Forms;

namespace MobiEPUB
{
    public class Settings
    {
        private bool m_Changed = false;
        private String m_eyeCatcher;
        private String m_defaultDir;
        private String m_defaultLang;
        private String m_defaultEncoding;
        private Point m_Location;
        private Size m_Size;
        private String[] m_availableLanguages;
        private String[] m_availableEncodings;

        public Settings()
        {
            // Check the eye catcher to make sure its a valid settings file
            String eyeCatcher = Properties.MobiEPUB.Default.EyeCatcher;
            if (eyeCatcher != "MobiEPUB")
                throw new Exception("Invalid Settings");

            // Load the settings from the properties
            m_eyeCatcher = Properties.MobiEPUB.Default.EyeCatcher;
            m_defaultDir = Properties.MobiEPUB.Default.DefaultDir;
            m_defaultLang = Properties.MobiEPUB.Default.DefaultLang;
            m_defaultEncoding = Properties.MobiEPUB.Default.DefaultEncoding;
            m_Location = Properties.MobiEPUB.Default.Location;
            m_Size = Properties.MobiEPUB.Default.Size;

            // Set defaults for language, encoding, location and size
            if (m_defaultLang == null || m_defaultLang.Equals(""))
            {
                m_defaultLang = CultureInfo.CurrentCulture.EnglishName;
                m_Changed = true;
            }
            if (m_defaultEncoding == null || m_defaultEncoding.Equals(""))
            {
                m_defaultEncoding = Encoding.Default.WebName;
                m_Changed = true;
            }
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            Rectangle formRect = new Rectangle(m_Location, m_Size);
            if (!screen.Contains(formRect))
            {
                m_Size = new Size(Math.Min(500, (screen.Width * 100) / 60), Math.Min(300, (screen.Height * 100) / 60));
                m_Location = new Point(Math.Max(200, screen.Width * 100) / 20, Math.Max(200, (screen.Height * 100) / 20));
            }

            // Load the available languages and encodings
            m_availableLanguages = getLanguages();
            m_availableEncodings = getEncodings();
        }

        public void Save()
        {
            if (m_Changed)
            {
                Properties.MobiEPUB.Default.DefaultDir = m_defaultDir;
                Properties.MobiEPUB.Default.DefaultEncoding = m_defaultEncoding;
                Properties.MobiEPUB.Default.DefaultLang = m_defaultLang;
                Properties.MobiEPUB.Default.Location = m_Location;
                Properties.MobiEPUB.Default.Size = m_Size;
                Properties.MobiEPUB.Default.Save();
            }
        }

        public bool Changed
        {
            get
            {
                return m_Changed;
            }
        }

        public String DefaultDir
        {
            get
            {
                return m_defaultDir;
            }

            set
            {
                m_defaultDir = value;
                m_Changed = true;
            }
        }

        public String DefaultLanguage
        {
            get
            {
                return m_defaultLang;
            }

            set
            {
                String lang = value;
                if (m_availableLanguages.Contains(lang))
                    m_defaultLang = lang;
                else
                    throw new Exception("Unknown language: " + lang);
                m_Changed = true;

            }
        }

        public String DefaultEncoding
        {
            get
            {
                return m_defaultEncoding;
            }

            set
            {
                String enc = value;
                if (m_defaultEncoding.Contains(enc))
                    m_defaultEncoding = enc;
                else
                    throw new Exception("Unknown encoding: " + enc);
                m_Changed = true;
            }
        }

        public Point Location
        {
            get
            {
                return m_Location;
            }

            set
            {
                m_Location = value;
                m_Changed = true;
            }
        }

        public Size Size
        {
            get
            {
                return m_Size;
            }

            set
            {
                m_Size = value;
                m_Changed = true;
            }
        }

        public String[] AvailableLanguages
        {
            get
            {
                return m_availableLanguages;
            }
        }

        public String[] AvailableEncodings
        {
            get
            {
                return m_availableEncodings;
            }
        }


        private String[] getLanguages()
        {
            // Setup the return array of supported languages
            ArrayList result = new ArrayList();

            // Get a list of all cultures from .Net and return sorted
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo culture in cultures)
                result.Add(culture.EnglishName);
            result.Sort();
            return result.ToArray(typeof(string)) as string[];
        }

        private String[] getEncodings()
        {
            String[] ret = Properties.MobiEPUB.Default.AvailableEncodings.Split(',');
            if (ret == null)
                ret = new String[2] { "Windows-1252", "UTF-8" };
            return ret;
        }

    }
}
