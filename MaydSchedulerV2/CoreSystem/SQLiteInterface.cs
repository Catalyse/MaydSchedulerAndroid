using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MaydSchedulerApp
{
    public static class SQLiteInterface
    {
        private static string sqliteFileName = "PrimaryDB.sq3";

        public static bool createSettingsDB()
        {
            try
            {
                string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var db = new SQLiteConnection(dbPath);
                db.CreateTable<CoreSettingsType>();
                db.Insert(CoreSystem.coreSettings);
                return true;
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        private static bool createEmpDB()
        {
            try
            {
                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                return true;
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        private static bool updateSettingsData(CoreSettingsType data, string path)
        {
            try
            {

                return true;
            }
            catch (SQLiteException ex)
            {

                return false;
            }
        }
    }
}