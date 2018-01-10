using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using DatabaseService;
using RESTFlights;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using System.Threading;
using System.Configuration;

namespace WindowsFormsAirTraffic
{
    
    public partial class MainForm : Form
    {
        public List<String> lCountries;
        public List<Flight> lFlights;
        public List<Country> lAvCountries;
        Rest Rest = new Rest();
        Crud Crud = new Crud();

        public MainForm()
        {
            InitializeComponent();

            //DATA GRID DRŽAVE
            lAvCountries = Crud.GetAvailableCountries();
            dataGridViewCountries.DataSource = lAvCountries;

            //DATA GRID LETOVI
            lFlights = Rest.GetFlights();
            dataGridViewFlights.DataSource = Rest.GetFlights();

            lCountries = Rest.GetAllCountries();
            List<String> lAllCountries = lCountries.ToList();
            lAllCountries.Insert(0, "Sve države");

            comboBoxCountries.DataSource = lAllCountries;

            //dodavanje buttona u kolonu
            DataGridViewImageColumn oDeleteButton = new DataGridViewImageColumn();
            oDeleteButton.Image = Image.FromFile("remove.png");
            oDeleteButton.Width = 20;
            oDeleteButton.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCountries.Columns.Add(oDeleteButton);

            dataGridViewCountries.AutoGenerateColumns = false;
        }

        private void comboBoxCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sCountry = (string)comboBoxCountries.SelectedItem;

            lCountries = Rest.GetAllCountries();
            if (sCountry != "Sve države")
            {
                lCountries = lCountries.ToList();
            }
            else
            {
                comboBoxCountries.DataSource = lCountries;
            }
        }

        private void izlazIzProgramaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAddCountry_Click(object sender, EventArgs e)
        {
            Country oCountry = new Country();
            oCountry.sCountryName = (string)comboBoxCountries.SelectedItem;
            Crud Crud = new Crud();
            Crud.AddCountry(oCountry);

            string name = (string)comboBoxCountries.SelectedItem;
            lCountries.Remove(name);
            comboBoxCountries.DataSource = lCountries;
            dataGridViewCountries.DataSource = Crud.GetAvailableCountries();
        }

        private void dataGridViewCountries_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewCountries.Rows[e.RowIndex].Selected = true;

            if (dataGridViewCountries.CurrentCell.ColumnIndex.Equals(2) && e.RowIndex != -1) 
            {
                Country oCountry = new Country();
                oCountry.nCountryID = Convert.ToInt32(dataGridViewCountries.Rows[e.RowIndex].Cells[0].Value);
                Crud Crud = new Crud();
                Crud.DeleteCountry(oCountry);
            }
            dataGridViewCountries.DataSource = Crud.GetAvailableCountries();
        }

        private void gMapAirTraffic_Load(object sender, EventArgs e)
        {
            lFlights = Rest.GetFlights();

            gMapAirTraffic.MapProvider = GMap.NET.MapProviders.GoogleHybridMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gMapAirTraffic.ShowCenter = false;
            gMapAirTraffic.DragButton = MouseButtons.Left;

            SetFlightsOnMap();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Mateja Podravac" + Environment.NewLine + "VSMTI, 2017.");
        }

        private void SetFlightsOnMap()
        {
            lFlights = Rest.GetFlights();

            GMapOverlay markers = new GMapOverlay("markers");
            for (int i = 0; i < lFlights.Count; i++)
            {
                GMapMarker marker = new GMarkerGoogle(
                new PointLatLng(lFlights[i].fLatitude, lFlights[i].fLongitude),
                new Bitmap("plane.png"));
                markers.Markers.Add(marker);
            }
            gMapAirTraffic.Overlays.Add(markers);
        }

       /* public void ScheduleService()
        {
            // Objekt klase Timer
            System.Threading.Timer Schedular = new System.Threading.Timer(new TimerCallback(SchedularCallback));
            // Postavljanje vremena 'po defaultu'
            DateTime scheduledTime = DateTime.MinValue;
            string mode = ConfigurationManager.AppSettings["Mode"].ToUpper();
            if (mode == "DAILY")
            {
                //Dohvati vrijeme iz konfiguracijske datoteke.
                scheduledTime =
                DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]);
                if (DateTime.Now > scheduledTime)
                {
                    //Ukoliko je termin prošao, dodaj 1 dan.
                    scheduledTime = scheduledTime.AddDays(1);
                }
            }
            if (mode.ToUpper() == "INTERVAL")
            {
                // Dohvati vrijeme iz konfiguracijske datoteke
                int intervalMinutes =
                Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);
                //Postavi zakazano vrijeme za jednu minutu od trenutnog vremena.
                scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                if (DateTime.Now > scheduledTime)
                {
                    //Ukoliko je termin prošao, dodaj 1 minutu.
                    scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                }
            }
            // Vremenski interval
            TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
            string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3}seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            //Razlika između trenutnog vremena i planiranog vremena
            int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);
            // Promjena vremena izvršavanja metode povratnog poziva.
            Schedular.Change(dueTime, Timeout.Infinite);
        }
        private void SchedularCallback(object e)
        {
            lFlights = Rest.GetFlights();
            gMapAirTraffic.Refresh();
            SetFlightsOnMap();
            gMapAirTraffic.Refresh();
            ScheduleService();
        }*/
    }
}
