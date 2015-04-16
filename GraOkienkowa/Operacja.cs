using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investments
{
    public enum transakcja
    {
        kupno,
        sprzedaż,
    };

    public class Operacja
    {
        public int Id { get; set; }
        public transakcja Transakcja { get; set; }
        public int Ilość { get; set; }
        public DateTime StempelCzasowy { get; set; }
        public int InwestycjaId { get; set; }
        public int UżytkownikId { get; set; }

        public virtual Inwestycja Inwestycja { get; set; }
        public virtual Użytkownik Użytkownik { get; set; }

        public override string ToString()
        {
            return string.Format("Op Id={0}, Transakcja={1}, Ilość={2}, Inwestycja={3}", Id, transakcja.kupno.ToString(), Ilość, Inwestycja.Nazwa);
        }
    }
}
