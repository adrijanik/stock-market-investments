using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Investments;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
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
        
        List<Inwestycja> dzisiejsze_inwestycje = new List<Inwestycja>();
        DateTime StartTime;

        public MainPanel()
        {



            StartTime = DateTime.Now;
            var timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval =10* 1000; //1 seconds *10
            timer.Start();

           
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            czas_aktualny.Text =Convert.ToString(new DateTime(2014, 1, 2));
            WyswietlPortfel();
            WyswietlFirmy();
            OdświeżFirmy();
            WyswietlRynek();
            WyswietlDobra();
            WyswietlHistorie();
            WyswietlStanKonta();
        }

       public void WyswietlDobra()
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
                
    /*            iloscCol_buy.HeaderText = "Ilość";
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
                }*/
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wyświetlania Rynku " + ex);
            }
       }

       private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
       {
         //  MessageBox.Show(e.ColumnIndex.ToString());
           if (e.ColumnIndex == dataGridView4.Columns["Buy"].Index)
           {
              
               
                DateTime data_aktualna = DateTime.Parse(czas_aktualny.Text);
                if (!data_aktualna.IsHoliday())
                {
                    // MessageBox.Show("Row:"+(e.RowIndex + 1) + " Column: " + (e.ColumnIndex + 1) + "  Column button clicked ");
                    // kup daną inwestycję
                    Inwestycja inv = (Inwestycja)dataGridView4.Rows[e.RowIndex].DataBoundItem;
                    //  MessageBox.Show(inv.ToString());
                    //Kup inwestycję:
                    var user = (from tmp in ctx.Użytkownik
                                where tmp.Hasło == userPasswd
                                select tmp).First();
                    try
                    {
                        int ilość = Int32.Parse(dataGridView4.Rows[e.RowIndex].Cells[dataGridView4.Columns["ilość"].Index].Value.ToString());
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
            if (dataGridView2.Columns["ilość"] == null)
                dataGridView2.Columns.Add(iloscCol_sell);

            if (dataGridView2.Columns["sell"] == null)
                dataGridView2.Columns.Add(sell);
            
            dataGridView2.Columns["ŚredniKursKupna"].DefaultCellStyle.Format = "n2";
            dataGridView2.Columns["Zysk"].DefaultCellStyle.Format = "n2";


           // List<string> dobra = new List<string>();
           // List<int> ilość = new List<int>();
            var grupy = new Dictionary<string, int>();

            foreach (var gr in ctx.Grupa)
            {
                grupy.Add(gr.Name,0);

            }

            //this.chart1.Series["historia"].ChartType = SeriesChartType.FastLine;
            //this.chart1.Series["historia"].XValueType = ChartValueType.DateTime;
            //chart1.Series.Add(series);

           foreach (var rekord in Portfel.Rekords)
           {
               grupy[rekord.Typ] += rekord.Liczba;
           }

           List<string> keyList = new List<string>(grupy.Keys);        
           List<int> valueList = new List<int>(grupy.Values);
           

            // bind the datapoints
            chart2.Series["portfel"].Points.DataBindXY(keyList,valueList);
            foreach (DataPoint dp in chart2.Series["portfel"].Points)
                dp.IsEmpty = (dp.YValues[0] == 0) ? true : false;

            chart2.Invalidate();






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
                    dataGridView3.Columns["Id"].Visible = false;
                    dataGridView3.Columns["InwestycjaId"].Visible = false;
                    dataGridView3.Columns["UżytkownikId"].Visible = false;
                    dataGridView3.Columns["Użytkownik"].Visible = false;
                    dataGridView3.Columns["Inwestycja"].Visible = false;
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


        public void WyswietlRynek()
        {
            DateTime data_aktualna = Convert.ToDateTime(czas_aktualny.Text);
            if (!data_aktualna.IsHoliday())
            {
                dzisiejsze_inwestycje.Clear();
               // dzisiejsze_inwestycje = ctx.Inwestycja.Where(c => c.Data == data_aktualna).ToList();
                foreach (var fm in ctx.Firma)
                {
                    dzisiejsze_inwestycje.Add(fm.AktualnaInwestycja);
                }

                try
                {
                    dataGridView4.DataSource = dzisiejsze_inwestycje.ToList();
                    dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView4.Columns["Id"].Visible = false;
                    dataGridView4.Columns["GrupaId"].Visible = false;
                    dataGridView4.Columns["Grupa"].Visible = false;
                    dataGridView4.Columns["FirmaId"].Visible = false;
                    dataGridView4.Columns["Firma"].Visible = false;
                    dataGridView4.Columns["Data"].Visible = false;


                    iloscCol_buy.HeaderText = "Ilość";
                    iloscCol_buy.Name = "ilość";
                    iloscCol_buy.Visible = true;
                    iloscCol_buy.Width = 40;
                    iloscCol_buy.CellTemplate = cell_buy;


                    buy.Name = "Buy";
                    buy.Text = "buy";
                    buy.UseColumnTextForButtonValue = true;
                    if (dataGridView4.Columns["ilość"] == null)
                        dataGridView4.Columns.Add(iloscCol_buy);
                    if (dataGridView4.Columns["Buy"] == null)
                    {
                        dataGridView4.Columns.Add(buy);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd wyświetlania Rynku " + ex);
                }
            }
        }

        public void OdświeżRynek()
        {
            DateTime data_aktualna = Convert.ToDateTime(czas_aktualny.Text);
        /*    dzisiejsze_inwestycje.Clear();
            foreach (var fm in ctx.Firma)
            {
                dzisiejsze_inwestycje.Add(fm.AktualnaInwestycja);
            }
            dataGridView4.DataSource = dzisiejsze_inwestycje;*/
            if (!data_aktualna.IsHoliday())
            {
                dzisiejsze_inwestycje.Clear();
                dzisiejsze_inwestycje = ctx.Inwestycja.Where(c => c.Data == data_aktualna).ToList();
            }
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

            OdświeżFirmy();
            OdświeżPortfel();
            WyswietlRynek();
            OdświeżRynek();
   //         WyswietlFirmy();
            WyswietlHistorie();
            WyswietlStanKonta();
    
        }


        public void WyswietlFirmy()
        {
            dataGridView5.DataSource = ctx.Firma.ToList();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(e.ColumnIndex.ToString());
            
            if (e.ColumnIndex == dataGridView2.Columns["Sell"].Index)
            {
                // MessageBox.Show("Row:"+(e.RowIndex + 1) + " Column: " + (e.ColumnIndex + 1) + "  Column button clicked ");
                // kup daną inwestycję
                //Inwestycja inv = (Inwestycja)dataGridView2.Rows[e.RowIndex].DataBoundItem;
                //  MessageBox.Show(inv.ToString());
                //Kup inwestycję:
                DateTime data_aktualna = DateTime.Parse(czas_aktualny.Text);
                if (!data_aktualna.IsHoliday())
                {
                    string nazwa = dataGridView2.Rows[e.RowIndex].Cells[dataGridView2.Columns["Nazwa"].Index].Value.ToString();
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
                        int ilość = Int32.Parse(dataGridView2.Rows[e.RowIndex].Cells[dataGridView2.Columns["Ilość"].Index].Value.ToString());
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
           // CultureInfo provider = CultureInfo.InvariantCulture;
           // string format = "yyyy-mm-dd";

           // DateTime data_aktualna = DateTime.ParseExact(czas_aktualny.Text,format,provider);
            DateTime data_aktualna = DateTime.Parse(czas_aktualny.Text);
            foreach(var fm in ctx.Firma)
            {
                if (fm.Name == "Przykład")
                {
                    ;
                }
                else
                {
                    if (data_aktualna.IsHoliday())
                        data_aktualna=data_aktualna.GetLastWorkingDay();
                    var inv = (from tmp in fm.Archiwum
                                   where tmp.Nazwa == fm.Name
                                   where tmp.Data.Day == data_aktualna.Day
                                   select tmp).First();

                    fm.AktualnaInwestycja = inv;
                    
                }

            }
            try
            { ctx.SaveChanges(); }
            catch (Exception ex)
            {
                MessageBox.Show("zapis bazy przy odświeżaniu firm "+ex);
            }
        }

        private void load_Click(object sender, EventArgs e)
        {
          //  this.chart1.Series["historia"].Points.AddXY(0, 10);
          //  this.chart1.Series["historia"].Points.AddXY(10, 20);
          //  this.chart1.Series["historia"].Points.AddXY(20, 30);
           // this.chart1.Series["historia"].Points.AddXY(30, 40);
           /* this.chart1.DataSource = dataGridView4;
            this.chart1.Series["historia"].XValueMember = "Data";
            this.chart1.Series["historia"].YValueMembers = "Kurs";
            */

         //   var chartArea = new ChartArea();
         //   chartArea.AxisX.LabelStyle.Format = "dd/MMM\nhh:mm";
          this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MMM";
         /*   chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chart1.ChartAreas.Add(chartArea);
           */ 
            List<DateTime> daty = new List<DateTime>();
            List<double> kursy = new List<double>();
            var tmp = (from fm in ctx.Firma
                      where fm.Name == "AUD"
                      select fm).First();
            
            foreach (var t in tmp.Archiwum)
            {
                daty.Add(t.Data);
                kursy.Add(t.Kurs);
            }


           

            var xvals = new[]
                {
                    new DateTime(2012, 4, 4), 
                    new DateTime(2012, 4, 5), 
                    new DateTime(2012, 4, 6), 
                    new DateTime(2012, 4, 7)
                };
            var yvals = new[] { 1, 3, 7, 12 };

          //  var series = new Series();
          //  series.Name = "Series1";
           this.chart1.Series["historia"].ChartType = SeriesChartType.FastLine;
           this.chart1.Series["historia"].XValueType = ChartValueType.DateTime;
            //chart1.Series.Add(series);

            // bind the datapoints
            chart1.Series["historia"].Points.DataBindXY(daty, kursy);
            chart1.Invalidate();

        }

       

        

    } //MainPanel
} //GraInwestycyjna namespace
