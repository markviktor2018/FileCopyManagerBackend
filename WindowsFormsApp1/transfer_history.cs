using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class transfer_history_db
    {
        private SQLiteConnection m_dbConnection;
        private string database_path;

        public transfer_history_db()
        {
            database_path = AppDomain.CurrentDomain.BaseDirectory + "\\transfer_history.db";
        }

        public void init()
        {
            if (!File.Exists(this.database_path))
            {
                m_dbConnection = new SQLiteConnection("Data Source=" + database_path + ";Version=3;");
                m_dbConnection.Open();
                string sql = "create table transfer_history (id int, date text, time text, job text, device text, size text, transfer_status text)";

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
            else
            {
                m_dbConnection = new SQLiteConnection("Data Source=" + database_path + ";Version=3;");
                m_dbConnection.Open();

            }
        }

        public void database_close()
        {
            m_dbConnection.Close();
        }

        public void make_query(string sql_query)
        {
            this.init();

            SQLiteCommand command = new SQLiteCommand(sql_query, m_dbConnection);
            command.ExecuteNonQuery();

        }

        public SQLiteDataReader select_query(string sql_query)
        {
            this.init();

            SQLiteDataReader myReader = null;

            using (SQLiteCommand selectCMD = m_dbConnection.CreateCommand())
            {
                selectCMD.CommandText = sql_query;
                selectCMD.CommandType = CommandType.Text;

                myReader = selectCMD.ExecuteReader();
            }

            return myReader;
        }

    }
}
