using Challenge17ApiPeliculas.IdentityAuth;
using Challenge17ApiPeliculas.Models;
using Challenge17ApiPeliculas.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticateController> _logger;
        
        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,ILogger<AuthenticateController> logger)
        {
           
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _logger.LogDebug(1, "log inyectado en el controlador de autenticacion");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "El usuario ya existe!" });
            
            //Si no existe creo un nuevo usuario
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            //Entro en el administrador de usuarios y lo creo, enviandole el password
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)//Si falla la creacion por algun motivo , muestro el error interno
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Fallo la creacion de usuario! Porfavor vea los detalles de usuario y vuelva a intentarlo" });
            
            _logger.LogInformation("Se ha registrado un nuevo usuario",DateTime.UtcNow);
            //Si salio todo bien muestro el status y el mensaje todo bien
            return Ok(new Response { Status = "Success", Message = "Usuario creado satisfactoriamente!" });

            
        }

        [HttpPost]
        [EmailEspecifico("@yopmail")]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Este usuario ya existe!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Fallo la creacion de usuario! Porfavor vea los detalles de usuario y vuelva a intentarlo" });

            //Comprobacion de roles:
            //Si no esta creado ese rol , que lo cree
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            //Si no esta creado ese rol , que lo cree
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            //Si existe el rol admin, entonces le asigno ese rol al usuario en este metodo.
            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            _logger.LogInformation("Se ha registrado un nuevo usuario de tipo administrador", DateTime.UtcNow);
            //Retorno nueva respuesta
            return Ok(new Response { Status = "Success", Message = "Usuario Administrador creado satisfactoriamente!" });




        }

        [HttpPost]
        [EmailEspecifico("@yopmail")]
        [Route("register-freeuser")]
        public async Task<IActionResult> RegisterFreeUser([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Este usuario ya existe!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Fallo la creacion de usuario! Porfavor vea los detalles de usuario y vuelva a intentarlo" });

            //Comprobacion de roles:
            //Si no esta creado ese rol , que lo cree
            if (!await roleManager.RoleExistsAsync(UserRoles.FreeUser))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.FreeUser));
            ////Si no esta creado ese rol , que lo cree
            //if (!await roleManager.RoleExistsAsync(UserRoles.FreeUser))
            //    await roleManager.CreateAsync(new IdentityRole(UserRoles.FreeUser));
            //Si existe el rol admin, entonces le asigno ese rol al usuario en este metodo.
            if (await roleManager.RoleExistsAsync(UserRoles.FreeUser))
            {
                await userManager.AddToRoleAsync(user, UserRoles.FreeUser);
            }
            _logger.LogInformation("Se ha registrado un nuevo usuario de tipo gratuito {time}", DateTime.UtcNow);
            //Retorno nueva respuesta
            return Ok(new Response { Status = "Success", Message = "Usuario Gratuito creado satisfactoriamente!" });    
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //le digo al adm de usuarios que busque el usuario por nombre.
            var user = await userManager.FindByNameAsync(model.UserName);
            //Si el usuario es nulo y el password es correcto entonces : 
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                //Obtengo el rol del usuario
                var userRoles = await userManager.GetRolesAsync(user);

                //Con el claim obtengo la informacion de ese usuario:
                var authClaims = new List<Claim>
                {
                    //Inicializo una nueva instancia de claim especificando una clave, y el valor
                new Claim(ClaimTypes.Name, user.UserName),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                //Recorro la lista de roles buscada anteriormente
                foreach (var userRole in userRoles)
                {
                    //En la lista de informacion anterior agrego una nueva instancia de tipo rol asignandole el rol del usuario.
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                //Obtengo la clave secreta encriptada
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],//Issuer:Emisor del Token
                audience: _configuration["Jwt:ValidAudience"],//Audience:Localhost donde se va a usar
                expires: DateTime.Now.AddMinutes(15),
                claims: authClaims,//Informacion del usuario
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                //Si todo va bien retorno un status 200  y el nuevo token con su expiracion.

                _logger.LogInformation("Se ha logeado un usuario {time}", DateTime.UtcNow);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
                
            }
            //Caso contrario a todo lo anterior retorno no autorizado 401.
            return Unauthorized();
        }
    }
}
