
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Investments
{
    class PobieranieDanych
    {
        public void PobierzKursyWalut()
        {
            Console.WriteLine("Pobieramy dane z NBP!");

            using (WebClient Client = new WebClient())
            {
                Client.DownloadFile("http://www.nbp.pl/kursy/xml/dir.txt", "kursy.txt");

                string line = "";
                var liczbaLinii = 0;
                try
                {
                    string path = "kursy.txt";
                    using (StreamReader sr = File.OpenText(path))
                    {
                        line = sr.ReadLine();
                        liczbaLinii = File.ReadLines(path).Count();
                        //   Console.WriteLine(liczbaLinii);
                        //   Console.WriteLine(line);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }



                char[] buffor = new char[line.Length];

                using (StringReader sr = new StringReader(line))
                {
                    // for (int i=1;i<=liczbaLinii;i++)
                    //{
                    // Read 11 characters from the string into the array.
                    sr.Read(buffor, 0, 11);
                    if (buffor[0] == 'c')
                    {
                        string nazwa = new string(buffor);
                        string url = "http://www.nbp.pl/kursy/xml/" + nazwa + ".xml";
                        Console.WriteLine(buffor);

                        Console.WriteLine(url);
                        nazwa = "dane.xml";
                        Client.DownloadFile(url, nazwa);


                        try
                        {
                            // Create an instance of StreamReader to read from a file.
                            // The using statement also closes the StreamReader.
                            using (StreamReader str = new StreamReader(nazwa))
                            {
                                string line1;
                                // Read and display lines from the file until the end of 
                                // the file is reached.
                                while ((line1 = str.ReadLine()) != null)
                                {
                                    //   Console.WriteLine(line1);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            // Let the user know what went wrong.
                            Console.WriteLine("The file could not be read:");
                            Console.WriteLine(e.Message);
                        }

                        //Parsowanie XMLa:
                        XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object

                        xmlDoc.XmlResolver = null; // ignoruj plik dtd
                        xmlDoc.Load(nazwa); // Load the XML document from the specified file

                        // Get elements
                        XmlNodeList pozycja = xmlDoc.GetElementsByTagName("pozycja");


                        // Display the results
                        Console.WriteLine("pozycja: " + pozycja[0].InnerText);

                        /*TODO:
                         * wyciągnąć informacje o wartość : cena kupna, cena sprzedaży, data, typ, przelicznik
                         * dla każdej waluty stworzyć osobną instancję klasy Inwestycja i od tego momentu można 
                         * wrzucać te dane do bazy danych
                         */


                    }
                    //}


                    // Read the rest of the string starting at the current string position.
                    // Put in the array starting at the 6th array member.
                    //  sr.Read(b, 5, line.Length - 13);
                    //    Console.WriteLine(b);
                }

                Console.Read();
            }
        }
    }
}

