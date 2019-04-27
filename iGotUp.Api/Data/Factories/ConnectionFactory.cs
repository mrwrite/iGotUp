using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace iGotUp.Api.Data.Factories
{
    public class ConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public SqlConnection CreateConnection(string nameOrConnectionString)
        {
            var sqlConnection = new SqlConnection(this.configuration.GetConnectionString(nameOrConnectionString));

            return sqlConnection;
        }
    }
}