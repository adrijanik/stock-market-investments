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
using System.Configuration;

namespace GraInwestycyjna
{
    
    public partial class GraInwestycyjna : Form
    {
        
        public GameDbContext ctx = new GameDbContext();
        public string login = "", hasło = "", nickname="";
        public GraInwestycyjna()
        {
            InitializeComponent();
            var service = new ObsługaBazyDanych();
            hasło_txt.PasswordChar='*';
            hasło_txt.MaxLength = 8;

         /*   foreach (var op in ctx.Operacja)
                ctx.Operacja.Remove(op);

            foreach (var user in ctx.Użytkownik)
            {
                if (user.Login == "adri")
                    ctx.Użytkownik.Remove(user);
            }
            ctx.SaveChanges();
            
*/
         //   StwórzFirmy();
         
        
       //     UsunNiepotrzebne_Sprzatacz();
       //     StwórzWaluty();
        
            
          //  service.PobieranieDanychAkcje();
       //     service.PobieranieDanychWaluty(); // uwaga -> trwa około 30 minut
   
         //   service.DodajPrzykladowoInwestycje();
         //   service.WypiszInwestycje();

         //   MessageBox.Show("Done!");
        }

    
        void UsunNiepotrzebne_Sprzatacz()
        {
            foreach (var inv in ctx.Inwestycja)
            {
                if (inv.Nazwa == "Przykład")
                    ctx.Inwestycja.Remove(inv);
            }
            foreach(var user in ctx.Użytkownik)
            {
                if (user.Login == "Nowak" )
                    ctx.Użytkownik.Remove(user);
            }
            foreach (var fm in ctx.Firma)
            {
                if (fm.Name == "Przykład")
                    ctx.Firma.Remove(fm);
            }

            ctx.SaveChanges();
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
        }

        //autoryzacja użytkownika           
        public bool isValidCredentials()
        {
            bool czyIstnieje = false;
            foreach (var user in ctx.Użytkownik)
            {
                if (login == user.Login && hasło == user.Hasło)
                {
                    czyIstnieje = true;
                    UpdateConfig("UserPasswd",hasło);
    //                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
      //              config.AppSettings.Settings.Add("UserPasswd", hasło);
        //            config.Save(ConfigurationSaveMode.Modified);

        /*            Configuration configuration =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    configuration.AppSettings.Settings["Login"].Value= login;
                    configuration.Save(ConfigurationSaveMode.Full, true);
                    ConfigurationManager.RefreshSection("appSettings");
                
         */ }

            }
            if (czyIstnieje)
            {
                
                
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
        private void UpdateConfig(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configFile.AppSettings.Settings[key].Value = value;

            //configFile.Save();
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }


        
        void StwórzFirmy()
        {
            string line;
            System.IO.StreamReader file =
            new System.IO.StreamReader(@"C:\Users\Adri\Desktop\Bats_Stocks.txt");
            while ((line = file.ReadLine()) != null)
            {
                string nazwa = line;
                Firma fm = new Firma() { Name = nazwa };
                ctx.Firma.Add(fm);
            }

        }
        void StwórzWaluty()
        {
            string line;
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"C:\Users\Adri\Desktop\waluty.txt");
         //   Inwestycja init = new Inwestycja { Nazwa = "init", Data = DateTime.Now, Kurs = 0 };
            while ((line = file.ReadLine()) != null)
            {
                string nazwa = line;
                if (!ctx.Firma.Any(p => p.Name == nazwa))
                {
                    Firma fm = new Firma() { Name = nazwa }; //, AktualnaInwestycja=init };
                    ctx.Firma.Add(fm);
                }
            }
            ctx.SaveChanges();
        }

    }
}
