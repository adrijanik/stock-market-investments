
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Investments
{

    class PobieranieDanych
    {
         //public List<Inwestycja> PobierzKursyWalut()
        public void PobierzKursyWalut()
        {
            MessageBox.Show("Pobieram waluty");
            // List<Inwestycja> waluty = new List<Inwestycja>();
            WebClient Client = new WebClient();
            using (var ctx = new GameDbContext())
            {
                // MessageBox.Show("Pobieramy dane z NBP!");
                try
                {
                    Client.DownloadFile("http://www.nbp.pl/kursy/xml/dir.txt", "kursy.txt");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Problem z pobieraniem katalogu z notowaniami walut "+ex);
                }
                string path = "kursy.txt";
                StreamReader sr = File.OpenText(path);
                string line = "";
                char[] buffor;
                string nazwa = "";
                bool zakoncz = false;
                while (true)
                {
                    do  // check if first letter of line is a which means this is the right file to read from
                    {
                        if ((line = sr.ReadLine()) != null)
                        {
                            buffor = new char[line.Length];
                            StringReader stream = new StringReader(line);
                            stream.Read(buffor, 0, 11);
                            nazwa = new string(buffor);
                          //  MessageBox.Show("Nazwa: "+nazwa);
                        }
                        else
                        {
                            MessageBox.Show("Zakończono wczytywanie danych - waluty");
                            zakoncz = true;
                            break;
                        }
                    } while (!(buffor[0] == 'a' && ((buffor[5]=='1' && buffor[6]=='4') || (buffor[5]==1 && buffor[6]==5))));
                    // MessageBox.Show("before while: " + buffor[0]);

                    if (zakoncz)
                        break;

                 /*   if (nazwa == "a150z040803")
                    {
                        MessageBox.Show("Koniec plików o poprawnej strukturze");
                        break;
                    }*/

                    string url = "http://www.nbp.pl/kursy/xml/" + nazwa + ".xml";
              //      MessageBox.Show(buffor.ToString());


                    nazwa = "dane" + nazwa + ".xml";
                //    MessageBox.Show(nazwa);
                    try
                    {
                        Client.DownloadFile(url, nazwa);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd pobierania pliku: " + nazwa + " " + ex);
                    }

                    try
                    {
                        // Create an instance of StreamReader to read from a file.
                        // The using statement also closes the StreamReader.
                        StreamReader str = new StreamReader(nazwa);
                        string line1;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        while ((line1 = str.ReadLine()) != null)
                        {
                            //   Console.WriteLine(line1);
                        }
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        MessageBox.Show("The file could not be read:" + e.Message);
                    }

                    //Parsowanie XMLa:
                    XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object

                    xmlDoc.XmlResolver = null; // ignoruj plik dtd
                    xmlDoc.Load(nazwa); // Load the XML document from the specified file

                    // Get elements
                    XmlNodeList data = xmlDoc.GetElementsByTagName("data_publikacji");

                    DateTime data_time = DateTime.Parse(data[0].InnerText);

                //    MessageBox.Show(data_time.ToString());

                    try
                    {
                        XmlNodeList pozycja = xmlDoc.SelectNodes("/tabela_kursow/pozycja");
                      //  MessageBox.Show("pobieram dane o kursach "+ pozycja.Count);
                        if (pozycja.Count == 5)
                        {
                            var grupa = (from gr in ctx.Grupa
                                         where gr.Name == "Waluty"
                                         select gr).First();


                            foreach (XmlNode xn in pozycja)
                            {
                                //  MessageBox.Show("Hello");
                                XmlNodeList lista_atrybutów = xn.ChildNodes;
                          //      MessageBox.Show("Ile atrybutow: " + lista_atrybutów.Count);
                                int przelicznik = Int32.Parse(lista_atrybutów[3].InnerText);
                          //      MessageBox.Show("Przelicznik: " + lista_atrybutów[3].InnerText);
                                string kurs1_txt = lista_atrybutów[5].InnerText;
                                double kurs = Convert.ToDouble(kurs1_txt);
                           //     MessageBox.Show("Kurs " + kurs);
                                Firma fm = new Firma() { Name = "zNeta" };
                                Inwestycja tmp = new Inwestycja { Firma = fm, Nazwa = lista_atrybutów[4].InnerText, Kurs = kurs, Przelicznik = przelicznik, Data = data_time, Grupa = grupa };
                                // waluty.Add(tmp);
                                ctx.Firma.Add(fm);
                                ctx.Inwestycja.Add(tmp);

                           //     MessageBox.Show(tmp.ToString());
                            }
                            ctx.SaveChanges();
                        }
                        else
                        {
                         //   MessageBox.Show("Dla 4 atrybutów");
                            var grupa = (from gr in ctx.Grupa
                                         where gr.Name == "Waluty"
                                         select gr).First();


                            foreach (XmlNode xn in pozycja)
                            {
                                //  MessageBox.Show("Hello");
                                XmlNodeList lista_atrybutów = xn.ChildNodes;
                         //       MessageBox.Show("Ile atrybutow: " + lista_atrybutów.Count);
                                try
                                {
                                    int przelicznik = Int32.Parse(lista_atrybutów[1].InnerText);
                                
                             //   MessageBox.Show("Przelicznik: " + lista_atrybutów[1].InnerText);
                                string kurs1_txt = lista_atrybutów[3].InnerText;
                                double kurs = Convert.ToDouble(kurs1_txt);
                             //   MessageBox.Show("Kurs " + kurs);
                                Firma fm = new Firma() { Name = "zNeta" };
                                Inwestycja tmp = new Inwestycja { Firma = fm, Nazwa = lista_atrybutów[2].InnerText, Kurs = kurs, Przelicznik = przelicznik, Data = data_time, Grupa = grupa };
                                // waluty.Add(tmp);
                                ctx.Firma.Add(fm);
                                ctx.Inwestycja.Add(tmp);

                              //  MessageBox.Show(tmp.ToString());
                                }
                                catch (Exception ex) { MessageBox.Show("Parsowanie przelicznika: " + ex); }
                            }
                            ctx.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show("Błąd wyłuskiwania danych o walutach " + ex);
                    }

              
                }
                // return waluty;
            }
        }

        public void PobierzNotowaniaGiełdowe()
        {
            ArchiveData StockData = new ArchiveData();
            using (var ctx = new GameDbContext())
            {
                string line;
                System.IO.StreamReader file =
                    new System.IO.StreamReader(@"C:\Users\Adri\Desktop\Bats_Stocks.txt");
                while ((line = file.ReadLine()) != null)
                {
                    string nazwa = line;
                   // MessageBox.Show(nazwa);
                   

                using (var giełda = new WebClient())
                {
                    string data="";
                    bool exception = false;
                    try
                    {
                        data = giełda.DownloadString("http://dev.markitondemand.com/Api/v2/InteractiveChart/json?parameters=%7B%22Normalized%22%3Atrue%2C%22NumberOfDays%22%3A545%2C%22DataPeriod%22%3A%22Day%22%2C%22Elements%22%3A%5B%7B%22Symbol%22%3A%22" + nazwa + "%22%2C%22Type%22%3A%22price%22%2C%22Params%22%3A%5B%22c%22%5D%7D%5D%7D");
                    }catch (Exception ex)
                    {
                        exception = true;
                    }
                        if (!exception)
                    {     
                    StockData = JsonConvert.DeserializeObject<ArchiveData>(data);
                    //int n=StockData.Elements.Count();

                    var akcje = (from gr in ctx.Grupa
                                 where gr.Name == "Akcje"
                                 select gr).First();

                 //   Grupa akcje = new Grupa() { Name = "Akction" };
                   
                    foreach (var elem in StockData.Elements)
                    {
                        for (int i = 0; i < elem.DataSeries.close.values.Count(); i++)
                        {
                            // MessageBox.Show(elem.DataSeries.close.values[i].ToString() + " " + StockData.Dates[i].ToString());
                            Inwestycja tmp = new Inwestycja();
                            tmp.Data = StockData.Dates[i];
                            tmp.Kurs = elem.DataSeries.close.values[i];
                            tmp.Przelicznik = 1;
                            tmp.Nazwa = elem.Symbol;
                            tmp.Grupa = akcje;
                            ctx.Inwestycja.Add(tmp);

                        }

                    }
                    try
                    { ctx.SaveChanges(); }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                        exception = false;
                }
                MessageBox.Show("I'm done");
                break;
            }
        }
        }
        

    }
    
    public class Close
    {
        public double min { get; set; }
        public double max { get; set; }
        public DateTime maxDate { get; set; }
        public DateTime minDate { get; set; }
        public List<double> values { get; set; }
    }

    public class DataSeries
    {
        public Close close { get; set; }
    }

    public class Element
    {
        public string Currency { get; set; }
        public object TimeStamp { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public DataSeries DataSeries { get; set; }
    }

    public class ArchiveData
    {
        public object Labels { get; set; }
        public List<double> Positions { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<Element> Elements { get; set; }
    }
}






