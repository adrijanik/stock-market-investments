using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investments
{
    public class Użytkownik
    {
        public Użytkownik()
        {
            this.Operacja = new HashSet<Operacja>();
        }

        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Login { get; set; }
        public string Hasło { get; set; }

        public virtual ICollection<Operacja> Operacja { get; set; }

        public override string ToString()
        {
            return string.Format("User Id={0}, Nickname={1}, Login={2}, Hasło={3}, Ilość operacji={4}", Id, Nickname, Login, Hasło, Operacja.Count());
        }
    }
}
