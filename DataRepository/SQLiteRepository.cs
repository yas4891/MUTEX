using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

namespace DataRepository
{
    /// <summary>
    /// stores the data into a SQLite database
    /// </summary>
    public class SQLiteRepository : IRepository
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(SQLiteRepository).Name);
        private const string SOURCE_TABLE_NAME = "source";

        private const string SQL_CREATE_TABLE =
            "CREATE TABLE IF NOT EXISTS " + SOURCE_TABLE_NAME + " (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
            "student VARCHAR(100) NOT NULL, assignment VARCHAR(100) NOT NULL, source TEXT NOT NULL, tokens TEXT NOT NULL)";

        private const string SQL_INSERT_SET = "INSERT INTO " + SOURCE_TABLE_NAME +
            "(student, assignment, source, tokens) VALUES(@student, @assignment, @sourcecode, @tokens);";

        private const string SQL_SELECT_BY_ASSIGNMENT = "SELECT student, assignment, tokens FROM " + SOURCE_TABLE_NAME + " WHERE assignment=@assignment;";

        /// <summary>
        /// path to the repository directory
        /// </summary>
        private static readonly string REPOSITORY_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                            "MUTEX", "repository");

        /// <summary>
        /// full path to the reference DB file
        /// </summary>
        private static readonly string REFERENCE_DB_PATH = Path.Combine(REPOSITORY_PATH, "reference.sqlite3");

        /// <summary>
        /// full path to the archive DB file
        /// </summary>
        private static readonly string ARCHIVE_DB_PATH = Path.Combine(REPOSITORY_PATH, "archive.sqlite3");

        /// <summary>
        /// SQLiteConnection to the reference database
        /// </summary>
        private readonly SQLiteConnection referenceConnection;

        /// <summary>
        /// SQLiteConnection to the archive database
        /// </summary>
        private readonly SQLiteConnection archiveConnection;

        /// <summary>
        /// creates the database file and the table if they don't exist
        /// </summary>
        public SQLiteRepository()
        {
            cLogger.DebugFormat("reference db connection string: {0}", string.Format("Data Source={0}", REFERENCE_DB_PATH));

            referenceConnection = new SQLiteConnection(string.Format("Data Source={0}", REFERENCE_DB_PATH));
            archiveConnection = new SQLiteConnection(string.Format("Data Source={0}", ARCHIVE_DB_PATH));
            cLogger.Debug("SQLiteRepository.ctor()");
            if (!Directory.Exists(REPOSITORY_PATH))
            {
                cLogger.DebugFormat("directory '{0}' does not exist", REPOSITORY_PATH);
                Directory.CreateDirectory(REPOSITORY_PATH);
                SQLiteConnection.CreateFile(REFERENCE_DB_PATH);
                SQLiteConnection.CreateFile(ARCHIVE_DB_PATH);
            }

            referenceConnection.Open();
            archiveConnection.Open();

            CreateTables(referenceConnection);
            CreateTables(archiveConnection);
        }

        /// <summary>
        /// creates the tables
        /// </summary>
        /// <param name="sqLiteConnection"></param>
        private static void CreateTables(SQLiteConnection sqLiteConnection)
        {
            var command = new SQLiteCommand(sqLiteConnection)
            {
                CommandText = "PRAGMA encoding='UTF-8';"
            };

            command.ExecuteNonQuery();

            command.CommandText = SQL_CREATE_TABLE;
            command.ExecuteNonQuery();
        }


        public void Store(SourceEntityData data, bool keepForTests)
        {
            var command = new SQLiteCommand(keepForTests ? referenceConnection : archiveConnection)
            {
                CommandText = SQL_INSERT_SET,
                CommandType = CommandType.Text,
                Parameters =
                    {
                        new SQLiteParameter("@student", data.StudentIdentifier),
                        new SQLiteParameter("@assignment", data.AssignmentIdentifier),
                        new SQLiteParameter("@sourcecode", data.RawSource),
                        new SQLiteParameter("@tokens", SerializeTokens(data.Tokens))
                    }
            };

            cLogger.DebugFormat("storing '{0}' by '{1}'", data.AssignmentIdentifier, data.StudentIdentifier);
            cLogger.DebugFormat("tokens stored: {0}", SerializeTokens(data.Tokens));
            command.ExecuteNonQuery();
        }

        public IEnumerable<SourceEntityData> LoadByAssignment(string assignment)
        {
            var command = new SQLiteCommand
            {
                Connection = referenceConnection,
                CommandType = CommandType.Text,
                CommandText = SQL_SELECT_BY_ASSIGNMENT,
                Parameters =
                    {
                        new SQLiteParameter("@assignment", assignment)
                    }
            };

            var list = new List<SourceEntityData>();
            using(var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    list.Add(new SourceEntityData(
                        reader.GetString(reader.GetOrdinal("student")),
                        assignment,
                        reader.GetString(reader.GetOrdinal("tokens")).Split(new[] {' '}),
                        string.Empty));
                }
            }

            cLogger.DebugFormat("loaded {0} data sets for assignment {1}", list.Count, assignment);
            return list;
        }

        /// <summary>
        /// concatenates tokens with a single whitespace in between tokens
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private string SerializeTokens(IEnumerable<string> tokens)
        {
            var builder = new StringBuilder();

            foreach (var element in tokens)
                builder.AppendFormat("{0} ", element);

            return builder.ToString().Trim();
        }

        public void Dispose()
        {
            referenceConnection.Dispose();
            archiveConnection.Dispose();
        }
    }
}
