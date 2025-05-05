using System.Data.SQLite;

namespace Library.Data
{
    public static class Database
    {
        private const string ConnectionString = "Data Source=library.db;Version=3;";

        public static SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
