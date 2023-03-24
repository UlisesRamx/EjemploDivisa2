using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploDivisa2.Model.Api
{
    internal class RespuestaMoneda
    {
        public bool Error { get; set; }
        public string Mensaje { get; set; }
        public Dictionary<string, string> Monedas { get; set; }
    }
}
