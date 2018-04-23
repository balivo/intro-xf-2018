using System;
using System.IO;
using BuscaCep.Droid.Providers;
using BuscaCep.Providers;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidSQLiteDatabasePathProvider))]

namespace BuscaCep.Droid.Providers
{
    sealed class DroidSQLiteDatabasePathProvider : ISQLiteDatabasePathProvider
    {
        public DroidSQLiteDatabasePathProvider()
        {

        }

        //public string GetDatabasePath()
        //{
        //    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "BuscaCep.db3");
        //}

        public string GetDatabasePath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "BuscaCep.db3");
    }
}