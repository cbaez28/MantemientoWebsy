using System;
using System.Collections.Generic;

#nullable disable

namespace ApiClientes.DataModel
{
    public partial class TblCliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
    }
}
