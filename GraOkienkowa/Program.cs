using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraInwestycyjna
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GraInwestycyjna login_form = new GraInwestycyjna();
            if (login_form.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainPanel());
            }
            Application.Run(new GraInwestycyjna());
        }
    }
}
