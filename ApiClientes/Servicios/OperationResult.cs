using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClientes.Servicios
{
    public  class OperationResult<t>
    {
        public t _Result { get; set; }
        public bool Success { get; set; }
        public bool DataFound { get; set; }
    }
}
