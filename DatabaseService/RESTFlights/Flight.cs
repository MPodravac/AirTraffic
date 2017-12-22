using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTFlights
{
    public class Flight
    {
        public string sIcao24 { get; set; }
        public string sCallSign { get; set; }
        public string sCountry { get; set; }
        public int nTimePosition { get; set; }
        public int nLastContact { get; set; }
        public float fLongitude { get; set; }
        public float fLatitude { get; set; }
        public float fGeoAltitude { get; set; }
        public bool bOnGround { get; set; }
        public float fVelocity { get; set; }
        public float fHeading { get; set; }
        public float fVerticalRate { get; set; }
        public float fBaroAltitude { get; set; }
        public string sSquawk { get; set; }
        public bool bSpi { get; set; }
        public int nPositionSource { get; set; }
    }
}
