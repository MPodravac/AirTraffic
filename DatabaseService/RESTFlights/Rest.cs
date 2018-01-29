using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DatabaseService;

namespace RESTFlights
{
    public class Rest
    {
        public static string CallRestMethod(string url)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();
            return result;
        }

        public List<Flight> GetFlights(string country)
        {
            Crud Crud = new Crud();
            List<Country> lCountries;
            lCountries = Crud.GetAvailableCountries();

            List<Flight> lRESTFlights = new List<Flight>();
            string sUrl = System.Configuration.ConfigurationManager.AppSettings["RestApiUrl"];
            string sJson = CallRestMethod(sUrl);

            JObject json = JObject.Parse(sJson);
            var oFlights = json["states"].ToList();

            if(country == "All countries" || String.IsNullOrEmpty(country))
            {
                for (int i = 0; i < oFlights.Count; i++)
                {
                    for (int j = 0; j < lCountries.Count; j++)
                    {
                        if (lCountries[j].sCountryName == (string)oFlights[i][2])
                        {
                            lRESTFlights.Add(new Flight
                            {
                                sIcao24 = (string)oFlights[i][0],
                                sCallSign = (string)oFlights[i][1],
                                sCountry = (string)oFlights[i][2],
                                nTimePosition = (int)(oFlights[i][3].Type == JTokenType.Null ? -1 : oFlights[i][3]), //skraćeni if
                                nLastContact = (int)(oFlights[i][4].Type == JTokenType.Null ? -1 : oFlights[i][4]),
                                fLongitude = (float)(oFlights[i][5].Type == JTokenType.Null ? -1 : oFlights[i][5]),
                                fLatitude = (float)(oFlights[i][6].Type == JTokenType.Null ? -1 : oFlights[i][6]),
                                fGeoAltitude = (float)(oFlights[i][7].Type == JTokenType.Null ? -1 : oFlights[i][7]),
                                bOnGround = (bool)oFlights[i][8],
                                fVelocity = (float)(oFlights[i][9].Type == JTokenType.Null ? -1 : oFlights[i][9]),
                                fHeading = (float)(oFlights[i][10].Type == JTokenType.Null ? -1 : oFlights[i][10]),
                                fVerticalRate = (float)(oFlights[i][11].Type == JTokenType.Null ? -1 : oFlights[i][11]),
                                fBaroAltitude = (float)(oFlights[i][13].Type == JTokenType.Null ? -1 : oFlights[i][13]),
                                sSquawk = (string)oFlights[i][14],
                                bSpi = (bool)oFlights[i][15],
                                nPositionSource = (int)(oFlights[i][16].Type == JTokenType.Null ? -1 : oFlights[i][16]),
                            });
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < oFlights.Count; i++)
                {
                    if (country == (string)oFlights[i][2])
                    {
                        lRESTFlights.Add(new Flight
                        {
                            sIcao24 = (string)oFlights[i][0],
                            sCallSign = (string)oFlights[i][1],
                            sCountry = (string)oFlights[i][2],
                            nTimePosition = (int)(oFlights[i][3].Type == JTokenType.Null ? -1 : oFlights[i][3]), //skraćeni if
                            nLastContact = (int)(oFlights[i][4].Type == JTokenType.Null ? -1 : oFlights[i][4]),
                            fLongitude = (float)(oFlights[i][5].Type == JTokenType.Null ? -1 : oFlights[i][5]),
                            fLatitude = (float)(oFlights[i][6].Type == JTokenType.Null ? -1 : oFlights[i][6]),
                            fGeoAltitude = (float)(oFlights[i][7].Type == JTokenType.Null ? -1 : oFlights[i][7]),
                            bOnGround = (bool)oFlights[i][8],
                            fVelocity = (float)(oFlights[i][9].Type == JTokenType.Null ? -1 : oFlights[i][9]),
                            fHeading = (float)(oFlights[i][10].Type == JTokenType.Null ? -1 : oFlights[i][10]),
                            fVerticalRate = (float)(oFlights[i][11].Type == JTokenType.Null ? -1 : oFlights[i][11]),
                            fBaroAltitude = (float)(oFlights[i][13].Type == JTokenType.Null ? -1 : oFlights[i][13]),
                            sSquawk = (string)oFlights[i][14],
                            bSpi = (bool)oFlights[i][15],
                            nPositionSource = (int)(oFlights[i][16].Type == JTokenType.Null ? -1 : oFlights[i][16]),
                        });
                    }
                }
            }
            return lRESTFlights;
        }

        public List<String>GetAllCountries()
        {
            Crud Crud = new Crud();
            List<Country> lBaseCountries;
            lBaseCountries = Crud.GetAvailableCountries();

            string sName;
            List<String> lAllCountries = new List<String>();
            string sUrl = System.Configuration.ConfigurationManager.AppSettings["RestApiUrl2"];
            string sJson = CallRestMethod(sUrl);

            JArray json = JArray.Parse(sJson);
            foreach (JObject item in json)
            {
                string name = (string)item.GetValue("name");
                lAllCountries.Add(sName = name);
            }
            for(int i=0; i<lBaseCountries.Count; i++)
            {
                for(int j=0; j<lAllCountries.Count;j++)
                {
                    if(lBaseCountries[i].sCountryName==lAllCountries[j])
                    {
                        lAllCountries.Remove(lBaseCountries[i].sCountryName);
                    }
                }
            }
            return lAllCountries;
        }
    }
}
