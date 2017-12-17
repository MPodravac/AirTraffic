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


namespace WindowsFormsAirTraffic
{
    
    public partial class MainForm : Form
    {
        public List<String> lCountries;
        public List<Flight> lFlights;
        Rest Rest = new Rest();

        public MainForm()
        {
            InitializeComponent();

            lCountries = Rest.GetAllCountries();
            List<String> lAllCountries = lCountries.ToList();
            lAllCountries.Insert(0, "Sve države");

            comboBoxCountries.DataSource = lAllCountries;

            //DATA GRID
            lFlights = Rest.GetFlights();
            dataGridViewFlights.DataSource = lFlights;
        }

        private void comboBoxCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sCountry = (string)comboBoxCountries.SelectedItem;

            lCountries = Rest.GetAllCountries();
            if (sCountry != "Sve države")
            {
                lCountries = lCountries.ToList();
                //comboBoxCountries.DataSource = lCountries;
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
        }

        private void btnDeleteCountry_Click(object sender, EventArgs e)
        {
            Country oCountry = new Country();
            oCountry.sCountryName = (string)comboBoxCountries.SelectedItem;
            Crud Crud = new Crud();
            Crud.DeleteCountry(oCountry);
        }
    }
}
