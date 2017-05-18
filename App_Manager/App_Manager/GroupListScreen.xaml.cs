using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace App_Manager
{
    public partial class GroupListScreen : Window
    {
        SQLiteManager SQLMan;
        List<Button> toBeDeleted = new List<Button>();
        List<Button> itemList = new List<Button>();
        InputForm inForm;
        MenuBar menu;

        PositionForm posForm;
        int rowPos, colPos;
        public static Boolean IsOpen { get; private set; }

        public GroupListScreen(SQLiteManager SQLman, String currentGroup)
        {
            InitializeComponent();
            SQLMan = SQLman;
            SQLMan.OpenTable(currentGroup);
            initGrid();
            GenerateList();
            menu = new MenuBar(SQLMan, this);
            menu.generateGroups(CopyJobs);
            menu.generateGroups(MoveJobs);
            IsOpen = true;
            this.Closed += new EventHandler(GLSwin_Close);
        }

        private void GLSwin_Close(object sender, EventArgs e)
        {
            IsOpen = false;
        }

        private void initGrid()
        {
            Console.WriteLine("init grid...");
            genCols();
            List<QueryData> listGenerate = SQLMan.getData();
            genRows(listGenerate.Count);
            rowPos = colPos = 1;

        }

        private void genCols()
        {
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(195) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(195) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(195) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70) });
        }

        private void genRows(int numRows)
        {
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            if (((numRows % 3) != 0) || numRows == 0) numRows += 1;
            for (int i = 0; i < numRows; i++)
                mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
        }

        private void deleteSelected(List<Button> toBeDeletedList)
        {
            Console.WriteLine("Deleting...");
            if (toBeDeletedList.Count == 0)
            {
                MessageBox.Show("Nothing selected to delete!");
                return;
            }
            foreach (Button n in toBeDeletedList)
            {
                itemList.Remove(n);
                String[] names = n.Tag.ToString().Split(',');
                SQLMan.deleteFromTable(names[0], names[1], names[2], names[3], names[4]);
                mainGrid.Children.Remove(n);
            }
            GenerateList();
            toBeDeletedList.Clear();
        }

        private void GenerateList()
        {
            Console.WriteLine("Generating List...");
            colPos = 1;
            rowPos = 1;
            if(itemList.Count != 0)
                foreach (Button n in itemList)
                    placeIteminGrid(n);
            else
            {
                List<QueryData> genList = SQLMan.getData();
                foreach (QueryData n in genList)
                    placeIteminGrid(generateButton(n.company, n.position, n.date, n.reqid, n.other));
            }
        }

        private void placeIteminGrid(Button n)
        {
            mainGrid.Children.Remove(n);
            Grid.SetColumn(n, colPos);
            Grid.SetRow(n, rowPos);
            mainGrid.Children.Add(n);
            colPos++;
            if (colPos > 3)
            {
                colPos = 1;
                mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
                rowPos++;
            }
        }
        
        private Button generateButton(String company, String position, String date, String reqid, String other)
        {
            Button newBtn = new Button();
            Grid.SetColumn(newBtn, colPos);
            Grid.SetRow(newBtn, rowPos);
            newBtn.Content = company + "\n" + position + "\n" + reqid; // show first 3 fields are button content.
            newBtn.Name = "Button";

            /* Store all fields for each button without showing. */
            newBtn.Tag = company + "," + position + "," + date + "," + reqid + "," + other;
            newBtn.Click += posButton_Click;
            newBtn.MouseDoubleClick += showPosition;
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
            Button n;
            if (inForm != null)
            {
                n = generateButton(inForm.Company.Text,
                    inForm.Position.Text,
                    inForm.Date_of_App.Text,
                    inForm.Requisition_ID.Text,
                    inForm.Addition_Info.Text);
                itemList.Add(n);
                placeIteminGrid(n);
                inForm = null;
            }
            else if (posForm != null)
            {
                n = generateButton(inForm.Company.Text,
                    inForm.Position.Text,
                    inForm.Date_of_App.Text,
                    inForm.Requisition_ID.Text,
                    inForm.Addition_Info.Text);
                itemList.Add(n);
                placeIteminGrid(n);
                posForm = null;
            }
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

        public void moveItems(String to)
        {
            if (toBeDeleted.Count == 0)
            {
                MessageBox.Show("Nothing selected to be moved!");
                return;
            }
            foreach (Button n in toBeDeleted)
            {
                String[] names = n.Tag.ToString().Split(',');
                SQLMan.deleteFromTable(names[0], names[1], names[2], names[3], names[4]);
                mainGrid.Children.Remove(n);
            }
            GenerateList();
            SQLMan.OpenTable(to);
            foreach (Button n in toBeDeleted)
            {
                String[] names = n.Tag.ToString().Split(',');
                SQLMan.insertFromForm(names[0], names[1], names[2], names[3], names[4]);
            }
            toBeDeleted.Clear();
            GenerateList();
        }

        public void copyItems(String to)
        {
            if (toBeDeleted.Count == 0)
            {
                MessageBox.Show("Nothing selected to be copied!");
                return;
            }
            foreach (Button n in toBeDeleted)
            {
                String[] names = n.Tag.ToString().Split(',');
                SQLMan.OpenTable(to);
                SQLMan.insertFromForm(names[0], names[1], names[2], names[3], names[4]);
            }
        }
    }
}
