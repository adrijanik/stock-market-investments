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
//using System.Timers;

namespace GraInwestycyjna
{
    public partial class MainPanel : Form
    {
        
        string userPasswd = ConfigurationManager.AppSettings["UserPasswd"];
        Portfel Portfel = new Portfel();
        public GameDbContext ctx = new GameDbContext();
       // DataGridViewCheckBoxColumn buy = new DataGridViewCheckBoxColumn();
        DataGridViewButtonColumn buy = new DataGridViewButtonColumn();
        DataGridViewButtonColumn sell = new DataGridViewButtonColumn();
        DataGridViewColumn  iloscCol_buy = new DataGridViewColumn(); // add a column to the grid
        DataGridViewCell cell_buy = new DataGridViewTextBoxCell();
        DataGridViewColumn iloscCol_sell = new DataGridViewColumn(); // add a column to the grid
        DataGridViewCell cell_sell = new DataGridViewTextBoxCell();
        DateTime StartTime;

        public MainPanel()
        {



            StartTime = DateTime.Now;
            var timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval =10* 1000; //1 seconds *10
            timer.Start();

           
            InitializeComponent();
            czas_aktualny.Text =Convert.ToString(new DateTime(2014, 1, 2));
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
         //  MessageBox.Show(e.ColumnIndex.ToString());
           if (e.ColumnIndex == 0)
           {
              
               
                DateTime data_aktualna = DateTime.Parse(czas_aktualny.Text);
                if (!data_aktualna.IsHoliday())
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

                            Operacja operation = new Operacja() { Transakcja = transakcja.kupno, StempelCzasowy = Convert.ToDateTime(czas_aktualny.Text), Ilość = ilość, Inwestycja = inv };
                            user.Operacja.Add(operation);
                            user.StanKonta -= ilość * inv.Kurs * inv.Przelicznik;
                            MessageBox.Show("zakupiłeś inwestycje " + inv.Nazwa);
                            Odśwież();
                            ctx.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Uzupełnij: pole ilość" + ex);
                    }
                }
           }
       }


       

        private void WyswietlPortfel()
        {
          //  var data = ctx.Użytkownik.Select(p => p);
           
           
            
            
            var user = (from tmp in ctx.Użytkownik
                        where tmp.Hasło == userPasswd
                        select tmp).First();
           // MessageBox.Show("Ilość operacji użytkownika: "+user.Operacja.Count());

            foreach (var operation in user.Operacja)
            {
                bool czyJestWPortfelu = false;
                DateTime data_aktualna = Convert.ToDateTime(czas_aktualny.Text);
                foreach (var rekord in Portfel.Rekords)
                {
                   
                            if (rekord.Nazwa == operation.Inwestycja.Nazwa  && !Portfel.OperacjeDodane.Contains(operation))
                            {
                                czyJestWPortfelu = true;
                                rekord.Liczba += ((operation.Transakcja == transakcja.kupno) ? 1 : (-1)) * operation.Ilość;
                                rekord.ŚredniKursKupna += ((operation.Transakcja == transakcja.kupno) ? operation.Inwestycja.Kurs / rekord.Liczba : 0);
                                rekord.OkresInwestycji = (rekord.OkresInwestycji > Convert.ToInt32((Convert.ToDateTime(czas_aktualny.Text) - operation.StempelCzasowy).TotalDays)) ? rekord.OkresInwestycji : Convert.ToInt32((Convert.ToDateTime(czas_aktualny.Text) - operation.StempelCzasowy).TotalDays);
                                Inwestycja data;
                                try
                                {
                                    data = (from inv in ctx.Inwestycja
                                                where inv.Data.Day == data_aktualna.Day
                                                select inv).First();
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }
                                rekord.Zysk = (data.Kurs - operation.Inwestycja.Kurs) * rekord.Liczba;
                                Portfel.OperacjeDodane.Add(operation);
                            }
                 }
                if (!czyJestWPortfelu && !Portfel.OperacjeDodane.Contains(operation))
                    {
                        Rekord rekord = new Rekord(operation, data_aktualna);
                        Portfel.Rekords.Add(rekord);
                        Portfel.OperacjeDodane.Add(operation);
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

            dataGridView2.Columns[5].DefaultCellStyle.Format = "n2";
            dataGridView2.Columns[7].DefaultCellStyle.Format = "n2";
        }

        private void WyswietlHistorie()
        {
                try
                {
                    var data = (from user in ctx.Użytkownik
                                where user.Hasło == userPasswd
                                select user).First();

                    dataGridView3.DataSource = data.Operacja.ToList();
                    dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                  //  dataGridView3.Columns[3].DefaultCellStyle.Format = "n2";
                   // dataGridView3.Columns[2].DefaultCellStyle.Format = "n2";
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
            try
            {
                WyswietlPortfel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Brak odpowiednich danych w bazie, nie można skonstruować portfela " + ex);
            }
       //     WyswietlRynek(); 
            WyswietlHistorie();
            WyswietlStanKonta();
    //        if (TimerExample.DateList.Count()!=0)
      //          czas_aktualny.Text = TimerExample.DateList[TimerExample.DateList.Count()-1].ToString();

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
                DateTime data_aktualna = DateTime.Parse(czas_aktualny.Text);
                if (!data_aktualna.IsHoliday())
                {
                    string nazwa = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.ToString();
                    var inv = (from tmp1 in ctx.Inwestycja
                               where tmp1.Data.Day == data_aktualna.Day
                               where tmp1.Nazwa == nazwa
                               select tmp1).First();

                    var user = (from tmp in ctx.Użytkownik
                                where tmp.Hasło == userPasswd
                                select tmp).First();
                    //MessageBox.Show(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex-1].Value.ToString());
                    try
                    {
                        int ilość = Int32.Parse(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString());
                        // skąd wziąć kurs? sprawdzić czy ma tyle inwestycji
                        //  MessageBox.Show("Brak środków");

                        var rekord = (from tmp in Portfel.Rekords
                                      where tmp.Nazwa == inv.Nazwa
                                      select tmp).First();
                        if (rekord.Liczba >= ilość)
                        {
                            Operacja operation = new Operacja() { Transakcja = transakcja.sprzedaż, StempelCzasowy = data_aktualna, Ilość = ilość, Inwestycja = inv };
                            user.Operacja.Add(operation);
                            user.StanKonta += ilość * inv.Kurs * inv.Przelicznik;
                            MessageBox.Show("sprzedałeś inwestycje " + inv.Nazwa);
                        }
                        else
                            MessageBox.Show("Nie masz tylu produktów!");
                        Odśwież();
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Uzupełnij: pole ilość " + ex);
                    }
                }
                else
                    MessageBox.Show("Jest weekend - mam wolne");
            
            }
        } //cell_click event

        void timer_Tick(object sender, EventArgs e)
        {
           
           var GameTime = new DateTime(2014, 1, 2);
            TimeSpan duration = DateTime.Now - StartTime;
            long ticks = duration.Ticks;
            ticks *= (24*60*6);
            GameTime += TimeSpan.FromTicks(ticks);
            if (GameTime <= new DateTime(2014, 1, 16))
            {
                czas_aktualny.Text = GameTime.ToString();
                OdświeżPortfel();
                OdświeżFirmy();
                Odśwież();
            }
           
                if (GameTime == new DateTime(2014, 1, 16))
                    MessageBox.Show("Ostatni dzień - wszystko działa");

          //  czas_aktualny.Text =DateTime.Now.ToString();
         }

        void OdświeżPortfel()
        {

            try 
            {
                DateTime data_aktualna = DateTime.Parse(czas_aktualny.Text);
                if (!data_aktualna.IsHoliday())
                {
                    // MessageBox.Show("Kalkuluje");
                    foreach (var rekord in Portfel.Rekords)
                    {
                        var inv = (from tmp in ctx.Inwestycja
                                   where tmp.Nazwa == rekord.Nazwa
                                   where tmp.Data.Day == data_aktualna.Day
                                   select tmp).First();

                        rekord.KursAktualny = inv.Kurs;
                        rekord.OkresInwestycji++;

                        Portfel.OperacjeDodane.OrderBy(o => o.StempelCzasowy).ToList();
                        //  MessageBox.Show(Portfel.OperacjeDodane[0].ToString());
                        rekord.Zysk = (inv.Kurs - Portfel.OperacjeDodane[0].Inwestycja.Kurs) * rekord.Liczba;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd aktualizacji portfela "+ex);
            }
        }//Odśwież portfel

        void OdświeżFirmy()
        {
            DateTime data_aktualna = DateTime.Parse(czas_aktualny.Text);
            
            foreach(var fm in ctx.Firma)
            {
                if (fm.Name == "Przykład")
                {
                    ;
                }
                else
                {
                    if (!data_aktualna.IsHoliday())
                    {
                        var inv = (from tmp in fm.Archiwum
                                   where tmp.Nazwa == fm.Name
                                   where tmp.Data.Day == data_aktualna.Day
                                   select tmp).First();

                        fm.AktualnaInwestycja = inv;
                    }
                }

            }
        }

        

    } //MainPanel
} //GraInwestycyjna namespace
