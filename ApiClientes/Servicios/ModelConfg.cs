using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClientes.Servicios
{
    public  class ModelConfg
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string ConnectionString { get; set; }
    }
}
