using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos;
using PruebaTecnica.Core.DataProviders.Entity.Seguridad;
using PruebaTecnica.Web.Comun;
using PruebaTecnica.Web.Dto;

 
namespace PruebaTecnica.Web.Controllers
{ 
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]

    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _IUsuarioRepository;
        private readonly UserManager<Usuario> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly JwtIssuerOptions _jwtOptions;

        public UsuarioController(IUsuarioRepository usuarioRepository ,
            UserManager<Usuario> userManager, 
            IPasswordHasher<Usuario> passwordHasher,
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            _IUsuarioRepository = usuarioRepository;
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _passwordHasher = passwordHasher;
            _jwtOptions = jwtOptions.Value;

        }
 
        [HttpGet]
        public IActionResult Get()
        { 
            IList<Usuario> lista =_IUsuarioRepository.ObtenerLista(); 
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
                EmailConfirmed = true             };
 
            string clave = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(crear.PasswordHash))).Replace("-", "");
            usuario.PasswordHash = clave;
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

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]CredencialDto credentials)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ClaimsIdentity identity = await GetClaimsIdentity(credentials.UserName,
                                                   credentials.Password);
            if (identity == null)
            {
                return BadRequest(("login_failure", " Invalido Usuario o Password", ModelState));
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName,
                _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }
        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

             string clave = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(password))).Replace("-", "");

             if (userToVerify.PasswordHash== clave)
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id, "notiene"));
            }

             return await Task.FromResult<ClaimsIdentity>(null);
        }




    }
}
