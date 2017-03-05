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
    /// Interaction logic for InputForm.xaml
    /// </summary>
    public partial class InputForm : Window
    {
        SQLiteManager SQLMan;
        public InputForm(SQLiteManager SQLman)
        {
            this.SQLMan = SQLman;
            InitializeComponent();
        }

        public string Company_Name
        {
            get { return Company.Text; }
        }
        public string Position_Name
        {
            get { return Position.Text; }
        }
        public string Date_Text
        {
            get { return Date_of_App.Text; }
        }
        public string Req_ID_Text
        {
            get { return Requisition_ID.Text; }
        }
        public string Additional_Text
        {
            get { return Addition_Info.Text; }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SQLMan.insertFromForm(Company_Name, Position_Name, Date_Text, Req_ID_Text, Additional_Text);
            this.Close();
        }
    }
}
