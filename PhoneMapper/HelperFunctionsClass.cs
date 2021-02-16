using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneMapper
{
    public static class HelperFunctionsClass
    {
        public static List<DataRow> getTableFromText(string content, string lastNRows)
        {
            List<DataRow> table = new List<DataRow>();

            string[] entries = content.Split(new string[] { "Location=" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < entries.Length; i++)
            {
                try
                {
                    string[] properties = entries[i].Split(new char[] { '\n' });

                    string firstline = properties[0];
                    string latitude = firstline.Split(new char[] { ',' })[0];
                    string longitude = firstline.Split(new char[] { ',' })[1];

                    string secondline = properties[1];
                    string thirdline = properties[2];
                    string date = secondline.Replace("Date=", "");
                    string time = thirdline.Replace("Time=", "");

                    string formattedDateTime = DateTime.Parse(date + " " + time).ToString("ddd | MMM d, yyyy | hh:mm:ss tt");

                    string fourthline = properties[3];
                    string battery = fourthline.Replace("Battery=", "");

                    string fifthline = properties[4];
                    string uptime = fifthline.Replace("Uptime=", "");

                    string sixthline = properties[5];
                    string internalFreeMB = sixthline.Replace("Internal free=", "");

                    string seventhline = properties[6];
                    string internalTotalMB = seventhline.Replace("Internal total=", "");

                    DataRow dataRow = new DataRow(formattedDateTime, latitude, longitude, battery, uptime);

                    table.Add(dataRow);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Exception while processing an entry of the log file. Skipped the entry.");
                }
                
            }

            int lastNRows_;
            if (int.TryParse(lastNRows, out lastNRows_)) //if lastNRows is a numeric value
            {
                if (lastNRows_ > 0 && lastNRows_ < table.Count)
                    table.RemoveRange(0, table.Count - lastNRows_);
            }


            return table;
        }

        public static List<ListViewItem> getListViewItemsFromTable(List<DataRow> table)
        {
            List<ListViewItem> listViewItems = new List<ListViewItem>();
            table.ForEach((dataRow) =>
            {
                listViewItems.Add(dataRow.ToListViewItem());
            });
            return listViewItems;
        }


    }
}
