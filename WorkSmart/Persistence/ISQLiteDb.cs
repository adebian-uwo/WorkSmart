using SQLite;

namespace WorkSmart
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}

