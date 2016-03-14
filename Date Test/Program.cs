using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DateUtils;


namespace Date_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateUtils.DateUtils.ShipDate(DateTime.Now, 5));
            Console.ReadKey();
        }
    }
}
