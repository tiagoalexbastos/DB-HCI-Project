using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace GeneProt
{
    class DBconnection
    {
        private static SqlConnection con;
        static DBconnection()
        {
            
        }
        public static SqlConnection getConnection()
        {
            
            con = new SqlConnection("Data Source=T-ASUS;Initial Catalog=GeneProt;Integrated Security=true");
            //con = new SqlConnection("Data Source=tcp:193.136.175.33\\SQLSERVER2012,8293;User Id=p3g2; Password=pedrotiago");
            return con;
        }

    }
}

