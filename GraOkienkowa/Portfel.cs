using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investments;

namespace GraInwestycyjna
{
    class Portfel
    {
        public List<Rekord> Rekords { get; set; }
        public List<Operacja> OperacjeDodane { get; set; }
        public Portfel()
        {
            Rekords = new List<Rekord>();
            OperacjeDodane = new List<Operacja>();
        }
    }
}
