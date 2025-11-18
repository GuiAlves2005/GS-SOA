using System.ComponentModel.DataAnnotations;
using HarmonyWork.Models;

namespace HarmonyWork.DTOs
{
    public class TarefaRequestDto
    {
        [Required(ErrorMessage = "O título é obrigatório")]
        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public DateTime DataVencimento { get; set; }

        // O usuário escolhe a energia, mas o ID e Status a gente controla
        public NivelEnergia EnergiaNecessaria { get; set; }
    }
}