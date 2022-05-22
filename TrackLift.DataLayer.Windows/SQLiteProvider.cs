using SQLite;
using System.Diagnostics;

namespace TrackLift.DataLayer.Windows
{
    internal abstract class SQLiteProvider
    {

        private static readonly string dbName = "trackLift.db";
        private static readonly string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
        private static readonly SQLiteConnection database = new SQLiteConnection(dbPath);

        public static SQLiteConnection Database { get => database; }
        public static bool DoesTableExist(String name)
        {
            var result = false;

            try
            {
                var list = Database.Query<object>($"SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{name}'");
                result = (list.Count == 1);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            return false;
        }

    }
}
