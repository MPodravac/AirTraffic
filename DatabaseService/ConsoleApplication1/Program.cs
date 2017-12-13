using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseService;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Crud Crud = new Crud();
            List<Country> lCountries = Crud.GetAllCountries();
            Console.ReadKey();
        }
    }
}
