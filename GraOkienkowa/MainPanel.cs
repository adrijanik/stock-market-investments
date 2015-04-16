﻿using System;
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
            var data = ctx.Użytkownik.Select(p => p);

            dataGridView1.DataSource = data.ToList();

            /*TODO:
             * Wybrać tylko inwestycje zalogowanego użytkownika
             * te, które niosą ze sobą jakąś informację (nr id jej nie niosą)
             */
        }

       
    }
}
