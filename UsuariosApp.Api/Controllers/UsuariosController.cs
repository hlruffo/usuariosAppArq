using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsuariosApp.Api.Identity.Entities;
using UsuariosApp.API.Identity.Contexts;
using UsuariosApp.API.Models;
using UsuariosApp.API.Services;

namespace UsuariosApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IdentityContext _identityContext;
        private readonly JwtTokenService _jwtTokenService;

        public UsuariosController(UserManager<Usuario> userManager, IdentityContext identityContext, JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _jwtTokenService = jwtTokenService;
        }

        private string msgAcessoNegado => "Acesso negado. Usuário inválido.";

        [HttpPost]
        [Route("autenticar")] //ENDPOINT: api/usuarios/autenticar
        [ProducesResponseType(typeof(AutenticarUsuarioResponseModel), 200)]
        public async Task<IActionResult> Autenticar(AutenticarUsuarioRequestModel model)
        {
            #region Buscar o usuário através do email

            var usuario = await _userManager.FindByEmailAsync(model.Email);
            if (usuario == null)
                return StatusCode(401, new { Message = msgAcessoNegado });

            #endregion

            #region Verificar se a senha está correta

            var isPasswordValid = await _userManager.CheckPasswordAsync(usuario, model.Senha);
            if (!isPasswordValid)
                return StatusCode(401, new { Message = msgAcessoNegado });

            #endregion

            #region Retornar os dados do usuário autenticado

            var response = new AutenticarUsuarioResponseModel
            {
                UsuarioId = usuario.Id,
                Nome = usuario.UserName,
                Email = usuario.Email,
                AccessToken = _jwtTokenService.CreateToken(usuario),
                DataHoraAcesso = DateTime.Now
            };

            return Ok(response);

            #endregion
        }
    }
}



