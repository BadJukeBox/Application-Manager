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
        }

        private void NewGroup_Click(object sender, RoutedEventArgs e)
        {
            menu.createNewGroup();
            menu.generateGroups(OpenMenu);
        }

        public void refreshList()
        {
            menu.generateGroups(OpenMenu);
        }

        private void toGrpScreen_Click(object sender, RoutedEventArgs e)
        {
            GroupListScreen GLS = new GroupListScreen(SQLMan, "Big4");
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
