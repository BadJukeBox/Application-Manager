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
        List<Button> toBeDeleted = new List<Button>();

        public GroupListScreen(SQLiteManager SQLman)
        {
            InitializeComponent();
            SQLMan = SQLman;
            GenerateList(false);
        }
        private void deleteSelected(List<Button> butList)
        {
            foreach(Button n in butList)
            {
                String[] names = n.Tag.ToString().Split(',');
                SQLMan.deleteFromTable(names[0], names[1], names[2], names[3], names[4]);
            }
            GenerateList(true);
        }

        private void GenerateList(bool newList)
        {
            if (newList)
            {
                stackPanel1.Children.Clear();
                stackPanel2.Children.Clear();
                stackPanel3.Children.Clear();
            }
            bool add1, add2, add3;
            add1 = true;
            add2 = add3 = false;
            List<QueryData> listGenerate = SQLMan.getData();
            stackPanel1.Height = stackPanel2.Height = stackPanel3.Height = listGenerate.Count * 200;
            Button newBtn;
            Thickness space;
            foreach (QueryData n in listGenerate)
            {
                newBtn = new Button();
                newBtn.Content = n.company + "\n" + n.position + "\n" + n.reqid;
                newBtn.Name = "Button";
                newBtn.Height = 50;
                newBtn.Width = 195;
                newBtn.Tag = n.company + "," + n.position + "," + n.date + "," + n.reqid + "," + n.other;
                newBtn.Click += posButton_Click;
                space = newBtn.Margin;
                space.Bottom = 5;
                if (add1)
                {
                    stackPanel1.Children.Add(newBtn);
                    newBtn.Margin = space;
                    add1 = false;
                    add2 = true;
                }
                else if (add2)
                {
                    stackPanel2.Children.Add(newBtn);
                    newBtn.Margin = space;
                    add2 = false;
                    add3 = true;
                }
                else if (add3)
                {
                    stackPanel3.Children.Add(newBtn);
                    newBtn.Margin = space;
                    add3 = false;
                    add1 = true;
                }
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            InputForm InForm = new InputForm(SQLMan);
            InForm.Show();
            InForm.Closed += Form_Closed;
        }

        void Form_Closed(object sender, EventArgs e)
        {
            GenerateList(true);
            Console.WriteLine("generated list");
        }

        private void delButton_Click(object sender, RoutedEventArgs e)
        {
            deleteSelected(toBeDeleted);
        }
        private void posButton_Click(object sender, RoutedEventArgs e)
        {
            toBeDeleted.Add(sender as Button);
        }
    }
}
