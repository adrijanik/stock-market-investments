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
        public string login = "", hasło = "adri", nickname="",haslo_loged_user="";
        public GraInwestycyjna()
        {
            InitializeComponent();
            var service = new ObsługaBazyDanych();
            hasło_txt.PasswordChar='*';
            hasło_txt.MaxLength = 8;
            if (haslo_loged_user!= "")
                service.DodajOperacjeUżytkownikowi(haslo_loged_user);

         //   service.PobieranieDanychAkcje();
         //   service.PobieranieDanychWaluty();

         //   service.DodajPrzykladowoInwestycje();
         //   service.WypiszInwestycje();

         //   MessageBox.Show("Done!");
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
            Użytkownik tmp = new Użytkownik() {Login=login,Hasło=hasło,Nickname=nickname,StanKonta=100000 };
            ctx.Użytkownik.Add(tmp);
            ctx.SaveChanges();
            MessageBox.Show("Done! Account created!");

        }

        private void nickname_txt_TextChanged(object sender, EventArgs e)
        {
            nickname = nickname_txt.Text;
        }

        private void zaloguj_Click(object sender, EventArgs e)
        {
           
            if (isValidCredentials()) 
            { 
                DialogResult = DialogResult.OK;
               // main_panel.ShowDialog();
                this.Close(); 
                
                
            } 
            else 
            { 
                //messageLabel.Visible = true; 
                MessageBox.Show("Failed to login");
                login_txt.Focus();
                DialogResult = DialogResult.None;
            }

           


            //autoryzacja użytkownika
            
            
            

            //ctx.Inwestycja.Add();
            //ctx.SaveChanges();
        }
        public bool isValidCredentials()
        {
            bool czyIstnieje = false;
            foreach (var user in ctx.Użytkownik)
            {
                if (login == user.Login && hasło == user.Hasło)
                    czyIstnieje = true;
            }
            if (czyIstnieje)
            {
                
                haslo_loged_user = hasło;
             //   MessageBox.Show("Zalogowano! " + haslo_loged_user);//Login
            //    MainPanel panel = new MainPanel();
            //    panel.Show();
                return true;
            }
            else
            {
               // MessageBox.Show("Błąd logowania. Nie ma takiego użytkownika!");
                return false;
            }
        }
    }
}
