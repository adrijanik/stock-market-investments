using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Investments
{
    public class ObsługaBazyDanych
    {

        public void PobieranieDanychWaluty()
        {
            PobieranieDanych tmp = new PobieranieDanych();
            tmp.PobierzKursyWalut();
           
        }
        public void PobieranieDanychAkcje()
        {
            PobieranieDanych tmp = new PobieranieDanych();
            tmp.PobierzNotowaniaGiełdowe();
        }


        /// <summary>
        /// Prosta metoda, ktora wypisuje wszystkie grupy i studentow z bazy danych.
        /// Normalnie takie metody powinny się znaleźć w odpowiednich klasach do obsługi danych
        /// </summary>
        ///
        public void WypiszInwestycje()
        {
            // Do odwoływania się do bazy danych używamy obiektu kontekstu
            using (var ctx = new GameDbContext())
            {
                // EF domyślnie wykorzystuje lazy loading - jeżeli coś nie jest potrzebne, to nie jest ładowane
                // Aby EF automatycznie moglo zaladowac relacje, musza one byc zdefiniowane jako virtual
                // W przeciwnym wypadku musielibysmy jawnie zadeklarowac powiazanie przez dolaczenie do zapytania tablicy Students, np.: 
                // var grupy = ctx.Grupa.Include("Students");

                foreach (var g in ctx.Grupa)
                {
                    Console.WriteLine("----------------");
                    Console.WriteLine(g);
                    foreach (var s in g.Inwestycje)
                    {
                        Console.WriteLine(s);
                    }
                }
                Console.WriteLine("----------------");
                foreach (var s in ctx.Operacja)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine("----------------");
                foreach (var h in ctx.Użytkownik)
                {
                    Console.WriteLine(h);
                }
            }
        }


        public void DodajPrzykladowoInwestycje()
        {
            using (var ctx = new GameDbContext())
            {
                // Tworzymy grupę w pamięci
                Grupa grupa = new Grupa() { Name = "Sosny" };

                Inwestycja inwestycja;

                // Dodajemy studenta do grupy
                inwestycja = new Inwestycja() { Nazwa = "Wysokie", Data = new DateTime(1990, 10, 10), Kurs = 5000, Przelicznik = 1 };
                grupa.Inwestycje.Add(inwestycja);


                // Grupę dodajemy do kolekcji reprezentującej dane w bazie danych
                ctx.Grupa.Add(grupa);



                // Zapisujemy zmienne przechowywane w kontekście
                ctx.SaveChanges();
            }
        }

    }
}
