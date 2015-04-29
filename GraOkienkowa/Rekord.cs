using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Investments;
using System.Windows.Forms;

namespace GraInwestycyjna
{
    
        class Rekord
        {


            public string Nazwa { get; set; }
            public double KursAktualny { get; set; }
            public int Przelicznik { get; set; }
            public string Typ { get; set; } //nazwa grupy
            public int Liczba { get; set; }
            public double ŚredniKursKupna { get; set; }
            public int OkresInwestycji { get; set; } // dni
            public double Zysk { get; set; }

            public Rekord() { }
            public Rekord(Operacja Operacja, DateTime CzasAktualny)
            {

                if (CzasAktualny.IsHoliday())
                {
                    CzasAktualny = CzasAktualny.GetLastWorkingDay();
                    MessageBox.Show("CzasAktualny "+ CzasAktualny);
                }
                try
                {
                    Nazwa = Operacja.Inwestycja.Nazwa;
                    using (var ctx = new GameDbContext())
                    {
                        try
                        {
                            var aktualny = (from tmp in ctx.Inwestycja
                                            where tmp.Nazwa == Nazwa
                                            where tmp.Data.Day == CzasAktualny.Day//DateTime.Today
                                            select tmp).First();
                            KursAktualny = aktualny.Kurs;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Brak danych! Uzupełnij dane w bazie");
                        }


                        //KursAktualny = 10000000;
                    }

                    Przelicznik = Operacja.Inwestycja.Przelicznik;
                    using (var ctx = new GameDbContext())
                    {
                        var grupa = (from tmp in ctx.Grupa
                                     where tmp.Id == Operacja.Inwestycja.GrupaId
                                     select tmp).First();
                        Typ = grupa.Name;

                    }
                    Liczba = ((Operacja.Transakcja == transakcja.kupno) ? 1 : (-1)) * Operacja.Ilość;
                    ŚredniKursKupna = Operacja.Inwestycja.Kurs;
                    OkresInwestycji = Convert.ToInt32((CzasAktualny - Operacja.StempelCzasowy).TotalDays);

                    using (var ctx = new GameDbContext())
                    {
                        var data = (from inv in ctx.Inwestycja
                                    where inv.Data.Day == CzasAktualny.Day
                                    select inv).First();
                        Zysk = (data.Kurs - Operacja.Inwestycja.Kurs) * Liczba;
                    }
                }
                catch(Exception ex)
                {
                    throw;  
                }
            }
        }
    }

