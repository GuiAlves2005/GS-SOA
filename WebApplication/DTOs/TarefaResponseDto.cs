using HarmonyWork.Models;

namespace HarmonyWork.DTOs
{
    public class TarefaResponseDto
    {
        public int Id { get; set; }

        // Adicione o = string.Empty; no final
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        public DateTime DataVencimento { get; set; }

        public string EnergiaNecessaria { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}