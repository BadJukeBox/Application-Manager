using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace App_Manager
{
    class SQLiteManager
    {
        SQLiteConnection SQLite_conn;
        private void Init()
        {
            if (File.Exists("GroupManager.sqlite"))
            {

            }
            else
            {
                SQLiteConnection.CreateFile("GroupManager.sqlite");
            }
        }
    }
}
