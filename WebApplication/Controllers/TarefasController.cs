using Asp.Versioning;
using HarmonyWork.DTOs;
using HarmonyWork.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HarmonyWork.Controllers
{
    // AQUI: Bloqueia todos os métodos, exigindo um token JWT válido
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly ITarefaService _service;

        public TarefasController(ITarefaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarefaResponseDto>>> GetTarefas()
        {
            // PROVA DE AUTORIZAÇÃO: Pega o ID do usuário que está logado (se necessário)
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var resultado = await _service.ObterTodasAsync();
            return Ok(resultado);
        }

       

        [HttpGet("{id}")]
        public async Task<ActionResult<TarefaResponseDto>> GetTarefa(int id)
        {
            var resultado = await _service.ObterPorIdAsync(id);
            if (resultado == null) return NotFound("Tarefa não encontrada.");
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<TarefaResponseDto>> PostTarefa(TarefaRequestDto dto)
        {
            var novaTarefa = await _service.CriarTarefaAsync(dto);
            return CreatedAtAction(nameof(GetTarefa), new { id = novaTarefa.Id, version = "1" }, novaTarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarefa(int id, TarefaRequestDto dto)
        {
            var sucesso = await _service.AtualizarTarefaAsync(id, dto);
            if (!sucesso) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarefa(int id)
        {
            var sucesso = await _service.DeletarTarefaAsync(id);
            if (!sucesso) return NotFound();
            return NoContent();
        }
    }
}