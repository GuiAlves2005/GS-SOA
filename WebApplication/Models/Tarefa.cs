using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarmonyWork.Models
{
    [Table("Tarefas")]
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [MaxLength(100)]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Descricao { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime DataVencimento { get; set; }

        // --- Atributos do HarmonyWork ---

        // Define se a tarefa exige muita ou pouca energia mental
        [Required]
        public NivelEnergia EnergiaNecessaria { get; set; }

        [Required]
        public StatusTarefa Status { get; set; }
    }

    // Enums para facilitar a categorização
    public enum NivelEnergia
    {
        Baixa,
        Media,
        Alta
    }

    public enum StatusTarefa
    {
        Pendente,
        EmProgresso,
        Concluida,
        Cancelada
    }
}