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


namespace GraInwestycyjna
{
    public partial class MainPanel : Form
    {
        //clock
        Timer t = new Timer();
        int WIDTH = 50, HEIGHT = 50, secHAND = 20, minHAND = 110, hrHAND = 80;
        //center
        int cx, cy;
        Bitmap bmp;
        Graphics g;
        const int MAX_COUNT = 60;

        string userPasswd = ConfigurationManager.AppSettings["UserPasswd"];
        Portfel Portfel = new Portfel();
        public GameDbContext ctx = new GameDbContext();
        DataGridViewButtonColumn buy = new DataGridViewButtonColumn();
        DataGridViewButtonColumn sell = new DataGridViewButtonColumn();
        DataGridViewColumn  iloscCol_buy = new DataGridViewColumn(); // add a column to the grid
        DataGridViewCell cell_buy = new DataGridViewTextBoxCell();
        DataGridViewColumn iloscCol_sell = new DataGridViewColumn(); // add a column to the grid
        DataGridViewCell cell_sell = new DataGridViewTextBoxCell();
        
        List<Inwestycja> dzisiejsze_inwestycje = new List<Inwestycja>();
        DateTime StartTime;
        //DateTime PauseTime;
        bool play_ = false;
        bool pause_ = false;
        int counter = 0;
      
        DateTime GameTime = new DateTime(2014, 1, 2);
        
        public MainPanel()
        {
            StartTime = DateTime.Now;
            var timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval =10* 1000; //1 seconds *10
            timer.Start();

           
            InitializeComponent();
            //Clock clock = new Clock(pictureBox1);

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

                dataGridView1.Columns["Nazwa"].ReadOnly = true;
                dataGridView1.Columns["Kurs"].ReadOnly = true;
                dataGridView1.Columns["Przelicznik"].ReadOnly = true;
                dataGridView1.Columns["Data"].ReadOnly = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wyświetlania Rynku " + ex);
            }
       }

       private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
       {
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
                         //   Odśwież();
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
            var user = (from tmp in ctx.Użytkownik
                        where tmp.Hasło == userPasswd
                        select tmp).First();

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

       



            //!!!!!!!!
            int row_scroll = 0, col = 1, row = 1;
            if (dataGridView2.Rows.Count > 0 && dataGridView2.Columns.Count > 0)
            {
                if (dataGridView2.CurrentCell != null && dataGridView2.FirstDisplayedCell != null)
                {
                    col = dataGridView2.CurrentCell.ColumnIndex;
                    row = dataGridView2.CurrentCell.RowIndex;
                    row_scroll = dataGridView2.FirstDisplayedCell.RowIndex;
                }

            }

            dataGridView2.DataSource = Portfel.Rekords.ToList();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            iloscCol_sell.HeaderText = "Ilość";
            iloscCol_sell.Name = "ilość";
            iloscCol_sell.Visible = true;
            iloscCol_sell.Width = 40;
            iloscCol_sell.CellTemplate = cell_buy;

            dataGridView2.Columns["Nazwa"].ReadOnly = true;
            dataGridView2.Columns["KursAktualny"].ReadOnly = true;
            dataGridView2.Columns["Przelicznik"].ReadOnly = true;
            dataGridView2.Columns["Typ"].ReadOnly = true;
            dataGridView2.Columns["Liczba"].ReadOnly = true;
            dataGridView2.Columns["ŚredniKursKupna"].ReadOnly = true;
            dataGridView2.Columns["OkresInwestycji"].ReadOnly = true;
            dataGridView2.Columns["Zysk"].ReadOnly = true;


            sell.Name = "Sell";
            sell.Text = "sell";
            sell.UseColumnTextForButtonValue = true;
            if (dataGridView2.Columns["ilość"] == null)
                dataGridView2.Columns.Add(iloscCol_sell);

            if (dataGridView2.Columns["sell"] == null)
                dataGridView2.Columns.Add(sell);

            dataGridView2.Columns["ŚredniKursKupna"].DefaultCellStyle.Format = "n2";
            dataGridView2.Columns["Zysk"].DefaultCellStyle.Format = "n2";


            var grupy = new Dictionary<string, int>();

            foreach (var gr in ctx.Grupa)
            {
                grupy.Add(gr.Name, 0);

            }

            foreach (var rekord in Portfel.Rekords)
            {
                grupy[rekord.Typ] += rekord.Liczba;
            }

            List<string> keyList = new List<string>(grupy.Keys);
            List<int> valueList = new List<int>(grupy.Values);


            // bind the datapoints
            if (keyList != null && valueList != null)
            {
                chart2.Series["portfel"].Points.DataBindXY(keyList, valueList);
                foreach (DataPoint dp in chart2.Series["portfel"].Points)
                    dp.IsEmpty = (dp.YValues[0] == 0) ? true : false;

                chart2.Invalidate();
            }            

            if (row_scroll != 0 && row_scroll < dataGridView2.Rows.Count)
            {
                dataGridView2.FirstDisplayedScrollingRowIndex = row_scroll;
            }
            if (dataGridView2.Rows.Count > 0 && dataGridView2.Columns.Count > 0 && dataGridView2.CurrentCell != null)
                if (col < dataGridView2.Columns.Count && row < dataGridView2.Rows.Count)
                    dataGridView2.CurrentCell = dataGridView2[col, row];
            //!!!!!!!!
            

        }

