using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PhoneMapper
{
    public class SettingsControllerClass
    {
        public string SettingsFile { get; set; }


        public SettingsControllerClass(string settingsFile)
        {
            this.SettingsFile = settingsFile;
        }

        public void SaveSettings(Settings settings)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
            try
            {
                using (FileStream fileStream = new FileStream(this.SettingsFile, FileMode.Create, FileAccess.Write))
                {
                    xmlSerializer.Serialize(fileStream, settings);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in SaveSettings()");
            }

        }

        public Settings LoadSettings()
        {
            Settings settings = new Settings(); //instantiate a blank(default) Settings object

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));

            try
            {
                using (FileStream fileStream = new FileStream(this.SettingsFile, FileMode.Open, FileAccess.Read))
                {
                    settings = (Settings)xmlSerializer.Deserialize(fileStream);
                }
            }
            catch(FileNotFoundException fnfex)
            {
                throw fnfex;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occurred in LoadSettings()");
            }

            return settings;

        }


    }
}
