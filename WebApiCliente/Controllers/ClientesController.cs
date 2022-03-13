using ApiClientes.DataModel;
using ApiClientes.Manager;
using ApiClientes.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCliente.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private ICliente _Clt;
        public ClientesController(ICliente clt)
        {
            _Clt = clt;
        }

        [Route("Buscar/{Correo}")]
        [HttpGet("{Correo}")]
        public IActionResult Get(string Correo)
        {
           
                var args = new OperationArgs<string>();
                args.param = Correo;

                var Cliente = _Clt.GetCliente(args);
                return Ok(Cliente);

        }

        [Route("Buscar")]
        [HttpGet()]
        public IActionResult Get()
        {
            var Cliente = _Clt.GetClientes();
           return Ok(Cliente);

        }

        [Route("Insertar")]
        [HttpPost]
        public IActionResult Post([FromBody] DTOCliente DTOclt)
        {
           var args = new OperationArgs<TblCliente>();
            TblCliente newClt = new TblCliente();
            newClt.Nombre = DTOclt.Nombre;
            newClt.Apellido = DTOclt.Apellido;
            newClt.Correo = DTOclt.Correo;

            args.param = newClt;
            return Ok( _Clt.PostCliente(args));

        }

        [Route("Actulizar")]
        [HttpPut]
        public IActionResult  Put([FromBody] DTOCliente DTOclt)
        {
            var args = new OperationArgs<TblCliente>();
            TblCliente newClt = new TblCliente();
            newClt.Nombre = DTOclt.Nombre;
            newClt.Apellido = DTOclt.Apellido;
            newClt.Correo = DTOclt.Correo;
            args.param = newClt;
            return Ok(_Clt.PutCliente(args, DTOclt.OldCorreo));

        }

        [Route("Eliminar/{Correo}")]
        [HttpDelete("{Correo}")]
        public IActionResult Post(string Correo)
        {
            var args = new OperationArgs<string>();
            args.param = Correo;
            return Ok( _Clt.DeleteCleinte(args));

        }

    }
}
