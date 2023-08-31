using DbExtensions;
using MySql.Data.MySqlClient;
using LIB.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.db
{
    public static class DBFactory
    {
        public static readonly string DB_NAME = "database";
        public static readonly string HOST = "localhost";
        public static readonly string PORT = "3306";
        public static readonly string CONNECTION_STRING = loadConfig();

        //$"Server={HOST};Database={DB_NAME};Uid=root;Pwd=xxxx"; //
        public static readonly string MYSQL_PROVIDER = "MySql.Data.MySqlClient";
        public static readonly string SQLCE_PROVIDER = "System.Data.SqlServerCe.4.0";

        public static Database Get()
        {
            Database db = new Database(CONNECTION_STRING, MYSQL_PROVIDER); // , MYSQL_PROVIDER);
            //db.Configuration.Log = Console.Out;
            return db;
        }

        public static Database GetEnsureOpen()
        {
            Database db = new Database(CONNECTION_STRING, MYSQL_PROVIDER); // , MYSQL_PROVIDER);
            //db.Configuration.Log = Console.Out;
            db.EnsureConnectionOpen();
            return db;
        }

        public static Database GetEnsureOpen(string connectionString)
        {   
            //TODO: fail fast throw error
            if (connectionString == null || connectionString.Length <= 5)
                connectionString = CONNECTION_STRING;
            Database db = new Database(connectionString, MYSQL_PROVIDER);
            //db.Configuration.Log = Console.Out;
            db.EnsureConnectionOpen();
            return db;
        }

        public static Database Get(string connectionString)
        {
            //TODO: fail fast throw error
            if (connectionString == null || connectionString.Length <= 5)
                connectionString = CONNECTION_STRING;
            return new Database(connectionString, MYSQL_PROVIDER);
        }

        public static MySqlConnection GetSqlConnection()
        {
            return new MySqlConnection(CONNECTION_STRING);
        }

        private static string loadConfig()
        {
            try
            {
                System.Configuration.ConnectionStringSettings connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["phd_tas_db"];
                //TODO: Don't inicialize if .config not has phd_tas_db connnection
                if (connectionString == null)
                    return $"Server={HOST};Database={DB_NAME};Uid=root;Pwd=phd579";

                return connectionString.ConnectionString;
            }
            catch (Exception ex)
            {
                Logging.Error("ON LoadConnectionString Config E: " + ex.Message);
                return $"Server={HOST};Database={DB_NAME};Uid=root;Pwd=phd579";
            }
        }
    }
}
