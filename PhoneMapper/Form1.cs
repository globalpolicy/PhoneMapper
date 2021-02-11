using GMap.NET.MapProviders;
using System;
using System.Windows.Forms;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

namespace PhoneMapper
{
    public partial class Form1 : Form, IFileDownloads
    {
        private MapFunctionsClass mapFunctionsClass; //instance of the map control editor helper class should exist as long as the form exists
        private Settings settings; //instance of the Settings class should exist as long as the form exists. this should remain updated with any changes to settings made by the user
        private SettingsControllerClass settingsControllerClass; //instance of the SettingsControllerClass should exist as long as the form exists
        private InstructionsForm instructionForm; //instance of the Instructions form

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSettings();
            LoadData();
        }

        public void LoadSettings()
        {
            settings = new Settings(); //instantiate the Settings class

            settingsControllerClass = new SettingsControllerClass(ConstantsClass.SettingFileName); //instantiate the Settings controller class

            try
            {
                settings = settingsControllerClass.LoadSettings(); //try to load settings from the settings file
            }
            catch (FileNotFoundException fnfex)
            {
                Debug.WriteLine("Settings file not found.");
                //populate settings with default values
                settings.FileURL = ConstantsClass.DefaultLogFileURL;
                settings.LoadLatestNum = ConstantsClass.DefaultLoadLatestNum;
                settings.RecenterOnSelection = ConstantsClass.DefaultRecenterOnSelection;
                settings.ShowMarkerTooltips = ConstantsClass.DefaultShowMarkerTooltips;
            }

            //update controls on forms with the settings' values
            toolStripTextBoxLogFileURL.Text = settings.FileURL;
            toolStripTextBoxDataNumbersToLoad.Text = settings.LoadLatestNum.ToString();
            recenterOnSelectionToolStripMenuItem.Checked = settings.RecenterOnSelection;
            showTooltipToolStripMenuItem.Checked = settings.ShowMarkerTooltips;
        }

        public void LoadData()
        {

            mapFunctionsClass = new MapFunctionsClass(gMapControl1); //instantiate the MapFunctionsClass helper class

            toolStripStatusLabel1.Text = "Downloading latest phone log...";

            NetworkClass.DownloadFile(settings.FileURL, this); //ask to download the phone log file. upon success, the FileDownloaded() callback method will be called

        }

        public void FileDownloaded(string contents)
        {

            toolStripStatusLabel1.Text = "Ready";

            //process the string contents into a list of DataRows
            List<DataRow> table = HelperFunctionsClass.getTableFromText(contents, toolStripTextBoxDataNumbersToLoad.Text);

            if (table.Count > 0)
            {
                //process the list of DataRows into a list of ListViewItems
                List<ListViewItem> listViewItems = HelperFunctionsClass.getListViewItemsFromTable(table);

                //add the ListViewItems to the listview control
                listView1.Items.Clear(); //clear pre-existing items from the listview table
                listView1.Items.AddRange(listViewItems.ToArray());
                //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize); apparently, this command is a CPU hog and makes the app's UI laggy while populating the listview table
                toolStripStatusLabel2.Text = $"{listView1.Items.Count} records added!";

                //center the map at the last position
                mapFunctionsClass.centerMapAt(Double.Parse(table[table.Count - 1].Latitude), Double.Parse(table[table.Count - 1].Longitude));

                //place markers for all positions
                for (int i = 0; i < table.Count; i++)
                {
                    DataRow dataRow = table[i];
                    mapFunctionsClass.putMarkerAt(i, dataRow.DateTime, Double.Parse(dataRow.Latitude), Double.Parse(dataRow.Longitude), GMarkerGoogleType.blue_pushpin);
                    mapFunctionsClass.setTooltipMode(showTooltipToolStripMenuItem.Checked ? MarkerTooltipMode.OnMouseOver : MarkerTooltipMode.Never);
                };
            }
            else
            {
                toolStripStatusLabel1.Text = "No entries found in log file";
            }

        }

