using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Investments;

namespace GraInwestycyjna
{
    
    public partial class GraInwestycyjna : Form
    {
        public GameDbContext ctx = new GameDbContext();
        private string login = "", hasło = "", nickname="";
       
        public GraInwestycyjna()
        {
            InitializeComponent();
            var service = new ObsługaBazyDanych();

            service.PobieranieDanych();

        //    service.DodajPrzykladowoInwestycje();
        //    service.WypiszInwestycje();

            MessageBox.Show("Done!");
        }

        private void login_txt_TextChanged(object sender, EventArgs e)
        {
            login = login_txt.Text;
        }

        private void hasło_txt_TextChanged(object sender, EventArgs e)
        {
            hasło = hasło_txt.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Użytkownik tmp = new Użytkownik() {Login=login,Hasło=hasło,Nickname=nickname };
            ctx.Użytkownik.Add(tmp);
            ctx.SaveChanges();
            MessageBox.Show("Done! Account created!");

            foreach (var s in ctx.Użytkownik)
            {
                MessageBox.Show(s.ToString());
            }
        }

        private void nickname_txt_TextChanged(object sender, EventArgs e)
        {
            nickname = nickname_txt.Text;
        }

        private void zaloguj_Click(object sender, EventArgs e)
        {
            //autoryzacja użytkownika
            bool czyIstnieje=false;
            foreach(var user in ctx.Użytkownik)
            {
                if (login == user.Login && hasło == user.Hasło)
                    czyIstnieje = true;
            }
            if (czyIstnieje)
            {
                MessageBox.Show("Zalogowano!");//Login
                MainPanel panel = new MainPanel();
                panel.Show();
            }
            else
                MessageBox.Show("Błąd logowania. Nie ma takiego użytkownika!");

            //ctx.Inwestycja.Add();
            //ctx.SaveChanges();
        }
    }
}
