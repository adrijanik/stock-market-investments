using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investments
{
    public class Firma
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Inwestycja AktualnaInwestycja;// { get; set; }  


            public Firma()
            {
                Archiwum = new List<Inwestycja>();
                AktualnaInwestycja = new Inwestycja();

            }

            // Referencja do Grupy jest virtual. Dzieki temu EF bedzie mogl wykorzystywac lazy loading
            // do automatycznego zaladowania powiazanych danych.
            public virtual List<Inwestycja> Archiwum { get; set; }

            public override string ToString()
            {
                return string.Format("Grupa Id={0}, Name={1}, Licznosc={2}", Id, Name, Archiwum == null ? 0 : Archiwum.Count());
            }

        }
}    



