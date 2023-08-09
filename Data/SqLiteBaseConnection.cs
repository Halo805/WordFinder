using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordFinder.Data
{
    public class SqLiteBaseConnection
    {
        string connectionString;
        SQLiteConnection connection;
        public SqLiteBaseConnection()
        {
            connectionString = string.Format("Data Source=:memory:;Version=3;New=True;");
            CreateTables();
        }


        

        private void OpenConnection()
        {
            connection = new SQLiteConnection(connectionString);
            connection.Open();
        }

        private void CloseConnection()
        {
            connection.Close();
        }
        private void CreateTables()
        {
            string query;

            //Drop Tables we need to modify them:
            DropTable();

            OpenConnection();

            query = "CREATE TABLE IF NOT EXISTS Items (id_item INTEGER PRIMARY KEY, code NVARCHAR(20), description NVARCHAR(250), unti_price DECIMAL, barcode NVARCHAR(20))";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }

            query = "CREATE TABLE IF NOT EXISTS Colors (id_color INTEGER PRIMARY KEY, code NVARCHAR(20), description NVARCHAR(250), rgb_value NVARCHAR(50), hex_value NVARCHAR(50))";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        private void DropTable()
        {
            string query;

            OpenConnection();

            query = "DROP TABLE IF EXISTS MatrixSetting";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }

            query = "DROP TABLE IF EXISTS MatrixDetail";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }


            CloseConnection();
        }
    }
}
