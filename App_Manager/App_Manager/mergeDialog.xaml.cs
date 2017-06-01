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
    /// Interaction logic for mergeDialog.xaml
    /// </summary>
    public partial class mergeDialog : Window
    {
        public bool isSave = false;
        public mergeDialog()
        {
            InitializeComponent();
        }

        private void Text_GotFocus(Object sender, EventArgs e)
        {
            (sender as TextBox).Text = "";
        }

        public void save_Click(object sender, RoutedEventArgs e)
        {
            isSave = true;
            this.Close();
        }
    }
}
