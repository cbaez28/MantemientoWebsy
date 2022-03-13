using System;
using System.Collections.Generic;
using System.Text;


using ApiClientes.DataModel;
using Microsoft.EntityFrameworkCore;

namespace ApiClientes.Servicios
{
    public class Global : IGlobal
    {
        DBClientesContext _db;
        public Global(DBClientesContext db)
        {
            _db = db;
        }
        public DbContext _DbContext { get { return _db; } }
    }
}
