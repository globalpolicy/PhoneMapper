using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneMapper
{
    public static class ConstantsClass
    {
        static readonly string _settingFileName = "PhoneMapperConfig.xml";
        static readonly string _defaultLogFileURL = "https://drive.google.com/uc?export=download&id=xxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        static readonly int _defaultLoadLatestNum = 0;
        static readonly bool _defaultRecenterOnSelection = false;
        static readonly bool _defaultShowMarkerTooltips = false;

        public static string SettingFileName
        {
            get
            {
                return _settingFileName;
            }
        }

        public static string DefaultLogFileURL
        {
            get
            {
                return _defaultLogFileURL;
            }
        }

        public static int DefaultLoadLatestNum
        {
            get
            {
                return _defaultLoadLatestNum;
            }
        }

        public static bool DefaultRecenterOnSelection
        {
            get
            {
                return _defaultRecenterOnSelection;
            }
        }

        public static bool DefaultShowMarkerTooltips
        {
            get
            {
                return _defaultShowMarkerTooltips;
            }
        }


    }
}
