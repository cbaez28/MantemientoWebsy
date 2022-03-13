using ApiClientes.DataModel;
using ApiClientes.Manager;
using ApiClientes.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace ApiClientes
{
    public interface IClienteHelper
    {
        string GetUrl(string mnt, string mtd);
        public OperationResult<t> Invoke<t>(string url, string mtd);
        public OperationResult<t> Invoke<t>(string url, string mtd, DTOCliente clt);

    }
    public class ClienteHelper: IClienteHelper
    {
        private readonly ILogger _logger;
        private readonly IGlobal _global;
        private DBClientesContext _db;

        public ClienteHelper(ILogger<ClienteHelper> logger, IGlobal global  )
        {
            _logger = logger;
            _global = global;
            _db = (DBClientesContext)_global._DbContext;
        }

        /// <summary>
        /// Optiene URL de la operacion a realizar 
        /// </summary>
        /// <param name="mnt">Mantenimiento al que esta relacionado la URL</param>
        /// <param name="mtd">Operacion a ejecutar (GET,POST,PUT,DELETE)</param>
        /// <returns></returns>
        public string GetUrl(string mnt, string mtd)
        {
            try
            {
              var _Url = _db.TblApiConfigs.AsNoTracking().SingleOrDefault(x => x.Mantenimiento .ToUpper () == mnt.ToUpper () && x.Metodo.ToUpper() == mtd.ToUpper ());
                return _Url.Url.ToString().Trim();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message, null);
                throw;
            }
        }

        /// <summary>
        /// Consume WEB API
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="url">URL del metodo a inovar</param>
        /// <param name="mtd">Operacion a realizar (GET,POST,PUT,DELETE)</param>
        /// <returns></returns>
        public OperationResult<t> Invoke<t>(string url, string mtd)
        {
          return   Invoke<t>(url, mtd, null);
        }
        public OperationResult<t> Invoke<t>(string url, string mtd, DTOCliente clt)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = mtd;
                request.ContentType = "application/json";       
                WebResponse response = null;
                if (clt != null)
                {
                    var js = JsonConvert.SerializeObject(clt);
                    using (var sw = new StreamWriter(request.GetRequestStream()))
                    {
                        sw.Write(js);
                        sw.Flush();
                        sw.Dispose();
                    }
                }
                
                response = request.GetResponse();
                OperationResult<t> OperationResult;
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var result = sr.ReadToEnd().Trim();
                    OperationResult = JsonConvert.DeserializeObject<OperationResult<t>>(result);
                    
                }
                return OperationResult;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message, null);
                throw;
            }
            
        }
    }
}
