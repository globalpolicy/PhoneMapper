using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneMapper
{
    public class DataRow
    {
        public string DateTime { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Battery { get; set; }

        public string Uptime { get; set; }

        public DataRow(string dateTime, string latitude, string longitude, string battery, string uptime)
        {
            this.DateTime = dateTime;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Battery = battery;
            this.Uptime = uptime;
        }

        public ListViewItem ToListViewItem()
        {
            ListViewItem listViewItem;
            listViewItem = new ListViewItem(new string[] { this.DateTime, this.Latitude, this.Longitude, this.Battery, this.Uptime });
            return listViewItem;
        }
    }
    
    [Serializable]
    public class Settings
    {
        public bool RecenterOnSelection { get; set; }
        public bool ShowMarkerTooltips { get; set; }
        public int LoadLatestNum { get; set; }
        public string FileURL { get; set; }
    }
}
