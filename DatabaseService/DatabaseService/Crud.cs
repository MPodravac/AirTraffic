using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace DatabaseService
{
    public class Crud
    {
       public List <Country> GetAvailableCountries()
        {
            List<Country> lAirTrafficCountries = new List<Country>();
            String sSqlConnectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            using (DbConnection oConnection = new SqlConnection(sSqlConnectionString))
            using (DbCommand oCommand = oConnection.CreateCommand())
            {
                oCommand.CommandText = "SELECT * FROM AIRTRAFFIC_COUNTRIES";
                oConnection.Open();
                using (DbDataReader oReader = oCommand.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        lAirTrafficCountries.Add(new Country()
                        {
                            nCountryID = (int)oReader["COUNTRY_ID"],
                            sCountryName = (string)oReader["COUNTRY_NAME"],
                        });
                    }
                }
            }
            return lAirTrafficCountries;
        }

        public void AddCountry(Country oCountry)
        {
            String sSqlConnectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            using (DbConnection oConnection = new SqlConnection(sSqlConnectionString))
            using (DbCommand oCommand = oConnection.CreateCommand())
            {
                oCommand.CommandText = "INSERT INTO AIRTRAFFIC_COUNTRIES (COUNTRY_NAME) VALUES('" + oCountry.sCountryName + "');";
                oConnection.Open();
                using (DbDataReader oReader = oCommand.ExecuteReader())
                {
                }
            }
        }

        public void DeleteCountry(Country oCountry)
        {
            String sSqlConnectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            using (DbConnection oConnection = new SqlConnection(sSqlConnectionString))
            using (DbCommand oCommand = oConnection.CreateCommand())
            {
                oCommand.CommandText = "DELETE FROM AIRTRAFFIC_COUNTRIES WHERE COUNTRY_ID = " + oCountry.nCountryID;
                oConnection.Open();
                using (DbDataReader oReader = oCommand.ExecuteReader())
                {
                }
            }
        }
    }
}
