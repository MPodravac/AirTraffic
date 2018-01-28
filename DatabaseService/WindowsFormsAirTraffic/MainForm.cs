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
using System.Diagnostics;

namespace WindowsFormsAirTraffic
{
    
    public partial class MainForm : Form
    {
        public List<String> lCountries;
        public List<Flight> lCurrentFlightsMap = new List<Flight>();
        public List<Flight> lCurrentFlightsDatagrid;
        public List<Flight> lPastFlights;
        public List<Country> lAvCountries;
        public GMapOverlay markers;

        Rest Rest = new Rest();
        Crud Crud = new Crud();

        public MainForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            string sPretragaDrzave = inptPretraziDrzave.Text;

            //DATA GRID DRŽAVE
            dataGridViewCountries.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewCountries.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            lAvCountries = Crud.GetAvailableCountries();
            dataGridViewCountries.DataSource = lAvCountries;

            //DATA GRID LETOVI
            lCurrentFlightsDatagrid = Rest.GetFlights(sPretragaDrzave);
            dataGridViewFlights.DataSource = Rest.GetFlights(sPretragaDrzave);

            //COMBOBOX DRŽAVE
            lCountries = Rest.GetAllCountries();
            List<String> lAllCountries = lCountries.ToList();
            lAllCountries.Insert(0, "All countries");

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
            lCountries = Rest.GetAllCountries();

            string sCountry = (string)comboBoxCountries.SelectedItem;

            if (sCountry != "All countries")
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
            comboBoxCountries.DataSource = Rest.GetAllCountries();
        }

        private void gMapAirTraffic_Load(object sender, EventArgs e)
        {
            gMapAirTraffic.MapProvider = GMap.NET.MapProviders.BingHybridMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gMapAirTraffic.ShowCenter = false;
            gMapAirTraffic.DragButton = MouseButtons.Left;

            SetFlightsOnMap();
            ScheduleService();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Mateja Podravac" + Environment.NewLine + "VSMTI, 2017.");
        }

        public bool ControlInvokeRequired(Control c, Action a)
        {
            if (c.InvokeRequired) c.Invoke(new MethodInvoker(delegate { a(); }));
            else return false;

            return true;
        }

        private void SetFlightsOnMap()
        {
            string sPretragaLeta = inptPretraziLetove.Text;

            if (ControlInvokeRequired(gMapAirTraffic, () => SetFlightsOnMap())) return;
            gMapAirTraffic.Overlays.Clear();

            if (!(markers is GMapOverlay))
            {
                markers = new GMapOverlay("markers");
            }

            lCurrentFlightsMap = Rest.GetFlights(sPretragaLeta);
            markers.Clear();

            GMarkerGoogle marker;
            for (int i = 0; i < lCurrentFlightsMap.Count; i++)
            {
                if(lCurrentFlightsMap[i].fHeading >= 0 && lCurrentFlightsMap[i].fHeading <= 90)
                {
                    marker = new GMarkerGoogle(
                    new PointLatLng(lCurrentFlightsMap[i].fLatitude, lCurrentFlightsMap[i].fLongitude),
                    new Bitmap("plane0-90.png"));
                    markers.Markers.Add(marker);
                }
                if (lCurrentFlightsMap[i].fHeading > 90 && lCurrentFlightsMap[i].fHeading <= 180)
                {
                    marker = new GMarkerGoogle(
                    new PointLatLng(lCurrentFlightsMap[i].fLatitude, lCurrentFlightsMap[i].fLongitude),
                    new Bitmap("plane90-180.png"));
                    markers.Markers.Add(marker);
                }
                if (lCurrentFlightsMap[i].fHeading > 180 && lCurrentFlightsMap[i].fHeading <= 270)
                {
                    marker = new GMarkerGoogle(
                    new PointLatLng(lCurrentFlightsMap[i].fLatitude, lCurrentFlightsMap[i].fLongitude),
                    new Bitmap("plane180-270.png"));
                    markers.Markers.Add(marker);
                }
                if (lCurrentFlightsMap[i].fHeading > 270 && lCurrentFlightsMap[i].fHeading <= 360)
                {
                    marker = new GMarkerGoogle(
                    new PointLatLng(lCurrentFlightsMap[i].fLatitude, lCurrentFlightsMap[i].fLongitude),
                    new Bitmap("plane270-360.png"));
                    markers.Markers.Add(marker);
                }
            }
            gMapAirTraffic.Overlays.Add(markers);
        }

         public void ScheduleService()
         {
             // Objekt klase Timer
             System.Threading.Timer Schedular = new System.Threading.Timer(new TimerCallback(SchedularCallback));
             // Postavljanje vremena 'po defaultu'
             DateTime scheduledTime = DateTime.MinValue;
             
             // Dohvati vrijeme iz konfiguracijske datoteke
             float intervalMinutes =
             Convert.ToSingle(ConfigurationManager.AppSettings["IntervalMinutes"]);
            //Postavi zakazano vrijeme za jednu minutu od trenutnog vremena.
            scheduledTime = DateTime.Now.AddSeconds(5);
             if (DateTime.Now > scheduledTime)
                 {
                     //Ukoliko je termin prošao, dodaj 1 minutu.
                     scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                 }
       
             // Vremenski interval
             TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
             //Razlika između trenutnog vremena i planiranog vremena
             int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);
             // Promjena vremena izvršavanja metode povratnog poziva.
             Schedular.Change(dueTime, Timeout.Infinite);
         }
         private void SchedularCallback(object e)
         {
             SetFlightsOnMap();
             ScheduleService();
        }

        private void btnPretrazi_Click(object sender, EventArgs e)
        {
            string sPretragaDrzave = inptPretraziDrzave.Text;
            lCurrentFlightsDatagrid = Rest.GetFlights(sPretragaDrzave);
            dataGridViewFlights.DataSource = Rest.GetFlights(sPretragaDrzave);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sPretragaLeta = inptPretraziLetove.Text;
            SetFlightsOnMap();
        }
    }
}
