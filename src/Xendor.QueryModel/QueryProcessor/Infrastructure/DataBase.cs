﻿using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Xendor.QueryModel.QueryProcessor.Infrastructure
{
    public class DataBase : IDataBase
    {
        private readonly string _connectionString;
        private readonly DbProviderFactory _dbProviderFactory;
        private DbConnection _dbConnection;
        protected DataBase(string connectionString, DbProviderFactory dbProviderFactory)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connectionString = connectionString;
            _dbProviderFactory = dbProviderFactory ?? throw new ArgumentNullException(nameof(dbProviderFactory));
            Init();
        }

        private void Init()
        {
            _dbConnection = _dbProviderFactory.CreateConnection();
            if (_dbConnection != null)
                _dbConnection.ConnectionString = _connectionString;
            OpenAsync().Wait();
        }
        #region IConnection
        public void Dispose()
        {
            _dbConnection.Dispose();
        }
        public async Task OpenAsync()
        {
            if (_dbConnection.State != ConnectionState.Open)
                await _dbConnection.OpenAsync();

        }

        public DbCommand CreateCommand()
        {
            return _dbConnection.CreateCommand();
        }

        public void Close()
        {
            if (_dbConnection.State == ConnectionState.Open)
                _dbConnection.Close();
        }
        #endregion
    }
}