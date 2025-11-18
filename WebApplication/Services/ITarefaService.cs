using HarmonyWork.DTOs;

namespace HarmonyWork.Services
{
    public interface ITarefaService
    {
        Task<IEnumerable<TarefaResponseDto>> ObterTodasAsync();
        Task<TarefaResponseDto?> ObterPorIdAsync(int id);
        Task<TarefaResponseDto> CriarTarefaAsync(TarefaRequestDto dto);
        Task<bool> AtualizarTarefaAsync(int id, TarefaRequestDto dto);
        Task<bool> DeletarTarefaAsync(int id);
    }
}