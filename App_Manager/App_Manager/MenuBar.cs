using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace App_Manager
{
    class MenuBar
    {
        SQLiteManager SQLMan;
        MenuItem groupItem;
        nameDialog nD;
        MainWindow mainW;
        public MenuBar(SQLiteManager SQLman, MainWindow main)
        {
            SQLMan = SQLman;
            mainW = main;
        }

        public void ImportFileClick()
        {

        }

        public void generateGroups(MenuItem openMenu)
        {
            List<String> groups = SQLMan.listTables();
            openMenu.Items.Clear();
            foreach (String group in groups)
            {
                groupItem = new MenuItem();
                groupItem.Header = group;
                groupItem.Click += openGroupClick;
                openMenu.Items.Add(groupItem);
            }
        }

        private void openGroupClick(object sender, RoutedEventArgs e)
        {
            MenuItem mItem = sender as MenuItem;
            GroupListScreen gLS = new GroupListScreen(SQLMan, mItem.Header.ToString());
            gLS.Show();
        }

        public void createNewGroup()
        {
            nD = new nameDialog();
            nD.ShowDialog();
            nD.Closed += Form_Closed;
        }

        private void Form_Closed(object sender, EventArgs e)
        {
            String name = nD.ResponseBlock.Text;
            SQLMan.CreateTable(name);
            GroupListScreen newGroup = new GroupListScreen(SQLMan, name);
            newGroup.Show();
            mainW.refreshList();
        }
    }
}
