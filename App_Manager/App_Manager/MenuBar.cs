using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace App_Manager
{
    class MenuBar
    {
        SQLiteManager SQLMan;
        MenuItem groupItem;
        nameDialog nD;
        mergeDialog mD;
        MainWindow mainW;
        GroupListScreen glsmain;
        String importfilename;

        public MenuBar(SQLiteManager SQLman, MainWindow main)
        {
            SQLMan = SQLman;
            mainW = main;
        }

        public MenuBar(SQLiteManager SQLman, GroupListScreen GLSMain)
        {
            SQLMan = SQLman;
            glsmain = GLSMain;
        }

        public void ImportFileClick()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "CSV Files (.csv)|*.csv";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                if (filename.Contains(".csv"))
                {
                    foreach (QueryData n in (importReadFile(filename)))
                        SQLMan.insertFromForm(n.company, n.position, n.date, n.reqid, n.other);
                }
                else
                {
                    MessageBox.Show("Invalid file format! Try Again.");
                    return;
                }
                GroupListScreen newGroup = new GroupListScreen(SQLMan, importfilename);
                newGroup.ShowDialog();
                mainW.refreshList();
            }
        }

        private IEnumerable<QueryData> importReadFile(String filename)
        {
            string[] contents;
            contents = File.ReadAllLines(Path.ChangeExtension(filename, ".csv"));

            //Remove the file path and extension from the name for the group name
            string title = Path.GetExtension(filename);
            importfilename = filename.Substring(0, filename.Length - title.Length);
            int filepathend = importfilename.LastIndexOf('\\');
            importfilename = importfilename.Substring(filepathend+1); //+1 to account for the '\'

            //Import the file to QueryData entries.
            SQLMan.CreateTable(importfilename);
            return contents.Select(line =>
            {
                string[] data = line.Split(',');
                return new QueryData(data[0], data[1], data[2], data[3], data[4]);
            });
        }

        public void MergeTables()
        {
            mD = new mergeDialog();
            mD.Closed += mgForm_Closed;
            mD.ShowDialog();
        }

        public void generateGroups(MenuItem openMenu)
        {
            List<String> groups = SQLMan.listTables();
            openMenu.Items.Clear();
            foreach (String group in groups)
            {
                groupItem = new MenuItem();
                groupItem.Header = group;
                switch (openMenu.Header.ToString())
                {
                    case "Move Job(s)":
                        groupItem.Click += mvClick;
                        break;
                    case "Copy Job(s)":
                        groupItem.Click += cpClick;
                        break;
                    case "Delete":
                        groupItem.Click += rmClick;
                        break;
                    default:
                        groupItem.Click += openGroupClick;
                        break;
                }
                openMenu.Items.Add(groupItem);
            }
        }

        private void openGroupClick(object sender, RoutedEventArgs e)
        {
            if (GroupListScreen.IsOpen) return;
            MenuItem mItem = sender as MenuItem;
            GroupListScreen gLS = new GroupListScreen(SQLMan, mItem.Header.ToString());
            gLS.ShowDialog();
        }

        private void mvClick(object sender, RoutedEventArgs e)
        {
            MenuItem mItem = sender as MenuItem;
            glsmain.moveItems(mItem.Header.ToString());
        }

        private void cpClick(object sender, RoutedEventArgs e)
        {
            MenuItem mItem = sender as MenuItem;
            glsmain.copyItems(mItem.Header.ToString());
        }

        private void rmClick(object sender, RoutedEventArgs e)
        {
            MenuItem mItem = sender as MenuItem;
            if (MessageBox.Show("Are you sure you want to delete this group?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                SQLMan.deleteTable(mItem.Header.ToString());
                mainW.refreshList();
            }
        }

        public void createNewGroup()
        {
            nD = new nameDialog("newGroup");
            nD.Closed += ngForm_Closed;
            nD.ShowDialog();
        }

        private void ngForm_Closed(object sender, EventArgs e)
        {
            if (!nD.isSave) return;
            String name = nD.ResponseBlock.Text;
            if (name == "")
            {
                MessageBox.Show("Name box empty!");
                return;
            }
            SQLMan.CreateTable(name);
            GroupListScreen newGroup = new GroupListScreen(SQLMan, name);
            newGroup.ShowDialog();
            mainW.refreshList();
        }

        private void mgForm_Closed(object sender, EventArgs e)
        {
            if (!mD.isSave) return;
            String mFrom = mD.MergeFrom.Text;
            String mTo = mD.MergeTo.Text;  
            try
            {
                SQLMan.moveItems(mFrom, mTo);
            }
            catch(System.Data.SQLite.SQLiteException)
            {
                MessageBox.Show("One or more invalid groups!");
                return;
            }     
            foreach (QueryData n in (SQLMan.getData()))
            {
                Console.WriteLine(n.company);
            }
            SQLMan.deleteTable(mFrom);
            GroupListScreen newGroup = new GroupListScreen(SQLMan, mTo);
            newGroup.ShowDialog();
            mainW.refreshList();
        }
    }
}
