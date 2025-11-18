using HarmonyWork.Data;
using HarmonyWork.DTOs;
using HarmonyWork.Models;
using Microsoft.EntityFrameworkCore;

namespace HarmonyWork.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly AppDbContext _context;

        public TarefaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TarefaResponseDto>> ObterTodasAsync()
        {
            var tarefas = await _context.Tarefas.ToListAsync();
            return tarefas.Select(t => new TarefaResponseDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                DataVencimento = t.DataVencimento,
                EnergiaNecessaria = t.EnergiaNecessaria.ToString(),
                Status = t.Status.ToString()
            });
        }

        public async Task<TarefaResponseDto?> ObterPorIdAsync(int id)
        {
            var t = await _context.Tarefas.FindAsync(id);
            if (t == null) return null;

            return new TarefaResponseDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                DataVencimento = t.DataVencimento,
                EnergiaNecessaria = t.EnergiaNecessaria.ToString(),
                Status = t.Status.ToString()
            };
        }

        public async Task<TarefaResponseDto> CriarTarefaAsync(TarefaRequestDto dto)
        {
            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                DataVencimento = dto.DataVencimento,
                EnergiaNecessaria = dto.EnergiaNecessaria,
                Status = StatusTarefa.Pendente,
                DataCriacao = DateTime.Now
            };

            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();

            return new TarefaResponseDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataVencimento = tarefa.DataVencimento,
                EnergiaNecessaria = tarefa.EnergiaNecessaria.ToString(),
                Status = tarefa.Status.ToString()
            };
        }

        public async Task<bool> AtualizarTarefaAsync(int id, TarefaRequestDto dto)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return false;

            tarefa.Titulo = dto.Titulo;
            tarefa.Descricao = dto.Descricao;
            tarefa.DataVencimento = dto.DataVencimento;
            tarefa.EnergiaNecessaria = dto.EnergiaNecessaria;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletarTarefaAsync(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return false;

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}