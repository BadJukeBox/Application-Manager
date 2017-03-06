using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace App_Manager
{
    public class SQLiteManager
    {
        SQLiteConnection sqlite_conn;
        SQLiteCommand command;
        String currentTable = "";
        String tableFormat = "create table if not exists newTable (company varchar(20), position varchar(50), date varchar(10), reqid varchar(15), other varchar(150))";
        String insertFormat;
        String removeFormat = "delete from tableName where";
        String dropFormat = "drop table tableName;";
        String ListTable;


        /* Attempts to open connection to general database. If one doesn't exist, creates a new one. */
        public void Init()
        {
            if (!File.Exists("GroupManager.sqlite"))
            {
                SQLiteConnection.CreateFile("GroupManager.sqlite");
                Console.WriteLine("Init Create Successful.");
            }
            sqlite_conn = new SQLiteConnection("Data Source = GroupManager.sqlite; Version = 3;");
            sqlite_conn.Open(); //REMEMBER TO CLOSE 
            Console.WriteLine("Init Successful.");
            Console.WriteLine(sqlite_conn);
             
        }

        /* Creates a table if none exists. If one exists, does nothing */
        //Should probably add functionality to check if one exists already
        public void CreateTable(String name)
        {
                tableFormat = tableFormat.Replace("newTable", name);
                command = new SQLiteCommand(tableFormat, sqlite_conn);
                command.ExecuteNonQuery();
                Console.WriteLine("Connected Successfully.");
        }

        //Sets the table being worked with in the format Strings
        public void OpenTable(String name)
        {
            currentTable = name;
            Console.WriteLine("Open Success Successful.");
        }

        public List<QueryData> getData()
        {
            List<QueryData> data = new List<QueryData>();
            QueryData entry;
            ListTable = "select * from " + currentTable + ";";
            command = new SQLiteCommand(ListTable, sqlite_conn);
            SQLiteDataReader sqRead = command.ExecuteReader();
            try
            {
                while (sqRead.Read())
                {
                    entry = new QueryData();
                    entry.company = sqRead.GetString(0);
                    entry.position = sqRead.GetString(1);
                    entry.date = sqRead.GetString(2);
                    entry.reqid = sqRead.GetString(3);
                    entry.other = sqRead.GetString(4);
                    data.Add(entry);
                }
            }
            finally
            {
                sqRead.Close();
            }
            return data;
        }

        /* Inserts a new item into the Table, doesn't check for duplicates. */
        public void insertFromForm(String comp, String pos, String date, String reqid, String other)
        {
            insertFormat = "insert into " + currentTable + " (company, position, date, reqid, other) values ('" + comp +"', '" + pos + "', '" + date + "', '" + reqid + "', '" + other + "')";
            command = new SQLiteCommand(insertFormat, sqlite_conn);
            command.ExecuteNonQuery();
            Console.WriteLine("Insert Successful.");
        }

        /* Deletes item from table. */
        public void deleteFromTable()
        {

        }

        /* Drops table. Need to make sure that the user wants to perform this action before they do. */
        public void deleteTable(String nameToDelete)
        {
            dropFormat = dropFormat.Replace("tableName", nameToDelete);
            command = new SQLiteCommand(dropFormat, sqlite_conn);
            command.ExecuteNonQuery();
            Console.WriteLine("Drop Successful.");
        }

        /* Close connection. */
        public void closeConnection()
        {
            sqlite_conn.Close();
        }


    }
     public class QueryData
    {
        public String company;
        public String position;
        public String date;
        public String reqid;
        public String other;
    }
}
