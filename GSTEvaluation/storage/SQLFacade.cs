using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

namespace GSTEvaluation.storage
{
    class SQLFacade
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(SQLFacade).Name);
        private static readonly string REPOSITORY_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                            "MUTEX", "evaluation");

        private const string SOURCE_TABLE_NAME = "source";
        private const string COMPARISON_TABLE_NAME = "comparison";
        private const string EVAL_RUN_TABLE_NAME = "eval_run";

        private static readonly string DATABASE_PATH = Path.Combine(REPOSITORY_PATH, "eval_db.sqlite3");
        private static SQLFacade cInstance;

        public static SQLFacade Instance
        {
            get { return cInstance ?? (cInstance = new SQLFacade()); }
        }
    
        private readonly SQLiteConnection connection;
        
        private SQLFacade()
        {
            cLogger.DebugFormat("db connection string: {0}", string.Format("Data Source={0}", DATABASE_PATH));

            connection = new SQLiteConnection(string.Format("Data Source={0}", DATABASE_PATH));

            if (!Directory.Exists(REPOSITORY_PATH))
            {
                cLogger.DebugFormat("directory '{0}' does not exist", REPOSITORY_PATH);
                Directory.CreateDirectory(REPOSITORY_PATH);
                SQLiteConnection.CreateFile(DATABASE_PATH);
            }

            connection.Open();
            CreateTables();
        }

        private void CreateTables()
        {
            var command = new SQLiteCommand(connection)
            {
                CommandText = "PRAGMA encoding='UTF-8';"
            };

            command.ExecuteNonQuery();

            var tables = new[]
            {
                "CREATE TABLE IF NOT EXISTS " + SOURCE_TABLE_NAME + " " +
                    "(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "tokens TEXT NOT NULL, name TEXT NOT NULL)",
                "CREATE TABLE IF NOT EXISTS " + COMPARISON_TABLE_NAME + " " + 
                    "(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL, " + 
                    "source_1_id INTEGER NOT NULL, source_2_id INTEGER NOT NULL, " + 
                    "run_id INTEGER NOT NULL, result INTEGER NOT NULL)",
                "CREATE TABLE IF NOT EXISTS " + EVAL_RUN_TABLE_NAME + " " +
                    "(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " + 
                    "datetime TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP)"
            };

            foreach(var table in tables)
            {
                command.CommandText = table;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// creates a new evaluation run and returns the ID
        /// </summary>
        /// <returns></returns>
        public Int64 CreateEvaluationRun()
        {
            var command = new SQLiteCommand(connection)
            {
                CommandText = "INSERT INTO " + EVAL_RUN_TABLE_NAME + " DEFAULT VALUES"
            };

            command.ExecuteNonQuery();

            command.CommandText = "SELECT id FROM " + EVAL_RUN_TABLE_NAME + " ORDER BY id DESC LIMIT 1";

            return (Int64) command.ExecuteScalar();
        }

        /// <summary>
        /// inserts the token stream into the database and returns the ID of the new object
        /// </summary>
        /// <param name="joinedTokenStream">tokens joined by WhiteSpace</param>
        /// <returns></returns>
        public Int64 InsertSource(string joinedTokenStream, string name)
        {
            var command = new SQLiteCommand(connection)
            {
                CommandText = string.Format("INSERT INTO {0}(tokens, name) VALUES('{1}', '{2}')", SOURCE_TABLE_NAME, joinedTokenStream, name)
            };

            //Console.WriteLine("SQL:{0}", command.CommandText);
            command.ExecuteNonQuery();

            command.CommandText = string.Format("SELECT id FROM {0} ORDER BY id DESC LIMIT 1", SOURCE_TABLE_NAME);

            return (Int64)command.ExecuteScalar();
        }

        public Int64 CreateComparison(string name, Int32 result, Int64 evalRunId, Int64 source1ID, Int64 source2ID)
        {
            var command = new SQLiteCommand(connection)
            {
                CommandText = string.Format("INSERT INTO {0}(name, run_id, source_1_id, source_2_id, result) " +
                        "VALUES('{1}', {2}, {3}, {4}, {5})", COMPARISON_TABLE_NAME, name, evalRunId, source1ID, source2ID, result)
            };

            command.ExecuteNonQuery();

            command.CommandText = string.Format("SELECT id FROM {0} ORDER BY id DESC LIMIT 1", COMPARISON_TABLE_NAME);

            return (Int64)command.ExecuteScalar();
        }

        /// <summary>
        /// retrieves all evaluation run ID and comparison results for the given comparison name
        /// </summary>
        /// <param name="comparisonName"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<Int64, Int32>> GetComparisonHistory(string comparisonName)
        {
            var command = new SQLiteCommand(connection)
            {
                CommandText = string.Format("SELECT eval.id, comp.result " + 
                                    "FROM {0} AS comp INNER JOIN {1} AS eval ON comp.run_id = eval.id " + 
                                    "WHERE comp.name='{2}' " + 
                                    "ORDER BY eval.datetime ASC ", COMPARISON_TABLE_NAME, EVAL_RUN_TABLE_NAME, comparisonName)
            };

            var reader = command.ExecuteReader();

            var list = new List<Tuple<Int64, Int32>>();

            while(reader.Read())
            {
                list.Add(new Tuple<Int64, Int32>(
                    reader.GetInt64(reader.GetOrdinal("id")), 
                    reader.GetInt32(reader.GetOrdinal("result"))));
            }


            return list;
        }
    }
}
