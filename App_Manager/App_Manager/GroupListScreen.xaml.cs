using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace App_Manager
{
    /// <summary>
    /// Interaction logic for GroupListScreen.xaml
    /// </summary>
    public partial class GroupListScreen : Window
    {
        SQLiteManager SQLMan;
        public GroupListScreen(SQLiteManager SQLman)
        {
            InitializeComponent();
            SQLMan = SQLman;
            GenerateList();
        }

        private void GenerateList()
        {
            bool add1, add2, add3;
            add1 = true;
            add2 = add3 = false;
            List<QueryData> listGenerate = SQLMan.getData();
            stackPanel1.Height = stackPanel2.Height = stackPanel3.Height = listGenerate.Count * 200;
            Button newBtn;
            foreach (QueryData n in listGenerate)
            {
                newBtn = new Button();
                newBtn.Content = n.company + "\n" + n.position + "\n" + n.reqid;
                newBtn.Name = "Button";
                newBtn.Height = 50;
                newBtn.Width = 200;
                if (add1)
                {
                    stackPanel1.Children.Add(newBtn);
                    add1 = false;
                    add2 = true;
                }
                else if (add2)
                {
                    stackPanel2.Children.Add(newBtn);
                    add2 = false;
                    add3 = true;
                }
                else if (add3)
                {
                    stackPanel3.Children.Add(newBtn);
                    add3 = false;
                    add1 = true;
                }

            }
        }

    }
}
