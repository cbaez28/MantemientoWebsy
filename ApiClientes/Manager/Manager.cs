using ApiClientes.DataModel;
using ApiClientes.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiClientes.Manager
{
    public class ClienteManager : ICliente
    {
        private readonly IGlobal _global;
        private readonly ILogger _logger;
        private readonly DBClientesContext  _db;
        IDbContextTransaction _Trans;

        public ClienteManager(IGlobal global, ILogger<ClienteHelper> logger )
        {
            _global = global;
            _logger = logger;
            _db = (DBClientesContext ) _global._DbContext;
        }
        /// <summary>
        /// Metodo para eliminar un cliente por medio del correo electronico
        /// </summary>
        /// <param name="args">Parametro generico por donde viajan los criterio de busqueda, en este caso correo electronico</param>
        /// <returns></returns>
        public OperationResult<bool> DeleteCleinte(OperationArgs<string> args)
        {
            try
            {

                var ctl = _db.TblClientes.AsNoTracking().SingleOrDefault(x => x.Correo.ToUpper().Trim () == args.param.ToUpper().Trim ());
                if (ctl != null)
                {
                    _Trans = _db.Database.BeginTransaction();
                    _db.TblClientes.Remove(ctl);
                    _db.SaveChanges();
                    _Trans.Commit();
                }
                return new OperationResult<bool>() { _Result = true, DataFound = true, Success =true  };
            }
            catch (Exception ex)
            {
                if (_Trans != null)
                    _Trans.Rollback();

                _logger?.LogError(ex, ex.Message, null);
                throw;
            }
            finally 
            {
                if (_Trans != null) _Trans.Dispose(); _Trans = null;
            }

        }

        /// <summary>
        /// Metodo para consultar un cliente por medio del correo electronico
        /// </summary>
        /// <param name="args">Parametro generico por donde viajan los criterio de busqueda, en este caso correo electronico</param>
        /// <returns></returns>
        public OperationResult <List<DTOCliente>> GetCliente(OperationArgs<string> args)
        {
            try
            {
                var ctl = _db.TblClientes.AsNoTracking().SingleOrDefault(x => x.Correo.ToUpper().Trim () == args.param.ToUpper().Trim ());

                List<DTOCliente> clientes = new List<DTOCliente>();
                if (ctl != null)
                {
                    clientes.Add(new DTOCliente()
                    {
                        Nombre = ctl.Nombre,
                        Apellido = ctl.Apellido,
                        Correo = ctl.Correo
                    });
                }

                return new OperationResult<List<DTOCliente>>()
                {
                    _Result = clientes, 
                    DataFound = ctl != null,
                    Success = true 
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message, null);
                throw;
            }
        }

        /// <summary>
        /// Metodo para consultar listado de cliente 
        /// </summary>
        /// <returns></returns>
        public OperationResult<List<DTOCliente>> GetClientes()
        {
            try
            {
                var ctl = _db.TblClientes.AsNoTracking().ToList();
               
                List<DTOCliente> listClt = new List<DTOCliente>();
                if (ctl != null)
                {
                    foreach (var row in ctl)
                    {
                        DTOCliente clt = new DTOCliente();
                        clt.Nombre = row.Nombre;
                        clt.Apellido = row.Apellido;
                        clt.Correo = row.Correo;

                        listClt.Add(clt);
                    }
                }

                return new OperationResult<List<DTOCliente>>() { _Result = listClt, DataFound = (ctl.Count() > 0), Success = true  };

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message, null);
                throw;
            }
        }

        /// <summary>
        /// Metodo para insertar nuevo cliente
        /// </summary>
        /// <param name="args"> Parametro generico por donde viajan lo el clinte a insertar, 
        /// en este caso una entidad nueva de cliente, Nombre, Apellido, Correo.  
        /// </param>
        /// <returns></returns>
        public OperationResult<bool> PostCliente(OperationArgs<TblCliente> args)
        {
            try
            {
                _Trans = _db.Database.BeginTransaction();
                _db.TblClientes.Add(args.param);
                _db.SaveChanges();
                _Trans.Commit();

                return new OperationResult<bool>() { _Result = true, DataFound = true, Success = true  };
            }
            catch (Exception ex)
            {
                if (_Trans != null)
                    _Trans.Rollback();

                _logger?.LogError(ex, ex.Message, null);
                throw;
            }
            finally 
            {
                if (_Trans != null) _Trans.Dispose(); _Trans = null;
            }
        }

        /// <summary>
        /// Metodo para actualzar los datos de un clinete ya existente
        /// </summary>
        /// <param name="args"> Parametro generico por donde viajan los criterio a actualizar, 
        /// en este caso una entidad nueva de cliente, Nombre, Apellido, Correo.  
        /// </param>
        /// <param name="OldCorreo"> Correo por el cual se identifica el cliente a actualizar</param>
        /// <returns></returns>
        public OperationResult<bool> PutCliente(OperationArgs<TblCliente> args, string OldCorreo)
        {
            try
            {
                _Trans = _db.Database.BeginTransaction();
                var ctl = _db.TblClientes.AsNoTracking().SingleOrDefault(x => x.Correo.ToUpper() == OldCorreo.ToUpper());
                ctl.Nombre = args.param.Nombre;
                ctl.Apellido  = args.param.Apellido ;
                ctl.Correo  = args.param.Correo ;
                _db.Set<TblCliente>().Update(ctl);
                _db.SaveChanges();
                _Trans.Commit();

                return new OperationResult<bool>() { _Result = true, DataFound =true, Success =true  };
            }
            catch (Exception ex)
            {
                if (_Trans != null)
                    _Trans.Rollback();

                _logger?.LogError(ex, ex.Message, null);
                throw;
            }
            finally
            {
                if (_Trans != null) _Trans.Dispose(); _Trans = null;
            }
        }
    }
}
