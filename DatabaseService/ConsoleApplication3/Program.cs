using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RESTFlights;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            Rest Rest = new Rest();
            List<Flight> letovi = Rest.GetFlights();
            for (int i = 0; i < letovi.Count; i++)
            {
                Console.WriteLine(letovi[i].sCallSign + "/n " + letovi[i].sCountry);
                Console.WriteLine("-----------------------------------");
            }
            Console.ReadLine();
        }
    }
}
