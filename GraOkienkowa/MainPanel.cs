using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Investments;
using System.Configuration;

namespace GraInwestycyjna
{
    public partial class MainPanel : Form
    {
        string userPasswd = ConfigurationManager.AppSettings["UserPasswd"];
        public GameDbContext ctx = new GameDbContext();
        public MainPanel()
        {
            InitializeComponent();  
        }

        private void Rynek_Click(object sender, EventArgs e)
        {
            var data = ctx.Inwestycja.Select(p => p);
            try
            { 
            dataGridView1.DataSource = data.ToList();
                }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wyświetlania Rynku "+ex);
            }

        }

        private void portfel_Click(object sender, EventArgs e)
        {
          //  var data = ctx.Użytkownik.Select(p => p);
           
           
            Portfel Portfel = new Portfel();
            
            var user = (from tmp in ctx.Użytkownik
                        where tmp.Hasło == userPasswd
                        select tmp).First();
            MessageBox.Show("Ilość operacji użytkownika: "+user.Operacja.Count());
        
            foreach (var operation in user.Operacja)
            {
                bool czyJestWPortfelu = false;
              
                foreach (var rekord in Portfel.Rekords)
                {
                            if (rekord.Nazwa == operation.Inwestycja.Nazwa)
                            {
                                czyJestWPortfelu = true;
                                rekord.IlośćPosiadanych += ((operation.Transakcja == transakcja.kupno) ? 1 : (-1)) * operation.Ilość;
                                rekord.ŚredniKursKupna += ((operation.Transakcja == transakcja.kupno) ? operation.Inwestycja.Kurs / rekord.IlośćPosiadanych : 0);
                                rekord.OkresInwestycji = (rekord.OkresInwestycji > Convert.ToInt32((DateTime.Today - operation.StempelCzasowy).TotalDays)) ? rekord.OkresInwestycji : Convert.ToInt32((DateTime.Today - operation.StempelCzasowy).TotalDays);

                                var data = (from inv in ctx.Inwestycja
                                            where inv.Data == DateTime.Today
                                            select inv).First();

                                rekord.Zysk = (data.Kurs - operation.Inwestycja.Kurs) * rekord.IlośćPosiadanych;
                            }
                        }
                    if (!czyJestWPortfelu)
                    {
                        Rekord rekord = new Rekord(operation);
                        Portfel.Rekords.Add(rekord);
                    }

                    /*sprawdź czy dana inwestycja jest już w portfelu
                     * ->  jeśli tak zaktualizuj informacje o:
                     *   ilości posiadanej inwestycji (ilość +=(znak_transakcji)ilość_operacji)
                     *   średnim kursie kupna (kurs+=(inwestycja(stempel czasowy inwestycji).kurs))/(ilość_inwestycji)
                     *   okresie inwestycji start=((start>inwestycja(stempel czasowy inwestycji))?inwestycja(stempel czasowy inwestycji):start;)
                     *   zysku (zysk+=inwestycja(inwestycja_aktualny_kurs -inwestycja(stempel czasowy inwestycji).kurs) * ilość_w_operacji)
                     *  
                      dodaj informacje do tabeli portfela
                     */
                    // ;
                }
            
            dataGridView1.DataSource = Portfel.Rekords.ToList();
        }

        private void historia_Click(object sender, EventArgs e)
        {
           
            using (var form = new GraInwestycyjna())
            {

                try
                {
                    var data = (from user in ctx.Użytkownik
                                where user.Hasło == userPasswd
                                select user).First();

                    dataGridView1.DataSource = data.Operacja.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd wyświetlania operacji w portfelu: " + ex);
                }
            }
        }

       
    }
}
