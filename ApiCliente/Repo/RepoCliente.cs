using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace ApiCliente
{
    public class RepoCliente : IClientes
    {
        private DBClientesContext _Db;
        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans;

        public bool DeleteCleinte(string Correo)
        {
            try
            {
                using (_Db = new DBClientesContext())
                {
                    trans = _Db.Database.BeginTransaction();
                    var Cliente = _Db.TblClientes.Where(x => x.Correo.Trim().ToLower() == Correo.Trim().ToLower()).
                                                  Select(x => new TblCliente()
                                                  {
                                                      Nombre = x.Nombre,
                                                      Apellido = x.Apellido,
                                                      Correo = x.Correo,
                                                      Id= x.Id 
                                                  }).FirstOrDefault();
                    _Db.TblClientes.Remove(Cliente);
                    _Db.SaveChanges();
                    trans.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw new ApplicationException("Error eliminado cliente ", ex);
            }
            finally
            {
                if (trans != null)
                {
                    trans.Dispose();
                    trans = null;
                }

            }
        }

        public DTOCliente GetCliente(string Correo)
        {
            try
            {
                using (_Db = new DBClientesContext())
                {
                    var Cliente = _Db.TblClientes.Where(x => x.Correo.Trim().ToLower() == Correo.Trim().ToLower()).
                                                  Select(x => new DTOCliente()
                                                  {
                                                      Nombre = x.Nombre,
                                                      Apellido = x.Apellido,
                                                      Correo = x.Correo
                                                  }).FirstOrDefault();
                    return Cliente;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error buscar cliente ", ex);
            }
        }

        public List<DTOCliente> GetClientes()
        {
            try
            {
                using (_Db = new DBClientesContext())
                {
                    List<DTOCliente> Clientes = new List<DTOCliente>();
                    var list = _Db.TblClientes.ToList();
                    if (list != null)
                    {
                        int n = 0;
                        while (n <= list.Count - 1)
                        {
                            Clientes.Add(new DTOCliente()
                            {
                                Nombre = list[n].Nombre,
                                Apellido = list[n].Apellido,
                                Correo = list[n].Correo
                            });
                            n++;
                        }
                    }
                    return Clientes;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error buscar clientes ", ex);
            }
        }

        public bool PostCliente(TblCliente clt)
        {
            try
            {
                using (_Db = new DBClientesContext())
                {
                    trans = _Db.Database.BeginTransaction();
                    _Db.TblClientes.Add(new TblCliente()
                    {
                        Nombre = clt.Nombre,
                        Apellido = clt.Apellido,
                        Correo = clt.Correo
                    });
                    _Db.SaveChanges();
                    trans.Commit();

                    return true;
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw new ApplicationException("Error Insertando cliente ", ex);
            }
            finally
            {
                if (trans != null)
                {
                    trans.Dispose();
                    trans = null;
                }

            }
        }

        public bool PutCliente(TblCliente clt, string OldCorreo)
        {
            try
            {
                using (_Db = new DBClientesContext())
                {
                    trans = _Db.Database.BeginTransaction();
                    var Ids = _Db .TblClientes .Where(x => x.Correo.Trim().ToLower() == OldCorreo).
                                                  Select(x => new { x.Id } ).ToList();

                   var Cliente = _Db.TblClientes.Find(Ids[0].Id );

                    if (Cliente != null)
                    {
                        Cliente.Nombre = clt.Nombre;
                        Cliente.Apellido = clt.Apellido;
                        Cliente.Correo = clt.Correo;
                        _Db.SaveChanges();
                    }

                    trans.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw new ApplicationException("Error Editando cliente ", ex);
            }
            finally
            {
                if (trans != null)
                {
                    trans.Dispose();
                    trans = null;
                }

            }
        }
    }
}