        public void FileNotDownloaded()
        {
            MessageBox.Show("File could not be downloaded!\nPlease check the download link.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            toolStripStatusLabel1.Text = "File download was unsuccessful :(";
        }

        public void NoInternet()
        {
            MessageBox.Show("Please check your internet connection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            toolStripStatusLabel1.Text = "No internet connection";
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //place a red marker on the map corresponding to the selected rows of the listview

            /* NOTE:
             * This method is triggered once on every selection event, regardless of whether the selection is single or multiple
             * (using CTRL/SHIFT keys)
             */

            ListView.SelectedListViewItemCollection selectedItems = listView1.SelectedItems;
            List<(string, double, double)> latLngList = new List<(string, double, double)>();

            for (int i = 0; i < selectedItems.Count; i++)
            {
                ListViewItem selectedItem = selectedItems[i];
                double lat = Double.Parse(selectedItem.SubItems[1].Text);
                double lng = Double.Parse(selectedItem.SubItems[2].Text);
                string dateTime = selectedItem.SubItems[0].Text;

                latLngList.Add((dateTime, lat, lng)); //add (datetime,lat,long) tuple to the list
            }

            mapFunctionsClass.putSelectionMarkerAt(latLngList, GMarkerGoogleType.yellow_small); //place selection markers from this list on the map

            if (recenterOnSelectionToolStripMenuItem.Checked)
            {
                try
                {
                    mapFunctionsClass.centerMapAt(latLngList[latLngList.Count - 1].Item2, latLngList[latLngList.Count - 1].Item3); //center the map at the last position from the selected list
                }
                catch (ArgumentOutOfRangeException argumentOutOfRangeException)
                {

                }
            }


        }



        private void gMapControl1_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            try
            {
                listView1.Focus();

                listView1.SelectedItems.Clear(); //clear previous selections

                listView1.Items[(int)item.Tag].Selected = true; //select the clicked marker's corresponding row in listview table
                listView1.EnsureVisible((int)item.Tag); //scroll to the selection

                string lat = listView1.Items[(int)item.Tag].SubItems[1].Text;
                string lng = listView1.Items[(int)item.Tag].SubItems[2].Text;
                DateTime dateTime = DateTime.Parse(listView1.Items[(int)item.Tag].SubItems[0].Text.Replace("|", ""));
                string dateTimeInfo = dateTime.ToString("MMMM dd, yyyy (dddd) | hh:mm:ss tt");

                toolStripStatusLabel1.Text = $"({lat},{lng}) @ {dateTimeInfo}";
            }
            catch (NullReferenceException nrex) //this exception occurs when the user clicks on a selection marker(which has nothing in its Tag property) instead of a default marker
            {
                Debug.WriteLine("NullReferenceException thrown from gMapControl1_OnMarkerClick");
            }

        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void toolStripTextBoxDataNumbersToLoad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                int.TryParse(toolStripTextBoxDataNumbersToLoad.Text, out int tmp);
                settings.LoadLatestNum = tmp; //should be 0 if the text was not a number

                LoadData();
                settingsToolStripMenuItem.HideDropDown();
            }

        }

        private void showTooltipToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (mapFunctionsClass != null) //this method also gets triggered when first applying the saved settings. so this check is necessary
                mapFunctionsClass.setTooltipMode(showTooltipToolStripMenuItem.Checked ? MarkerTooltipMode.OnMouseOver : MarkerTooltipMode.Never);
            settings.ShowMarkerTooltips = showTooltipToolStripMenuItem.Checked;
        }

        private void recenterOnSelectionToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            settings.RecenterOnSelection = recenterOnSelectionToolStripMenuItem.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            settingsControllerClass.SaveSettings(settings);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveImagetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = saveFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    gMapControl1.ToImage().Save(saveFileDialog1.FileName);
                    toolStripStatusLabel1.Text = $"Map saved to {saveFileDialog1.FileName}";
                }
                catch (Exception ex)
                {
                    toolStripStatusLabel1.Text = "Error saving image!";
                    Debug.WriteLine("Exception while saving image");
                }
            }
        }


        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Author: s0ft\nBlog: c0dew0rth.blogspot.com\nGithub: globalpolicy\nContact: yciloplabolg@gmail.com", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instructionForm = new InstructionsForm();
            instructionForm.ShowDialog();
        }

        private void toolStripTextBoxLogFileURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                settings.FileURL = toolStripTextBoxLogFileURL.Text;

                LoadData();
                settingsToolStripMenuItem.HideDropDown();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string output = "";
            foreach (ListViewItem row in listView1.SelectedItems)
            {
                for (int i = 0; i < row.SubItems.Count; i++)
                {
                    output += row.SubItems[i].Text + "\t";
                }
                output = output.Trim();
                output += "\n";
            }
            Clipboard.SetText(output);
            toolStripStatusLabel2.Text = $"{listView1.SelectedItems.Count} rows copied to clipboard!";
        }
    }
}
