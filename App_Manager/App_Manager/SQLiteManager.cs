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
        String insertFormat = "insert into tableName (company, position, date, reqid, other) values ('cmptoreplace', 'postoreplace', 'datetoreplace', 'reqidtoreplace', 'othertoreplace')";
        String removeFormat = "delete from tableName where";
        String dropFormat = "drop table tableName;";


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

        /* Inserts a new item into the Table, doesn't check for duplicates. */
        public void insertFromForm(String comp, String pos, String date, String reqid, String other)
        {
            insertFormat = insertFormat.Replace("tableName", currentTable);
            insertFormat = insertFormat.Replace("cmptoreplace", comp);
            insertFormat = insertFormat.Replace("postoreplace", pos);
            insertFormat = insertFormat.Replace("datetoreplace", date);
            insertFormat = insertFormat.Replace("reqidtoreplace", reqid);
            insertFormat = insertFormat.Replace("othertoreplace", other);
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
}
