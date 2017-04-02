using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Navigation;
using System.Data.SQLite;

namespace App_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteManager SQLMan;
        MenuBar mBar;
        public MainWindow()
        {
            InitializeComponent();
            SQLMan = new SQLiteManager();
            mBar = new MenuBar(SQLMan);
            SQLMan.Init();
            SQLMan.CreateTable("Big4");
            SQLMan.OpenTable("Big4");
            
        }

        private void NewGroup_Click(object sender, RoutedEventArgs e)
        {
            //SQLMan.CreateTable();
            GroupListScreen GLS = new GroupListScreen(SQLMan);
            GLS.Show();
        }

        private void toGrpScreen_Click(object sender, RoutedEventArgs e)
        {
            GroupListScreen GLS = new GroupListScreen(SQLMan);
            GLS.Show();
        }

        private void toForm_Click(object sender, RoutedEventArgs e)
        {
            InputForm InForm = new InputForm(SQLMan);
            InForm.Show();
        }

        private void GroupOpen_Click(object sender, RoutedEventArgs e)
        {
           
        }
        
    }
    
}
