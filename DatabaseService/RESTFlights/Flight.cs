using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTFlights
{
    public class Flight
    {
        public string sIcao24 { get; set; } //24-bitna adresa transpondera-hex (primanje signala)
        public string sCallSign { get; set; } //pozivni znak aviona
        public string sCountry { get; set; }
        public int nTimePosition { get; set; } //vremenska oznaka od zadnje poznate pozicije
        public int nLastContact { get; set; } //vremenska oznaka od zadnjeg primljenog signala s transpondera
        public float fLongitude { get; set; }
        public float fLatitude { get; set; }
        public float fGeoAltitude { get; set; } //visina u stupnjevima
        public bool bOnGround { get; set; } 
        public float fVelocity { get; set; } //brzina u m/s
        public float fHeading { get; set; } //pravac u stupnjevima (sjever=0°), smjer kazaljke na satu
        public float fVerticalRate { get; set; } //pozitivna vrij. avion se uspinje, negativna avion se spušta u m/s
        public float fBaroAltitude { get; set; } //visina u metrima
        public string sSquawk { get; set; } //kod transpondera
        public bool bSpi { get; set; } //special purpose indicator
        public int nPositionSource { get; set; }
    }
}
