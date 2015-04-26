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
       // DataGridViewCheckBoxColumn buy = new DataGridViewCheckBoxColumn();
        DataGridViewButtonColumn buy = new DataGridViewButtonColumn();
        DataGridViewButtonColumn sell = new DataGridViewButtonColumn();
        DataGridViewColumn  iloscCol_buy = new DataGridViewColumn(); // add a column to the grid
        DataGridViewCell cell_buy = new DataGridViewTextBoxCell();
        DataGridViewColumn iloscCol_sell = new DataGridViewColumn(); // add a column to the grid
        DataGridViewCell cell_sell = new DataGridViewTextBoxCell();
        
        
        public MainPanel()
        {
            
            InitializeComponent();
            WyswietlPortfel();
            WyswietlRynek();
            WyswietlHistorie();
            WyswietlStanKonta();
        }

       public void WyswietlRynek()
       {
            var data = ctx.Inwestycja.Select(p => p);
            try
            {
                dataGridView1.DataSource = data.ToList();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                //DataGridViewButtonColumn buy = new DataGridViewButtonColumn();
                
                iloscCol_buy.HeaderText = "Ilość";
                iloscCol_buy.Name = "ilość";
                iloscCol_buy.Visible = true;
                iloscCol_buy.Width = 40;
                iloscCol_buy.CellTemplate = cell_buy;


                buy.Name = "Buy";
                buy.Text = "buy";
                buy.UseColumnTextForButtonValue = true;
                if (dataGridView1.Columns["Ilość"] == null)
                    dataGridView1.Columns.Add(iloscCol_buy);
                if (dataGridView1.Columns["Buy"] == null)
                {
                    dataGridView1.Columns.Insert(7, buy);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wyświetlania Rynku " + ex);
            }
       }

       private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
       {
        //   MessageBox.Show(e.ColumnIndex.ToString());
           if (e.ColumnIndex == 0)
           {
              // MessageBox.Show("Row:"+(e.RowIndex + 1) + " Column: " + (e.ColumnIndex + 1) + "  Column button clicked ");
               // kup daną inwestycję
               Inwestycja inv = (Inwestycja)dataGridView1.Rows[e.RowIndex].DataBoundItem;
             //  MessageBox.Show(inv.ToString());
               //Kup inwestycję:
               var user = (from tmp in ctx.Użytkownik
                           where tmp.Hasło == userPasswd
                           select tmp).First();
               try
               { 
                   int ilość = Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.ToString());
                   if (user.StanKonta < ilość * inv.Kurs)
                       MessageBox.Show("Brak środków");
                   else
                   {
                       Operacja operation = new Operacja() { Transakcja = transakcja.kupno, StempelCzasowy = DateTime.Today, Ilość = ilość, Inwestycja = inv };
                       user.Operacja.Add(operation);
                       user.StanKonta -= ilość * inv.Kurs;
                       MessageBox.Show("zakupiłeś inwestycje " + inv.Nazwa);
                       Odśwież();
                       ctx.SaveChanges();
                   }
               }
               catch (Exception ex)
               {
                   MessageBox.Show("Uzupełnij: pole ilość");
               }

           }
       }


       

        private void WyswietlPortfel()
        {
          //  var data = ctx.Użytkownik.Select(p => p);
           
           
            Portfel Portfel = new Portfel();
            
            var user = (from tmp in ctx.Użytkownik
                        where tmp.Hasło == userPasswd
                        select tmp).First();
           // MessageBox.Show("Ilość operacji użytkownika: "+user.Operacja.Count());

            foreach (var operation in user.Operacja)
            {
                bool czyJestWPortfelu = false;
              
                foreach (var rekord in Portfel.Rekords)
                {
                            if (rekord.Nazwa == operation.Inwestycja.Nazwa)
                            {
                                czyJestWPortfelu = true;
                                rekord.Liczba += ((operation.Transakcja == transakcja.kupno) ? 1 : (-1)) * operation.Ilość;
                                rekord.ŚredniKursKupna += ((operation.Transakcja == transakcja.kupno) ? operation.Inwestycja.Kurs / rekord.Liczba : 0);
                                rekord.OkresInwestycji = (rekord.OkresInwestycji > Convert.ToInt32((DateTime.Today - operation.StempelCzasowy).TotalDays)) ? rekord.OkresInwestycji : Convert.ToInt32((DateTime.Today - operation.StempelCzasowy).TotalDays);

                                var data = (from inv in ctx.Inwestycja
                                            where inv.Data == DateTime.Today
                                            select inv).First();

                                rekord.Zysk = (data.Kurs - operation.Inwestycja.Kurs) * rekord.Liczba;
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
            
            dataGridView2.DataSource = Portfel.Rekords.ToList();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            iloscCol_sell.HeaderText = "Ilość";
            iloscCol_sell.Name = "ilość";
            iloscCol_sell.Visible = true;
            iloscCol_sell.Width = 40;
            iloscCol_sell.CellTemplate = cell_buy;


            sell.Name = "Sell";
            sell.Text = "sell";
            sell.UseColumnTextForButtonValue = true;
            if (dataGridView2.Columns["Ilość"] == null)
                dataGridView2.Columns.Add(iloscCol_sell);
            if (dataGridView2.Columns["Sell"] == null)
            {
                dataGridView2.Columns.Insert(9, sell);
            }
        }

        private void WyswietlHistorie()
        {
           
            using (var form = new GraInwestycyjna())
            {

                try
                {
                    var data = (from user in ctx.Użytkownik
                                where user.Hasło == userPasswd
                                select user).First();

                    dataGridView3.DataSource = data.Operacja.ToList();
                    dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView3.Columns[0].Visible = false;
                    dataGridView3.Columns[4].Visible = false;
                    dataGridView3.Columns[5].Visible = false;
                    dataGridView3.Columns[6].Visible = false;
                    dataGridView3.Columns[7].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd wyświetlania operacji w portfelu: " + ex);
                }
            }
        }


        private void WyswietlStanKonta()
        {
             var user = (from tmp in ctx.Użytkownik
                        where tmp.Hasło == userPasswd
                        select tmp).First();
             stan_konta.Text = user.StanKonta.ToString();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Odśwież();
        }

        private void Odśwież()
        {
            WyswietlPortfel();
            //      WyswietlRynek(); 
            WyswietlHistorie();
            WyswietlStanKonta();

            /* Wyświetl rynek odkomentowane bo przy odświeżaniu znikają kolumny - o co chodzi?*/
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(e.ColumnIndex.ToString());
            
            if (e.ColumnIndex == 1)
            {
                // MessageBox.Show("Row:"+(e.RowIndex + 1) + " Column: " + (e.ColumnIndex + 1) + "  Column button clicked ");
                // kup daną inwestycję
                //Inwestycja inv = (Inwestycja)dataGridView2.Rows[e.RowIndex].DataBoundItem;
                //  MessageBox.Show(inv.ToString());
                //Kup inwestycję:
                string nazwa = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.ToString();
                var inv = (from tmp1 in ctx.Inwestycja
                            where tmp1.Data == DateTime.Today
                            where tmp1.Nazwa == nazwa
                            select tmp1).First();

                var user = (from tmp in ctx.Użytkownik
                            where tmp.Hasło == userPasswd
                            select tmp).First();
                //MessageBox.Show(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex-1].Value.ToString());
                try
                { 
                int ilość = Int32.Parse(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex-1].Value.ToString());
                //if (user.Operacja < ilość * inv.Kurs) // skąd wziąć kurs? sprawdzić czy ma tyle inwestycji
                  //  MessageBox.Show("Brak środków");
                
                    Operacja operation = new Operacja() { Transakcja = transakcja.sprzedaż, StempelCzasowy = DateTime.Today, Ilość = ilość, Inwestycja = inv };
                    user.Operacja.Add(operation);
                    user.StanKonta += ilość * inv.Kurs;
                    MessageBox.Show("sprzedałeś inwestycje " + inv.Nazwa);
                    Odśwież();
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Uzupełnij: pole ilość");
                }
                
            
            }
        }
    }
}