        private void WyswietlHistorie()
        {
                try
                {
                    var data = (from user in ctx.Użytkownik
                                where user.Hasło == userPasswd
                                select user).First();

                    int row_scroll=0,col=1,row=1;
                    if (dataGridView3.Rows.Count > 0 && dataGridView3.Columns.Count > 0)
                    {
                        if (dataGridView3.CurrentCell != null && dataGridView3.FirstDisplayedCell != null)
                        {                             
                            col = dataGridView3.CurrentCell.ColumnIndex;
                            row = dataGridView3.CurrentCell.RowIndex;
                            row_scroll = dataGridView3.FirstDisplayedCell.RowIndex;
                        }

                    }

                    dataGridView3.DataSource = data.Operacja.ToList();
                    dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView3.Columns["Id"].Visible = false;
                    dataGridView3.Columns["InwestycjaId"].Visible = false;
                    dataGridView3.Columns["UżytkownikId"].Visible = false;
                    dataGridView3.Columns["Użytkownik"].Visible = false;
                    dataGridView3.Columns["Inwestycja"].Visible = false;

                    dataGridView3.Columns["Transakcja"].ReadOnly = true;
                    dataGridView3.Columns["StempelCzasowy"].ReadOnly = true;
                    dataGridView3.Columns["Ilość"].ReadOnly = true;

                    if (row_scroll != 0 && row_scroll < dataGridView3.Rows.Count)
                    {
                        dataGridView3.FirstDisplayedScrollingRowIndex = row_scroll;
                    }
                    if (dataGridView3.Rows.Count > 0 && dataGridView3.Columns.Count > 0 && dataGridView3.CurrentCell != null)
                        if(col<dataGridView3.Columns.Count && row <dataGridView3.Rows.Count)
                        dataGridView3.CurrentCell = dataGridView3[col,row];
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
             stan_konta.ReadOnly = true;
            
        }


        public void WyswietlRynek()
        {
            DateTime data_aktualna = Convert.ToDateTime(czas_aktualny.Text);
            if (!data_aktualna.IsHoliday())
            {
                dzisiejsze_inwestycje.Clear();
                foreach (var fm in ctx.Firma)
                {
                    dzisiejsze_inwestycje.Add(fm.AktualnaInwestycja);
                }

                try
                {


                    int row_scroll = 0, col = 1, row = 1;
                    if (dataGridView4.Rows.Count > 0 && dataGridView4.Columns.Count > 0)
                    {
                        if (dataGridView4.CurrentCell != null && dataGridView4.FirstDisplayedCell != null)
                        {
                            col = dataGridView4.CurrentCell.ColumnIndex;
                            row = dataGridView4.CurrentCell.RowIndex;
                            row_scroll = dataGridView4.FirstDisplayedCell.RowIndex;
                        }

                    }

                    dataGridView4.DataSource = dzisiejsze_inwestycje.ToList();
                    dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView4.Columns["Id"].Visible = false;
                    dataGridView4.Columns["GrupaId"].Visible = false;
                    dataGridView4.Columns["Grupa"].Visible = false;
                    dataGridView4.Columns["FirmaId"].Visible = false;
                    dataGridView4.Columns["Firma"].Visible = false;
                    dataGridView4.Columns["Data"].Visible = false;

                    dataGridView4.Columns["Nazwa"].ReadOnly = true;
                    dataGridView4.Columns["Kurs"].ReadOnly = true;
                    dataGridView4.Columns["Przelicznik"].ReadOnly = true;
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

                    if (row_scroll != 0 && row_scroll < dataGridView4.Rows.Count)
                    {
                        dataGridView4.FirstDisplayedScrollingRowIndex = row_scroll;
                    }
                    if (dataGridView4.Rows.Count > 0 && dataGridView4.Columns.Count > 0 && dataGridView4.CurrentCell != null)
                        if (col < dataGridView4.Columns.Count && row < dataGridView4.Rows.Count)
                        {
                            dataGridView4.CurrentCell = dataGridView4[col, row];
                            //MessageBox.Show(dataGridView4.CurrentRow.Cells["Nazwa"].Value.ToString());
                            wykres(dataGridView4.CurrentRow.Cells["Nazwa"].Value.ToString());
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
            WyswietlHistorie();
            WyswietlStanKonta();
    
        }

  

        public void WyswietlFirmy()
        {
            dataGridView5.DataSource = ctx.Firma.ToList();
            dataGridView5.Columns["Id"].ReadOnly = true;
            dataGridView5.Columns["Name"].ReadOnly = true;

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
                    try
                    {
                        int ilość = Int32.Parse(dataGridView2.Rows[e.RowIndex].Cells[dataGridView2.Columns["Ilość"].Index].Value.ToString());
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
         /*   if (play_)
            {
                var GameTime = new DateTime(2014, 1, 2);
                TimeSpan duration = DateTime.Now - StartTime;
                long ticks = duration.Ticks;
                ticks *= (24 * 60 * 6);
                GameTime += TimeSpan.FromTicks(ticks);
                if (pause_)
                {
                    StartTime = DateTime.Now;
                    play_ = false;
                }
                //PauseTime = GameTime;
                if (GameTime < new DateTime(2014, 1, 16))
                {
                    czas_aktualny.Text = GameTime.ToString();
                    Odśwież();
                }

                if (GameTime == new DateTime(2014, 1, 16))
                    MessageBox.Show("Ostatni dzień - wszystko działa");
            }*/
         }

        void OdświeżPortfel()
        {
            try 
            {
                DateTime data_aktualna = DateTime.Parse(czas_aktualny.Text);
                if (!data_aktualna.IsHoliday())
                {
                    foreach (var rekord in Portfel.Rekords)
                    {
                        var inv = (from tmp in ctx.Inwestycja
                                   where tmp.Nazwa == rekord.Nazwa
                                   where tmp.Data.Day == data_aktualna.Day
                                   select tmp).First();

                        rekord.KursAktualny = inv.Kurs;
                        rekord.OkresInwestycji++;

                        Portfel.OperacjeDodane.OrderBy(o => o.StempelCzasowy).ToList();
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
                    if (data_aktualna.IsHoliday())
                        data_aktualna=data_aktualna.GetLastWorkingDay();
                    try
                    {
                        var inv = (from tmp in fm.Archiwum
                                   where tmp.Nazwa == fm.Name
                                   where tmp.Data.Day == data_aktualna.Day
                                   select tmp).First();

                        fm.AktualnaInwestycja = inv;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("odświeżanie firm " + fm.Name+" "+ex);
                    }
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
            ;

            //wybieranie dóbr do wyświetleniach wykresów

            /*   if(dataGridView4.CurrentCell != null)
               {
                   int col = dataGridView4.CurrentCell.RowIndex;
                   int row = dataGridView4.CurrentCell.ColumnIndex;

                   string cur = dataGridView4[col, row].Value.ToString();
                   MessageBox.Show("wybrana waluta: " + cur);
               }*/
        }

        public void wykres(string nazwa)
        {
            this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MMM";
            List<DateTime> daty = new List<DateTime>();
            List<DateTime> daty_śr = new List<DateTime>();
            List<double> kursy = new List<double>();
            List<double> średnia_5_dni = new List<double>();
            var tmp = (from fm in ctx.Firma
                       where fm.Name == nazwa //"AUD"
                       select fm).First();

            // średnia krocząca
            int counter = 0, okres = 5;
            List<double> elem = new List<double>();
            foreach (var t in tmp.Archiwum)
            {
                daty.Add(t.Data);
                kursy.Add(t.Kurs);
                if (counter < okres)
                {
                    elem.Add(t.Kurs);
                    counter++;
                }
                if (counter == okres)
                {
                    double średnia = 0;

                    for (int i = (elem.Count - okres); i < elem.Count; i++)
                    {
                        średnia += elem[i];
                    }
                    średnia_5_dni.Add(średnia / okres);
                    daty_śr.Add(t.Data);
                    counter = okres - 1;
                }
            }

            this.chart1.Series["średnia5dni"].ChartType = SeriesChartType.FastLine;
            this.chart1.Series["średnia5dni"].XValueType = ChartValueType.DateTime;

            this.chart1.Series["historia"].ChartType = SeriesChartType.FastLine;
            this.chart1.Series["historia"].XValueType = ChartValueType.DateTime;

            //this.chart1.ChartAreas[0].AxisY.Minimum = 2;
            //this.chart1.ChartAreas[0].AxisY.Maximum = 3;
            chart1.Titles.Clear();   // Unnecessary if you have already clear
            Title Title = new Title(nazwa, Docking.Top, new Font("Verdana", 12), Color.Black);
            chart1.Titles.Add(Title);


            // bind the datapoints
            chart1.Series["historia"].Points.DataBindXY(daty, kursy);
            chart1.Series["historia"].Color = Color.Red;
            chart1.Series["średnia5dni"].Points.DataBindXY(daty_śr, średnia_5_dni);
            chart1.Series["średnia5dni"].Color = Color.Green;
        }

        private void play_Click(object sender, EventArgs e)
        {
            play_ = true;
            pause_ = false;
        }

        private void stop_Click(object sender, EventArgs e)
        {
            play_ = false;
            czas_aktualny.Text = new DateTime(2014, 1, 2).ToString();
            StartTime = DateTime.Now;
        }

        private void pause_Click(object sender, EventArgs e)
        {
            pause_ = true;
            StartTime = DateTime.Now;
            play_ = false;
        }


        private void t_Tick(object sender, EventArgs e)
        {
            //create graphics
            if (play_)
            {
                g = Graphics.FromImage(bmp);

                //get time
                int ss = DateTime.Now.Second;
                int mm = DateTime.Now.Minute;
                int hh = DateTime.Now.Hour;

                int[] handCoord = new int[2];

                //clear
                g.Clear(Color.Transparent);
                //draw circle
                g.DrawEllipse(new Pen(Color.LightGray, 2f), 0, 0, WIDTH, HEIGHT);
                //second hand
                handCoord = msCoord(ss, secHAND);
                g.DrawLine(new Pen(Color.LightGray, 2f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));
                //load bmp in picturebox1
                pictureBox1.Image = bmp;
                g.Dispose();

                if (counter == MAX_COUNT)
                {

                   
              //      TimeSpan duration = DateTime.Now - StartTime;
              //      long ticks = duration.Ticks;
               //     MessageBox.Show("Ticks in one day "+ticks);
             //       ticks *= (24 * 60);
             //       day = ticks;
             //       GameTime += TimeSpan.FromTicks(ticks);
                    TimeSpan time_span = new TimeSpan(1, 0, 0, 0, 0);
                    GameTime += time_span;
                    //PauseTime = GameTime;
                  //  if (GameTime < new DateTime(2014, 1, 16))
                  //  {
                        czas_aktualny.Text = GameTime.ToString();
                        Odśwież();
                  //  }

                  /*  if (GameTime == new DateTime(2014, 1, 16))
                        MessageBox.Show("Ostatni dzień - wszystko działa");
                    */
                    counter = 0;
                }
                counter++;
            }
        }

        //coord for minute and second hand
        private int[] msCoord(int val, int hlen)
        {
            int[] coord = new int[2];
            val *= 6;   //each minute and second make 6 degree

            if (val >= 0 && val <= 180)
            {
                coord[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }

        private void MainPanel_Load(object sender, EventArgs e)
        {
            //create bitmap
            bmp = new Bitmap(WIDTH + 1, HEIGHT + 1);

            //center
            cx = WIDTH / 2;
            cy = HEIGHT / 2;

            //backcolor
         //   this.BackColor = Color.Red;

            //timer
            t.Interval = 1000;      //half second in millisecond
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();

        }

        private void next_right_Click(object sender, EventArgs e)
        {

            TimeSpan time_span = new TimeSpan(1, 0, 0, 0, 0);
            GameTime += time_span;
            czas_aktualny.Text = GameTime.ToString();
            Odśwież();
        }

        private void next_left_Click(object sender, EventArgs e)
        {
            TimeSpan time_span = new TimeSpan(1, 0, 0, 0, 0);
            GameTime -= time_span;
            czas_aktualny.Text = GameTime.ToString();
            Odśwież();
        }



    } //MainPanel
} //GraInwestycyjna namespace
