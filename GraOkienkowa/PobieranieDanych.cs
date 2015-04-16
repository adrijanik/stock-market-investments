
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

namespace Investments
{

    class PobieranieDanych
    {

        public List<Inwestycja> PobierzKursyWalut()
        {
            List<Inwestycja> waluty = new List<Inwestycja>();
            WebClient Client = new WebClient();

            // MessageBox.Show("Pobieramy dane z NBP!");

            Client.DownloadFile("http://www.nbp.pl/kursy/xml/dir.txt", "kursy.txt");
            string path = "kursy.txt";
            StreamReader sr = File.OpenText(path);
            string line = "";
            char[] buffor;
            string nazwa = "";

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
                    }
                    else
                    {
                        MessageBox.Show("Zakończono wczytywanie danych - waluty");
                        break;
                    }
                } while (buffor[0] != 'a');
                // MessageBox.Show("before while: " + buffor[0]);


                if (nazwa == "a150z040803")
                {
                    MessageBox.Show("Koniec plików o poprawnej strukturze");
                    break;
                }

                string url = "http://www.nbp.pl/kursy/xml/" + nazwa + ".xml";
                //      MessageBox.Show(buffor.ToString());

                      
                nazwa = "dane"+nazwa+".xml";
              //  MessageBox.Show(nazwa);
                try
                {
                    Client.DownloadFile(url, nazwa);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd pobierania pliku: "+nazwa+" "+ex);
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


                try
                {
                    XmlNodeList pozycja = xmlDoc.SelectNodes("/tabela_kursow/pozycja");
                    //       MessageBox.Show("pobieram dane o kursach");
                    Grupa grupa = new Grupa { Name = "Waluty" };
                    foreach (XmlNode xn in pozycja)
                    {

                        XmlNodeList lista_atrybutów = xn.ChildNodes;
                        int przelicznik = Int32.Parse(lista_atrybutów[2].InnerText);
                        string kurs1_txt = lista_atrybutów[4].InnerText;
                        double kurs = Convert.ToDouble(kurs1_txt);

                        Inwestycja tmp = new Inwestycja { Nazwa = lista_atrybutów[3].InnerText, Kurs = kurs, Przelicznik = przelicznik, Data = data_time, Grupa = grupa };
                        waluty.Add(tmp);

                    }

                }
                catch (Exception ex)
                {
                   // MessageBox.Show("Błąd wyłuskiwania danych o walutach " + ex);
                }
               
            }
            return waluty;
        }


    }

}




