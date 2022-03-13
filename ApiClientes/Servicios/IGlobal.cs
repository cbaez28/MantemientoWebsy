
using System;
using System.Collections.Generic;
using System.Text;


using Microsoft.EntityFrameworkCore;

namespace ApiClientes.Servicios
{
    public interface IGlobal
    {
        DbContext _DbContext { get; }
    }
}
