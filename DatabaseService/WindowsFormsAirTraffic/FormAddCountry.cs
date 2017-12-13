using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAirTraffic
{
    public partial class FormAddCountry : Form
    {
        private readonly MainForm FormCountriesList;
        public FormAddCountry(MainForm FormCountries)
        {
            FormCountriesList = FormCountries;
            InitializeComponent();
        }
    }
}
