using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investments
{
    public class Grupa
    {
        public Grupa()
        {
            Inwestycje = new List<Inwestycja>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        // Referencja do Grupy jest virtual. Dzieki temu EF bedzie mogl wykorzystywac lazy loading
        // do automatycznego zaladowania powiazanych danych.
        public virtual List<Inwestycja> Inwestycje { get; set; }

        public override string ToString()
        {
            return string.Format("Grupa Id={0}, Name={1}, Licznosc={2}", Id, Name, Inwestycje == null ? 0 : Inwestycje.Count);
        }

    }
}
