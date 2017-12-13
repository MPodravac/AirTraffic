using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RESTFlights;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            Rest Rest = new Rest();
            Rest.GetFlights();

            Console.ReadKey();
        }
    }
}
