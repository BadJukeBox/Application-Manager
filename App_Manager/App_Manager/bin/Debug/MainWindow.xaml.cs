using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Navigation;


namespace App_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void toGrpScreen_Click(object sender, RoutedEventArgs e)
        {
            GroupListScreen GLS = new GroupListScreen();
            GLS.Show();
        }

        private void toForm_Click(object sender, RoutedEventArgs e)
        {
            InputForm InForm = new InputForm();
            InForm.Show();
        }

        private void GroupOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true) { }
               // txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        }
    }
}
