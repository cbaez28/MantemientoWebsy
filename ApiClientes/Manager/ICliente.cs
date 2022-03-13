using System;
using System.Collections.Generic;
using System.Text;

using ApiClientes.Servicios;
using ApiClientes.DataModel;
namespace ApiClientes.Manager
{
    public interface ICliente
    {
        OperationResult <List<DTOCliente>> GetClientes();
        OperationResult<List<DTOCliente>> GetCliente(OperationArgs <string> args);
        OperationResult <bool> PostCliente(OperationArgs<TblCliente> args);
        OperationResult <bool> PutCliente(OperationArgs <TblCliente> args, string OldCorreo);
        OperationResult <bool> DeleteCleinte(OperationArgs<String> args);
    }
}
