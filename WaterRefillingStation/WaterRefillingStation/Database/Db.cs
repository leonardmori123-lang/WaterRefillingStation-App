using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using WaterRefillingStation.Security;

namespace WaterRefillingStation.Database
{
    public static class Db
    {
        private const string AppFolderName = "WaterRefillingStation";
        private const string DbFileName = "station.db";
        private const string SchemaResourceSuffix = "water_refilling_schema.sql";

        // First-run login only. Change it after the first login.
        private const string DefaultAdminUsername = "admin";
        private const string DefaultAdminPassword = "admin123";

        public static string DbPath { get; } = BuildDbPath();

        private static string BuildDbPath()
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                AppFolderName);
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, DbFileName);
        }

        private static string ConnectionString
        {
            get
            {
                var builder = new SQLiteConnectionStringBuilder
                {
                    DataSource = DbPath,
                    Version = 3,
                    ForeignKeys = true
                };
                return builder.ToString();
            }
        }

        public static SQLiteConnection GetConnection()
        {
            var conn = new SQLiteConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        // Call once in Program.cs. Safe to run on every launch.
        public static void Initialize()
        {
            using (SQLiteConnection conn = GetConnection())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = ReadSchemaScript();
                    cmd.ExecuteNonQuery();
                }
                EnsureDefaultAdmin(conn);
            }
        }

        private static string ReadSchemaScript()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string resourceName = asm.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(SchemaResourceSuffix, StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
                throw new InvalidOperationException(
                    "water_refilling_schema.sql must have Build Action = Embedded Resource.");

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private static void EnsureDefaultAdmin(SQLiteConnection conn)
        {
            using (SQLiteCommand check = conn.CreateCommand())
            {
                check.CommandText = "SELECT COUNT(*) FROM Users";
                if (Convert.ToInt64(check.ExecuteScalar()) > 0) return;
            }

            using (SQLiteCommand insert = conn.CreateCommand())
            {
                insert.CommandText =
                    "INSERT INTO Users (Username, PasswordHash, FullName, Role) " +
                    "VALUES (@username, @hash, @fullName, 'Admin')";
                insert.Parameters.AddWithValue("@username", DefaultAdminUsername);
                insert.Parameters.AddWithValue("@hash", PasswordHasher.Hash(DefaultAdminPassword));
                insert.Parameters.AddWithValue("@fullName", "Administrator");
                insert.ExecuteNonQuery();
            }
        }

        // Always pass values as parameters, never build SQL by joining strings.
        // Example: Db.Query("SELECT * FROM Customers WHERE CustomerID = @id", ("@id", 5));

        public static int Execute(string sql, params (string Name, object Value)[] parameters)
        {
            using (SQLiteConnection conn = GetConnection())
            using (SQLiteCommand cmd = CreateCommand(conn, sql, parameters))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public static object Scalar(string sql, params (string Name, object Value)[] parameters)
        {
            using (SQLiteConnection conn = GetConnection())
            using (SQLiteCommand cmd = CreateCommand(conn, sql, parameters))
            {
                object result = cmd.ExecuteScalar();
                return result == DBNull.Value ? null : result;
            }
        }

        public static DataTable Query(string sql, params (string Name, object Value)[] parameters)
        {
            using (SQLiteConnection conn = GetConnection())
            using (SQLiteCommand cmd = CreateCommand(conn, sql, parameters))
            using (var adapter = new SQLiteDataAdapter(cmd))
            {
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        // Runs an INSERT and returns the new row's ID.
        public static long Insert(string sql, params (string Name, object Value)[] parameters)
        {
            using (SQLiteConnection conn = GetConnection())
            {
                using (SQLiteCommand cmd = CreateCommand(conn, sql, parameters))
                {
                    cmd.ExecuteNonQuery();
                }
                return conn.LastInsertRowId;
            }
        }

        private static SQLiteCommand CreateCommand(
            SQLiteConnection conn, string sql, (string Name, object Value)[] parameters)
        {
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            foreach (var p in parameters)
                cmd.Parameters.AddWithValue(p.Name, p.Value ?? DBNull.Value);
            return cmd;
        }
    }
}