using HarmonyWork.DTOs;
using HarmonyWork.Models;

namespace HarmonyWork.Services
{
    public interface IAuthService
    {
        // Tenta logar e retorna o Token (string) se der certo, ou null se falhar
        Task<string?> LoginAsync(LoginDto dto);

        // Registra um novo usuário
        Task<Usuario> RegistrarAsync(Usuario usuario);
    }
}