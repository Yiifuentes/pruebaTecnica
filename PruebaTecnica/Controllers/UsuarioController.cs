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
        public async Task<IActionResult>Post([FromBody] UsuarioDto crear)
        {   
            Usuario usuario = new Usuario
            {
                Nombre = crear.Nombre,
                Apellido = crear.Apellido,
                Identificacion = crear.Identificacion,
                TipoDocumentoId = crear.TipoIdentificacionId,
                Email = crear.Email
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

            return new OkObjectResult(new {
                Message = "Proceso termino exitosamente",
                Status = StatusCodes.Status200OK
            });

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UsuarioDto editar)
        {
            var obtenerUsuario = _IUsuarioRepository.Obtener(id);

            if (obtenerUsuario  == null)
                return new OkObjectResult(new { Message = "Ussuario no existe", Status = StatusCodes.Status404NotFound });


            obtenerUsuario.Nombre = editar.Nombre;
            obtenerUsuario.Apellido = editar.Apellido;
            obtenerUsuario.Email = editar.Email;

            _IUsuarioRepository.Actualizar(obtenerUsuario);

            return new OkObjectResult(new
            {
                Message = "Proceso termino exitosamente",
                Status = StatusCodes.Status200OK
            });
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _IUsuarioRepository.Eliminar(id);

            return new OkObjectResult(new
            {
                Message = "Proceso termino exitosamente",
                Status = StatusCodes.Status200OK
            });
        }
    }
}
