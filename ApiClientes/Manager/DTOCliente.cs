using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClientes.Manager
{
   public  class DTOCliente
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string OldCorreo { get; set; }
    }
}
