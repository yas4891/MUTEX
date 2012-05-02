using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataRepository
{
    /// <summary>
    /// provides an interface to the data storage
    /// </summary>
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// stores the data and if the boolean is set it will be kept for 
        /// further plagiarism tests. 
        /// If the second parameter is FALSE, the data will be stored but only for archiving purposes
        /// </summary>
        /// <param name="data"></param>
        /// <param name="keepForTests"></param>
        void Store(SourceEntityData data, bool keepForTests);

        /// <summary>
        /// loads data by exactly matching the assignment identifier. 
        /// This will only look up in the references database.
        /// returned SourceEntityData objects do not necessarily contain the full source
        /// </summary>
        /// <param name="assignment"></param>
        /// <returns>empty enumerable if no results where found</returns>
        IEnumerable<SourceEntityData> LoadByAssignment(string assignment);
    }


    public static class Repository
    {
        public static IRepository GetRepository()
        {
            return new SQLiteRepository();
        }
    }
}
