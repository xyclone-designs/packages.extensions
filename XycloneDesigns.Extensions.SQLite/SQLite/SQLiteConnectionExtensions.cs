using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SQLite
{
	public static class SQLiteConnectionExtensions
	{
		public static void CommitAndClose(this SQLiteConnection sqliteconnection) 
		{
			sqliteconnection.Commit();
			sqliteconnection.Close();
		}

		public static T InsertAndReturn<T>(this SQLiteConnection sqliteconnection, T obj) where T : new()
		{
			sqliteconnection.Insert(obj);
			sqliteconnection.Commit();

			return sqliteconnection.Table<T>().Last();
		}
		public static IEnumerable<T> InsertAllAndReturn<T>(this SQLiteConnection sqliteconnection, IEnumerable<T> objs) where T : new()
		{
			int num = sqliteconnection.InsertAll(objs);
			sqliteconnection.Commit();

			return sqliteconnection.Table<T>().TakeLast(num);
		}

		public static IEnumerable<T> TableEnumerable<T>(this SQLiteConnection sqliteconnection) where T : new()
		{
			return sqliteconnection.Table<T>().AsEnumerable();
		}

		public static SQLiteConnection Insert(this SQLiteConnection sqliteconnection, object obj, out int rowsdded)
		{
			rowsdded = sqliteconnection.Insert(obj);

			return sqliteconnection;
		}
		public static SQLiteConnection Update(this SQLiteConnection sqliteconnection, object obj, out int rowsdded)
		{
			rowsdded = sqliteconnection.Update(obj);

			return sqliteconnection;
		}
		public static SQLiteConnection InsertAll(this SQLiteConnection sqliteconnection, IEnumerable objects, out int rowsdded)
		{
			rowsdded = sqliteconnection.InsertAll(objects);

			return sqliteconnection;
		}
		public static SQLiteConnection UpdateAll(this SQLiteConnection sqliteconnection, IEnumerable objects, out int rowsdded)
		{
			rowsdded = sqliteconnection.UpdateAll(objects);

			return sqliteconnection;
		}
	}
}
