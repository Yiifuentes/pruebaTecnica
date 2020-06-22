using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos;
using PruebaTecnica.Core.DataProviders.Entity.Seguridad;
using PruebaTecnica.Web.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PruebaTecnica.Web.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
    [Produces("application/json")]
    //[Route("api/[controller]/[action]")]

    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _IUsuarioRepository;
        private readonly UserManager<Usuario> _userManager;


        public UsuarioController(IUsuarioRepository usuarioRepository , UserManager<Usuario> userManager)
        {
            _IUsuarioRepository = usuarioRepository;
            _userManager = userManager;

        }

        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet]
        public IActionResult Get()
        {
            Usuario usuario = new Usuario
            {
                Nombre = "Fidel",
                Apellido = "Fuentes",
                Identificacion = 10001,
                TipoDocumentoId = 1,
                Email = "fuentesl@hotmail.com"
            };
            try
            {
             _IUsuarioRepository.Crear(usuario);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new
                {
                    Message = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }

            IList<Usuario> lista =_IUsuarioRepository.ObtenerLista();
            lista.Add(new Usuario { Nombre = "Maria", Apellido = "Sanchez" });
            List<UsuarioDto> listaUsuario = new List<UsuarioDto>(); 
            foreach (var item in lista)
            {
                listaUsuario.Add(new UsuarioDto { Apellido = item.Apellido, Nombre = item.Nombre, Email=item.Email,Identificacion=item.Identificacion });
            }
            return new OkObjectResult(new
            {
                Message = "Proceso termino exitosamente",
                listaUsuario,
                Status = StatusCodes.Status200OK
            });
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult>Post([FromBody]string value)
        {
            var usuario = new Usuario
            {
                Nombre = "Fidel",
                Apellido = "Fuentes",
                Identificacion = 10001,
                TipoDocumentoId = 1,
                Email = "fuentesl@hotmail.com"
            };
            //var crear = await _userManager.CreateAsync(usuario);

            return new OkObjectResult(new {
                Message = "Proceso termino exitosamente",
                Status = StatusCodes.Status200OK
            });

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
