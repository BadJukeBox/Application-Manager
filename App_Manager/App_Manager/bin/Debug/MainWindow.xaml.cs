using System.Windows;
using System.Windows.Controls;

namespace App_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteManager SQLMan;
        MenuBar menu;
        public MainWindow()
        {
            InitializeComponent();
            SQLMan = new SQLiteManager();
            SQLMan.Init();
            menu = new MenuBar(SQLMan, this);
            menu.generateGroups(OpenMenu);
            menu.generateGroups(delMenu);
        }

        private void NewGroup_Click(object sender, RoutedEventArgs e)
        {
            menu.createNewGroup();
            menu.generateGroups(OpenMenu);
            menu.generateGroups(delMenu);
        }

        public void refreshList()
        {
            menu.generateGroups(OpenMenu);
            menu.generateGroups(delMenu);
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            menu.ImportFileClick();
        }

        private void merge_Click(object sender, RoutedEventArgs e)
        {
            menu.MergeTables();
        }
    }
    
}
