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
        InputForm inForm;
        PositionForm posForm;

        public GroupListScreen(SQLiteManager SQLman, String currentGroup)
        {
            InitializeComponent();
            SQLMan = SQLman;
            SQLMan.OpenTable(currentGroup);
            GenerateList(true, true, itemList);
        }

        private void deleteSelected(List<Button> toBeDeletedList)
        {
            Console.WriteLine("Deleting...");
            foreach(Button n in toBeDeletedList)
            {
                itemList.Remove(n);
                String[] names = n.Tag.ToString().Split(',');
                SQLMan.deleteFromTable(names[0], names[1], names[2], names[3], names[4]);
            }
            GenerateList(true, false, itemList);
            toBeDeletedList.Clear();
        }

        private void GenerateList(bool regenerateList, bool newList, List<Button> btnList)
        {
            int colWidth = 200;
            /* Clear panels so that we can regerate the list. */
            if (newList)
            {
                List<QueryData> listGenerate = SQLMan.getData();
                /* QueryData class found in SQLiteManager.cs */
                btnList.Clear();
                foreach (QueryData data in listGenerate)
                    btnList.Add(generateButton(data.company, data.position, data.date, data.reqid, data.other));
            }
            if (regenerateList)
            {
                stackPanel1.Children.Clear();
                stackPanel2.Children.Clear();
                stackPanel3.Children.Clear();
            }
            stackPanel1.Height = stackPanel2.Height = stackPanel3.Height = btnList.Count * colWidth;
            for(int i = 0; i < btnList.Count; i++)
            {
               if (i % 3 == 0)
                   stackPanel1.Children.Add(btnList[i]);
               else if (i % 3 == 1)
                   stackPanel2.Children.Add(btnList[i]);
               else if (i % 3 == 2)
                   stackPanel3.Children.Add(btnList[i]);
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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            inForm = new InputForm(SQLMan);
            inForm.Show();
            inForm.Closed += Form_Closed;
        }

        private void Form_Closed(object sender, EventArgs e)
        {
            if(inForm != null)
            {
                itemList.Add(generateButton(inForm.Company.Text, 
                    inForm.Position.Text, 
                    inForm.Date_of_App.Text, 
                    inForm.Requisition_ID.Text, 
                    inForm.Addition_Info.Text));
                inForm = null;
            }
            else if(posForm != null)
            {
                itemList.Add(generateButton(posForm.Company.Text,
                    posForm.Position.Text,
                    posForm.Date_of_App.Text,
                    posForm.Requisition_ID.Text,
                    posForm.Addition_Info.Text));
                posForm = null;
                
            }
            //newList is true because posForm doesn't automatically remove the item from the list so It has to be "remade"
            GenerateList(true, true, itemList);
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
            posForm = new PositionForm(names[0], names[1], names[2], names[3], names[4], SQLMan);
            posForm.Show();
            posForm.Closed += Form_Closed;
            e.Handled = true;
        }


    }
}
