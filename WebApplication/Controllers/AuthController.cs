using Asp.Versioning;
using HarmonyWork.DTOs;
using HarmonyWork.Models;
using HarmonyWork.Services;
using Microsoft.AspNetCore.Mvc;

namespace HarmonyWork.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Endpoint de Registro (Não autenticado)
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Usuario>> Register(LoginDto dto)
        {
            // Em projetos reais, faríamos hashing da senha aqui.
            var novoUsuario = new Usuario
            {
                Email = dto.Email,
                SenhaHash = dto.Senha, // Usamos SenhaHash como campo para a senha simples
                Nome = dto.Email, // Usando email como nome por simplicidade
                Perfil = "Admin" // Primeiro usuário é Admin, garantindo a prova de Autorização
            };

            var usuarioCriado = await _authService.RegistrarAsync(novoUsuario);

            // Retorna o usuário, mas esconde a senha hash
            usuarioCriado.SenhaHash = "[REMOVIDO]";
            return CreatedAtAction(nameof(Register), usuarioCriado);
        }

        // Endpoint de Login (Não autenticado)
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> Login(LoginDto dto)
        {
            // O AuthService tenta encontrar o usuário e gera o token
            var token = await _authService.LoginAsync(dto);

            if (token == null)
            {
                // Resposta padronizada para falha de login
                return Unauthorized("Credenciais inválidas.");
            }

            // Retorna o token como texto puro (Stateless Session)
            return Ok(token);
        }
    }
}