using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraInwestycyjna
{
    class Portfel
    {
        public List<Rekord> Rekords { get; set; }
        public Portfel()
        {
            Rekords = new List<Rekord>();
        }
    }
}
