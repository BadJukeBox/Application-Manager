using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace App_Manager
{
    public class QueryData
    {
        public String company;
        public String position;
        public String date;
        public String reqid;
        public String other;
    }

    public class SQLiteManager
    {
        SQLiteConnection sqlite_conn;
        String currentTable;

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
                String tableFormat = "create table if not exists "+ name +" (company varchar(20), position varchar(50), date varchar(10), reqid varchar(15), other varchar(150))";
                SQLiteCommand command = new SQLiteCommand(tableFormat, sqlite_conn);
                command.ExecuteNonQuery();
                Console.WriteLine("Table created Successfully.");
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
            String ListTable = "select * from " + currentTable + ";";
            SQLiteCommand command = new SQLiteCommand(ListTable, sqlite_conn);
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
            String insertFormat = "insert into " + currentTable + " (company, position, date, reqid, other) values ('" + comp +"', '" + pos + "', '" + date + "', '" + reqid + "', '" + other + "')";
            SQLiteCommand command = new SQLiteCommand(insertFormat, sqlite_conn);
            command.ExecuteNonQuery();
            Console.WriteLine("Insert Successful.");
        }

        /* Deletes item from table. */
        public void deleteFromTable(String comp, String pos, String date, String reqid, String other)
        {
            String removeFormat = "delete from " + currentTable + " where company='" + comp + "' and position='" + pos + "' and date= '" + date + "' and reqid='" + reqid + "';";
            SQLiteCommand command = new SQLiteCommand(removeFormat, sqlite_conn);
            command.ExecuteNonQuery();
            Console.WriteLine("Delete Successful.");
        }

        /* "Clears" table, (since we dont have truncate we have to drop and recreate) */
        public void clearTable()
        {
            String dropFormat = "drop table " + currentTable + ";";
            SQLiteCommand command = new SQLiteCommand(dropFormat, sqlite_conn);
            command.ExecuteNonQuery();
            String tableFormat = "create table if not exists " + currentTable + " (company varchar(20), position varchar(50), date varchar(10), reqid varchar(15), other varchar(150))";
            command = new SQLiteCommand(tableFormat, sqlite_conn);
            command.ExecuteNonQuery();
            Console.WriteLine("Clear Successful.");
        }

        public List<String> listTables()
        {
            List<String> tableList = new List<String>();
            String findTables = "select name from sqlite_master where type = 'table'";
            SQLiteCommand command = new SQLiteCommand(findTables, sqlite_conn);
            SQLiteDataReader sqRead = command.ExecuteReader();
            try
            {
                while (sqRead.Read())
                {
                    String res = sqRead.GetString(0);
                    tableList.Add(res);
                }
            }
            finally
            {
                sqRead.Close();
            }
            Console.WriteLine("List created successfully");
            return tableList;
        }

        /* Drops table. Need to make sure that the user wants to perform this action before they do. */
        public void deleteTable(String nameToDelete)
        {
            String dropFormat = "drop table " + nameToDelete + ";";
            SQLiteCommand command = new SQLiteCommand(dropFormat, sqlite_conn);
            command.ExecuteNonQuery();
            Console.WriteLine("Drop Successful.");
        }

        /* Close connection. */
        public void closeConnection()
        {
            sqlite_conn.Close();
        }


    }
     
}
