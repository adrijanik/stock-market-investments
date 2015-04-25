using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Investments;

namespace GraInwestycyjna
{
    class Rekord
    {

     
        public string Nazwa { get; set; }
        public double KursAktualny { get; set; }
        public int Przelicznik { get; set; }
        public string Typ { get; set; } //nazwa grupy
        public int IlośćPosiadanych {get; set;}
        public double ŚredniKursKupna { get; set; }
        public int OkresInwestycji { get; set; } // dni
        public double Zysk { get; set; }

        public Rekord() { }
        public Rekord(Operacja Operacja)
        {
            Nazwa = Operacja.Inwestycja.Nazwa;
            KursAktualny = Operacja.Inwestycja.Kurs;
            Przelicznik = Operacja.Inwestycja.Przelicznik;
           using (var ctx = new GameDbContext())
           {
               var grupa = (from tmp in ctx.Grupa
                            where tmp.Id == Operacja.Inwestycja.GrupaId
                            select tmp).First();
               Typ = grupa.Name;

           }
            IlośćPosiadanych = ((Operacja.Transakcja == transakcja.kupno) ? 1 : (-1)) * Operacja.Ilość;
            ŚredniKursKupna = Operacja.Inwestycja.Kurs;
            OkresInwestycji = Convert.ToInt32((DateTime.Today - Operacja.StempelCzasowy).TotalDays);

            using(var ctx = new GameDbContext())
            {
                var data = (from inv in ctx.Inwestycja
                            where inv.Data == DateTime.Today
                            select inv).First();
                Zysk = (data.Kurs-Operacja.Inwestycja.Kurs) * IlośćPosiadanych;
            }

            //TODO: Określić timing gry i poprawne odniesienia do dat!
        }
    }
}
