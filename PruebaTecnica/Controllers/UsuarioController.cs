using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos;
using PruebaTecnica.Core.DataProviders.Entity.Seguridad;
using PruebaTecnica.Web.Comun;
using PruebaTecnica.Web.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PruebaTecnica.Web.Controllers
{
    //[Route("api/[controller]")]
    //[Route("[controller]")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]

    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _IUsuarioRepository;
        private readonly UserManager<Usuario> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioController(IUsuarioRepository usuarioRepository ,
            UserManager<Usuario> userManager, IJwtFactory jwtFactory,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _IUsuarioRepository = usuarioRepository;
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _passwordHasher = passwordHasher;

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
            if (!ModelState.IsValid)
                return new OkObjectResult(new { Message="error el en formulairo", listaErro=ModelState, Status=StatusCodes.Status500InternalServerError});

            Usuario usuario = new Usuario
            {
                Nombre = crear.Nombre,
                Apellido = crear.Apellido,
                Identificacion = crear.Identificacion,
                TipoDocumentoId = crear.TipoIdentificacionId,
                Email = crear.Email,
                UserName = crear.Nombre + crear.Apellido,
                EmailConfirmed = true,
            };

            var hasedPassword = _passwordHasher.HashPassword(usuario, crear.PasswordHash);
            usuario.PasswordHash = hasedPassword;

            var usuarioNuevo =await _userManager.CreateAsync(usuario);
             
            if (usuarioNuevo.Succeeded)
            {
                return new OkObjectResult(new
                {
                    Message = "Proceso termino exitosamente",
                    Status = StatusCodes.Status200OK
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    Message = "Proceso termino"+ usuarioNuevo.Errors,
                    Status = StatusCodes.Status500InternalServerError
                });
            } 
            

        }

        private async Task CrearClave(UsuarioDto crear, Usuario usuario)
        {
            var user = await _userManager.FindByIdAsync(usuario.Id);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, crear.PasswordHash, crear.PasswordHash);

        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                var rolUsuario = await _userManager.GetRolesAsync(userToVerify);
                //var rolInfo = rolUsuario.FirstOrDefault();
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id, rolUsuario.FirstOrDefault()));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }



        // PUT api/values/5
        [HttpPost]
        public IActionResult Put([FromBody] UsuarioDto editar)
        {
            var obtenerUsuario = _IUsuarioRepository.obtenerUsuarioPoIdentificacion(editar.Identificacion);

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
        [HttpPost]
        public IActionResult Delete([FromBody] UsuarioDto eliminar)
        {
            var obtenerUsuario = _IUsuarioRepository.obtenerUsuarioPoIdentificacion(eliminar.Identificacion);
            if(obtenerUsuario ==null)
                return new OkObjectResult(new { Message = "Usuario No encontrado", Status = StatusCodes.Status404NotFound});

            _IUsuarioRepository.Eliminar(obtenerUsuario);

            return new OkObjectResult(new
            {
                Message = "Proceso termino exitosamente",
                Status = StatusCodes.Status200OK
            });
        }
    }
}
