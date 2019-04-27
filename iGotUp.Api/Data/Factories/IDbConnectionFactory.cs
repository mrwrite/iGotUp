using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iGotUp.Api.Data.Factories
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateConnection(string nameOrConnectionString);
    }
}