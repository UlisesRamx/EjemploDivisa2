using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploDivisa2.Model.Objects
{
    internal class TasasConversion
    {
        public String Disclaimer { get; set; }
        public String License { get; set; }
        public long Timestamp { get; set; }
        public String Base { get; set; }
        public Dictionary<string, double> Rates { get; set; }
    }
}
