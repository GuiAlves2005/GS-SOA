using Asp.Versioning;
using HarmonyWork.Data;
using HarmonyWork.DTOs;
using HarmonyWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HarmonyWork.Controllers
{
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")] // Requisito: Versionamento
    [Route("api/v{version:apiVersion}/[controller]")] // Rota fica: api/v1/tarefas
    public class TarefasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TarefasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/tarefas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarefaResponseDto>>> GetTarefas()
        {
            var tarefas = await _context.Tarefas.ToListAsync();

            // Converte Model para DTO
            var tarefasDto = tarefas.Select(t => new TarefaResponseDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                DataVencimento = t.DataVencimento,
                EnergiaNecessaria = t.EnergiaNecessaria.ToString(),
                Status = t.Status.ToString()
            });

            return Ok(tarefasDto);
        }

        // GET: api/v1/tarefas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TarefaResponseDto>> GetTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null) return NotFound("Tarefa não encontrada.");

            var dto = new TarefaResponseDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataVencimento = tarefa.DataVencimento,
                EnergiaNecessaria = tarefa.EnergiaNecessaria.ToString(),
                Status = tarefa.Status.ToString()
            };

            return Ok(dto);
        }

        // POST: api/v1/tarefas
        [HttpPost]
        public async Task<ActionResult<TarefaResponseDto>> PostTarefa(TarefaRequestDto dto)
        {
            // Converte DTO para Model (Entidade)
            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                DataVencimento = dto.DataVencimento,
                EnergiaNecessaria = dto.EnergiaNecessaria,
                Status = StatusTarefa.Pendente, // Padrão ao criar
                DataCriacao = DateTime.Now
            };

            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();

            // Retorna 201 Created
            return CreatedAtAction(nameof(GetTarefa), new { id = tarefa.Id, version = "1" }, dto);
        }

        // PUT: api/v1/tarefas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarefa(int id, TarefaRequestDto dto)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return NotFound();

            tarefa.Titulo = dto.Titulo;
            tarefa.Descricao = dto.Descricao;
            tarefa.DataVencimento = dto.DataVencimento;
            tarefa.EnergiaNecessaria = dto.EnergiaNecessaria;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/v1/tarefas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return NotFound();

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}