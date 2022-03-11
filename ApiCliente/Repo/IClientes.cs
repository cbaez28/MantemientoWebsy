using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCliente
{
    public  interface IClientes
    {
        List<DTOCliente> GetClientes();
        DTOCliente GetCliente(string  Correo);
        bool PostCliente(TblCliente clt);
        bool PutCliente(TblCliente clt, string OldCorreo);
        bool DeleteCleinte(String  Correo);
    }
}
