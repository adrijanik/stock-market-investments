using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investments
{
    /// <summary>
    /// Specjalna klasa, któa ma pomagać w inicjalizowaniu bazy danych.
    /// Może dziedziczyć po kilku klasach - jeżeli dziedziczy po DropCreateDatabaseIfModelChanges, 
    /// to baza zostanie przebudowana przy każdej zmianie modelu.
    /// Jezeli dziedziczy po DropCreateDatabaseAlways to przy każdym uruchomieniu baza danych tworzona jest od nowa.
    /// </summary>
    public class GameDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<GameDbContext>
    {

        /// <summary>
        /// Specjalna metoda, która jest wywoływana raz po przebudowaniu bazy danych.
        /// Założenie jest, że baza jest pusta, więc trzeba ją wypełnić początkowymi danymi.
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(GameDbContext context)
        {
            Grupa grupa;

            grupa = new Grupa() { Name = "Akcje" };
            grupa.Inwestycje.Add(new Inwestycja() { Nazwa = "ENEA", Kurs = 16.43, Data = new DateTime(2000, 1, 1), Przelicznik = 1 });
            grupa.Inwestycje.Add(new Inwestycja() { Nazwa = "ENEA", Kurs = 12.43, Data = new DateTime(2015, 4, 26), Przelicznik = 1 });
            grupa.Inwestycje.Add(new Inwestycja() { Nazwa = "PZU", Kurs = 200.80, Data = new DateTime(2010, 2, 2), Przelicznik = 1 });
            grupa.Inwestycje.Add(new Inwestycja() { Nazwa = "PZU", Kurs = 499.80, Data = new DateTime(2015, 4, 26), Przelicznik = 1 });
            grupa.Inwestycje.Add(new Inwestycja() { Nazwa = "PGE", Kurs = 35.00, Data = new DateTime(2015, 4, 26), Przelicznik = 1 });
            context.Grupa.Add(grupa);

            grupa = new Grupa() { Name = "Obligacje" };
            grupa.Inwestycje.Add(new Inwestycja() { Nazwa = "BST0319", Kurs = 140.30, Data = new DateTime(2000, 1, 1), Przelicznik = 1 });
            grupa.Inwestycje.Add(new Inwestycja() { Nazwa = "BST0319", Kurs = 100.30, Data = new DateTime(2015, 4, 26), Przelicznik = 1 });
            grupa.Inwestycje.Add(new Inwestycja() { Nazwa = "DS1020", Kurs = 117.52, Data = new DateTime(2000, 2, 2), Przelicznik = 1 });
            grupa.Inwestycje.Add(new Inwestycja() { Nazwa = "DS1020", Kurs = 150.52, Data = new DateTime(2015, 4, 26), Przelicznik = 1 });
            context.Grupa.Add(grupa);

            grupa = new Grupa { Name = "Waluty" };
            grupa.Inwestycje.Add(new Inwestycja { Nazwa = "EURASY", Kurs = 100, Przelicznik = 1, Data = new DateTime(2000,1,1) });
            context.Grupa.Add(grupa);          

            Inwestycja tmp = new Inwestycja() { Nazwa = "PGE", Kurs = 20.54, Data = new DateTime(2000, 2, 2), Przelicznik = 1, Grupa = grupa };
            Inwestycja tmp1 = new Inwestycja() { Nazwa = "PGE", Kurs = 25.54, Data = new DateTime(2001, 2, 2), Przelicznik = 1, Grupa = grupa };
            
            Użytkownik user = new Użytkownik() {Id = 0, Nickname ="Michał", Login="Nowak",StanKonta=1000000, Hasło="123abc" };
            context.Użytkownik.Add(user);
            Operacja operacja = new Operacja() {Id = 0, Ilość = 5, Transakcja = transakcja.kupno, Inwestycja = tmp, StempelCzasowy = new DateTime(2015,4,4), Użytkownik=user };
            context.Operacja.Add(operacja);
            operacja = new Operacja() { Id = 0, Ilość = 15, Transakcja = transakcja.kupno, Inwestycja = tmp1, StempelCzasowy = new DateTime(2015, 4, 4), Użytkownik = user };
            context.Operacja.Add(operacja);
            


            context.SaveChanges();
            base.Seed(context);
        }
    }
}
