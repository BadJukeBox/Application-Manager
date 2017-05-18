using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace App_Manager
{
    class GroupItemManagement
    {
        SQLiteManager SQLMan;
        List<Button> toBeDeleted = new List<Button>();
        List<Button> itemList = new List<Button>();
        public GroupItemManagement(SQLiteManager SQLman, GroupListScreen window)
        {
            SQLMan = SQLman;
        }
        public int getList()
        {
            List<QueryData> listGenerate = SQLMan.getData();
            return listGenerate.Count();
        }
        public void addItem()
        {
            InputForm inForm = new InputForm(SQLMan);
            inForm.Show();
            inForm.Closed += formClosed;
        }
        private void formClosed(object sender, EventArgs e)
        {

        }
        public void deleteItem()
        {

        }
        public void moveItem()
        {

        }
        public void showItemContents()
        {

        }
        public void selectItem(object sender)
        {
            Button btn = sender as Button;
            if (toBeDeleted.Contains(btn))
            {
                toBeDeleted.Remove(btn);
                btn.Background = Brushes.White;
            }
            else
            {
                toBeDeleted.Add(btn);
                btn.Background = Brushes.LightBlue;
            }
        }
    }
}
