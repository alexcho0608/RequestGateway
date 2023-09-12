using DAL.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DBConnection : IDBConnection
    {
        private readonly NpgsqlConnection _connection;

        public DBConnection(NpgsqlConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        ~DBConnection()
        {
            _connection.Close();
        }

        public NpgsqlConnection GetConnection()
        {
            return _connection;
        }


    }
}
