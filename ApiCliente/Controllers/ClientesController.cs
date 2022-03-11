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
        private IClientes _Repo;
        public ClientesController(IClientes clt)
        {
            _Repo = clt;
        }

        [Route("Buscar/{Correo}")]
        [HttpGet("{Correo}")]
        public IActionResult Get(string Correo)
        {
            var Cliente = _Repo.GetCliente(Correo);
            if (Cliente != null)
            {
                return Ok(Cliente);
            }
            return Ok(false);

        }

        [Route("Buscar")]
        [HttpGet()]
        public IActionResult Get()
        {
            var Cliente = _Repo.GetClientes();
            if (Cliente != null)
            {
                return Ok(Cliente);
            }
            return Ok(false);

        }

        [Route("Insertar")]
        [HttpPost]
        public bool Post([FromBody] DTOCliente DTOclt)
        {
            TblCliente clt = new TblCliente()
            {
                Nombre = DTOclt.Nombre,
                Apellido = DTOclt.Apellido,
                Correo = DTOclt.Correo
            };
            return _Repo.PostCliente(clt);

        }

        [Route("Actulizar")]
        [HttpPut]
        public bool Put([FromBody] DTOCliente DTOclt)
        {
            TblCliente clt = new TblCliente()
            {
                Nombre = DTOclt.Nombre,
                Apellido = DTOclt.Apellido,
                Correo = DTOclt.Correo
            };
            return _Repo.PutCliente(clt, DTOclt.OldCorreo);

        }

        [Route("Eliminar/{Correo}")]
        [HttpDelete("{Correo}")]
        public bool Post(string Correo)
        {
            return _Repo.DeleteCleinte(Correo);

        }

    }
}
