using BuscaCep.Data.Dtos;
using BuscaCep.Providers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace BuscaCep.Data
{
    class DatabaseService
    {
        private static Lazy<DatabaseService> _Lazy = new Lazy<DatabaseService>(() => new DatabaseService());

        public static DatabaseService Current { get => _Lazy.Value; }

        private DatabaseService()
        {
            var dbPath = DependencyService.Get<ISQLiteDatabasePathProvider>().GetDatabasePath();

            _SQLiteConnection = new SQLiteConnection(dbPath);
            _SQLiteConnection.CreateTable<CepDto>();
        }

        private readonly SQLiteConnection _SQLiteConnection;

        public bool CepSave(CepDto cep) => _SQLiteConnection.InsertOrReplace(cep) > 0;

        public List<CepDto> CepGetAll() => _SQLiteConnection.Table<CepDto>().ToList();

        public CepDto CepGet(Guid id) => _SQLiteConnection.Find<CepDto>(id);
    }
}
