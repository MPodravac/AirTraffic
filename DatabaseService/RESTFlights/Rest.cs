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

        public List<Flight> GetFlights()
        {
            List<Flight> lRESTFlights = new List<Flight>();
            string sUrl = System.Configuration.ConfigurationManager.AppSettings["RestApiUrl"];
            string sJson = CallRestMethod(sUrl);

            JArray json = JArray.Parse(sJson);
            foreach (JObject item in json)
            {
                string icao = (string)item.GetValue("icao24");
                string callsign = (string)item.GetValue("callsign");
                string country = (string)item.GetValue("origin_country");
                int timeposition = (int)item.GetValue("time_position");
                int lastcontact = (int)item.GetValue("last_contact");
                float longitude = (float)item.GetValue("longitude");
                float latitude = (float)item.GetValue("latitude");
                float geoaltitude = (float)item.GetValue("geo_altitude");
                bool onground = (bool)item.GetValue("on_ground");
                float velocity = (float)item.GetValue("velocity");
                float heading = (float)item.GetValue("heading");
                float verticalrate = (float)item.GetValue("vertical_rate");
                int sensors = (int)item.GetValue("sensors");
                float baroaltitude = (float)item.GetValue("baro_altitude");
                string squawk = (string)item.GetValue("squawk");
                bool spi = (bool)item.GetValue("api");
                int positionsource = (int)item.GetValue("position_source");

                lRESTFlights.Add(new Flight
                {
                    sIcao24 = icao,
                    sCallSign = callsign,
                    sCountry = country,
                    nTimePosition = timeposition,
                    nLastContact = lastcontact,
                    fLongitude = longitude,
                    fLatitude = latitude,
                    fGeoAltitude = geoaltitude,
                    bOnGround = onground,
                    fVelocity = velocity,
                    fHeading = heading,
                    fVerticalRate = verticalrate,
                    nSensors = sensors,
                    fBaroAltitude = baroaltitude,
                    sSquawk = squawk,
                    bSpi = spi,
                    nPositionSource =positionsource,
                });
            }
            return lRESTFlights;
        }
        public List<String>GetAllCountries()
        {
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
            return lAllCountries;
        }
    }
}
