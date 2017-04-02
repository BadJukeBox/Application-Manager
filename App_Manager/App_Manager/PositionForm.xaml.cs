using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace App_Manager
{
    /// <summary>
    /// Interaction logic for PositionForm.xaml
    /// </summary>
    public partial class PositionForm : Window
    {
        SQLiteManager SQLMan;
        String comp, pos, dat, req, oth;
        public PositionForm(String company, String position, String date, String reqid, String other, SQLiteManager SQLman)
        {
            InitializeComponent();
            SQLMan = SQLman;
            comp = Company.Text = company;
            pos = Position.Text = position;
            dat = Date_of_App.Text = date;
            req = Requisition_ID.Text = reqid;
            oth = Addition_Info.Text = other;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void save_Click(object sender, RoutedEventArgs e)
        {
            SQLMan.deleteFromTable(comp, pos, dat, req, oth);
            SQLMan.insertFromForm(Company.Text, Position.Text, Date_of_App.Text, Requisition_ID.Text, Addition_Info.Text);
            this.Close();
        }
    }
}
