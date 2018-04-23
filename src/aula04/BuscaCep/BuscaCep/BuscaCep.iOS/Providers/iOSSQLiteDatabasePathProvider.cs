using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BuscaCep.iOS.Providers;
using BuscaCep.Providers;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSSQLiteDatabasePathProvider))]

namespace BuscaCep.iOS.Providers
{
    sealed class iOSSQLiteDatabasePathProvider : ISQLiteDatabasePathProvider
    {
        public iOSSQLiteDatabasePathProvider()
        {

        }

        public string GetDatabasePath()
        {
            var databaseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library", "Databases");

            if (!Directory.Exists(databaseFolder))
                Directory.CreateDirectory(databaseFolder);

            return Path.Combine(databaseFolder, "BuscaCep.db3");
        }
    }
}