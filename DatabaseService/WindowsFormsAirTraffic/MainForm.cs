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
        public List<Country> lCountries;
        Crud Crud = new Crud();
        public MainForm()
        {
            InitializeComponent();
            lCountries = Crud.GetAllCountries();
            List<String> lAllCountries = lCountries.Where(o => o.sCountryName != "").Select(o => o.sCountryName).Distinct().ToList();
            lAllCountries.Insert(0, "Sve države");
            comboBoxCountries.DataSource = lAllCountries;
        }

        private void comboBoxCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sCountry = (string)comboBoxCountries.SelectedItem;

            lCountries = Crud.GetAllCountries();
            if (sCountry != "Sve države")
            {
                lCountries = lCountries.Where(o => o.sCountryName == sCountry).ToList();
                comboBoxCountries.DataSource = lCountries;
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

        /*private void btnAddCountry_Click(object sender, EventArgs e)
        {
            FormAddCountry FormAddCountry = new FormAddCountry(this);
            FormAddCountry.Show();
        }*/
    }
}
