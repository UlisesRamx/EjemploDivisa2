using EjemploDivisa2.Model.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploDivisa2.Model.Api
{
    internal class RespuestaTasas
    {
        public bool Error { get; set; }
        public String Mensaje { get; set; }
        public TasasConversion Tasas { get; set; }
    }
}
