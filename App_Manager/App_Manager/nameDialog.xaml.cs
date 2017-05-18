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
    /// Interaction logic for nameDialog.xaml
    /// </summary>
    public partial class nameDialog : Window
    {
        public nameDialog(String type)
        {
            InitializeComponent();
            switch (type)
            {
                case "newGroup":
                    Title.Text = "Enter name for new group.";
                    break;
                case "moveItems":
                    Title.Text = "Enter group to move to.";
                    break;
                case "copyItems":
                    Title.Text = "Enter group to copy to.";
                    break;
                default:
                    MessageBox.Show("Error, no type specified!");
                    this.Close();
                    break;
            }
        }

        public void save_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
