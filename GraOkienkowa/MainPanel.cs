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
        public GameDbContext ctx = new GameDbContext();
        public MainPanel()
        {
            InitializeComponent();  
        }

        private void Rynek_Click(object sender, EventArgs e)
        {
            var data = ctx.Inwestycja.Select(p => p);

            dataGridView1.DataSource = data.ToList();

        }

        private void portfel_Click(object sender, EventArgs e)
        {
          //  var data = ctx.Użytkownik.Select(p => p);
            string userPasswd = ConfigurationManager.AppSettings["UserPasswd"];
           // MessageBox.Show(userPasswd);

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
                    MessageBox.Show("Błąd wyświetlania operacji w portfelu: "+ex);
                }
            }
            /*TODO:
             * Wybrać tylko inwestycje zalogowanego użytkownika
             * te, które niosą ze sobą jakąś informację (nr id jej nie niosą)
             */
        }

       
    }
}
