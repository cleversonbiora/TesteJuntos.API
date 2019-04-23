using TesteJuntos.CrossCutting;
using System.Data.Common;
using System.Data.SqlClient;

namespace TesteJuntos.Infra
{
    public class ConnectionFactory
    {
        public static DbConnection GetTesteJuntosOpenConnection()
        {
            var connection = new SqlConnection(ConnectionStrings.TesteJuntosConnection);

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            return connection;
        }

        public static DbConnection GetTesteJuntosConnection()
        {
            var connection = new SqlConnection(ConnectionStrings.TesteJuntosConnection);
            return connection;
        }
    }
}
