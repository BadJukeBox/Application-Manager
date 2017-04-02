using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace App_Manager
{
    /// <summary>
    /// Interaction logic for GroupListScreen.xaml
    /// </summary>
    public partial class GroupListScreen : Window
    {
        SQLiteManager SQLMan;
        List<Button> toBeDeleted = new List<Button>();
        List<Button> itemList = new List<Button>();

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
                itemList.Remove(n);
                String[] names = n.Tag.ToString().Split(',');
                SQLMan.deleteFromTable(names[0], names[1], names[2], names[3], names[4]);
            }
            GenerateList(true);
            butList.Clear();
            foreach (Button n in itemList)
                Console.WriteLine(n.Tag);
        }

        private void GenerateList(bool newList)
        {
            int colWidth = 200;
            /* Clear panels so that we can regerate the list. */
            if (newList)
            {
                stackPanel1.Children.Clear();
                stackPanel2.Children.Clear();
                stackPanel3.Children.Clear();
            }
            /* QueryData class found in SQLiteManager.cs */
            List<QueryData> listGenerate = SQLMan.getData();
            stackPanel1.Height = stackPanel2.Height = stackPanel3.Height = listGenerate.Count * colWidth;
            Button newBtn;
            //foreach (QueryData n in listGenerate)
            for(int i = 0; i < listGenerate.Count; i++)
            {
               newBtn = generateButton(listGenerate[i].company, listGenerate[i].position, listGenerate[i].date, listGenerate[i].reqid, listGenerate[i].other);
               itemList.Add(newBtn);
               if (i % 3 == 0)
                   stackPanel1.Children.Add(newBtn);
               else if (i % 3 == 1)
                   stackPanel2.Children.Add(newBtn);
               else if (i % 3 == 2)
                   stackPanel3.Children.Add(newBtn);
            }
        }

        private Button generateButton(String company, String position, String date, String reqid, String other)
        {
            int btnHeight = 50;
            int btnWidth = 195;
            int btnMargin = 5;
            Thickness space;
            Button newBtn = new Button();
            newBtn.Content = company + "\n" + position + "\n" + reqid; // show first 3 fields are button content.
            newBtn.Name = "Button";
            newBtn.Height = btnHeight;
            newBtn.Width = btnWidth;
            /* Store all fields for each button without showing. */
            newBtn.Tag = company + "," + position + "," + date + "," + reqid + "," + other;
            newBtn.Click += posButton_Click;
            newBtn.MouseDoubleClick += showPosition;
            space = newBtn.Margin;
            space.Bottom = btnMargin;
            newBtn.Margin = space;
            return newBtn;
        }

        private void SortList(List<Button> items)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            InputForm InForm = new InputForm(SQLMan);
            InForm.Show();
            InForm.Closed += Form_Closed;
        }

        private void Form_Closed(object sender, EventArgs e)
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
        
        private void showPosition(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            String[] names = btn.Tag.ToString().Split(',');
            PositionForm posForm = new PositionForm(names[0], names[1], names[2], names[3], names[4], SQLMan);
            posForm.Show();
            posForm.Closed += Form_Closed;
            e.Handled = true;
        }


    }
}
