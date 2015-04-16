using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investments
{

    public class Inwestycja
    {
        // EF w ramach konwencji zaklada, ze klucz glowny ma nazwe Id.
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public double Kurs { get; set; }
        public DateTime Data { get; set; }
        public int Przelicznik { get; set; }

        // Zmienna GrupaId jest nullable, to znaczy, że moze byc ustawiona jako null.
        // Dzieki temu nie kazdy student musi byc przypisany do grupy.
        // EF w ramach konwencji zaklada, ze klucz obcy do tabeli zewnetrznej to nazwa tabli zakonczona Id.
        public int GrupaId { get; set; }

        // Jezeli chcemy, aby klucz obcy mial inna nazwe niz konwencja to mozemy to zrobic przez atrybut ForeighKey.
        [ForeignKey("GrupaId")]
        // Referencja do zmiennej Grupa jest virtual. Dzieki temu EF bedzie mogl wykorzystywac lazy loading
        // do automatycznego zaladowania powiazanych danych.
        public virtual Grupa Grupa { get; set; }

        public override string ToString()
        {
            return string.Format("St Id={0}, Name={1}, Kurs={2}, Data={3}, Przelicznik={4}, Grupa={5}", Id, Nazwa, Kurs, Data.ToString("d"), Przelicznik, Grupa.Name);
        }
    }

    /// <summary>
    /// Takie oznaczenie mówi - do dziedziczenia używaj osobnych tabel. Podejscie TPT - Table Per Type.
    /// Domyslnie EF przyjmuje podejscie TPH - Table Per Hierarchy.
    /// Szczegóły - patrz http://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-1-table-per-hierarchy-tph 
    /// </summary>
   
}
