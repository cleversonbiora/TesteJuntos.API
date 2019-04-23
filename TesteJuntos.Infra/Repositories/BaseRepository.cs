using System;
using System.Data;

namespace TesteJuntos.Infra.Repositories
{
    public class BaseRepository : IDisposable
    {
        internal IDbConnection _conn;
        public BaseRepository()
        {
            _conn = ConnectionFactory.GetTesteJuntosOpenConnection(); //Open the coonection
        }
        public void Dispose()
        {
            _conn.Close();
            GC.SuppressFinalize(this);
        }
    }
}
