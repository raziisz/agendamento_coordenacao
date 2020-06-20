using System;
using System.Security.Claims;
using System.Threading.Tasks;
using agendamento_coordenacao.Helpers;
using agendamento_coordenacao.Models.Dtos;
using agendamento_coordenacao.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace agendamento_coordenacao.Controller
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class AgendaController : ControllerBase
    {
        private readonly IAgendaRepository _repo;
        private readonly IUnityOfWork _uof;
        public AgendaController(IAgendaRepository repo, IUnityOfWork uof)
        {
            _uof = uof;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var schedules = await _repo.GetActualSchedules(userId);
            
            return Ok(new
            {
                schedules
            });
        }

        [HttpGet("atividades")]
        public async Task<IActionResult> Tasks([FromQuery] AtividadesParams ap)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var schedules = await _repo.GetSchedules(ap, userId);

            Response.AddPagination(schedules.CurrentPage, schedules.PageSize,
                schedules.TotalCount, schedules.TotalPages);

            return Ok(new { schedules });
        }

        [HttpPost]
        public async Task<IActionResult> Store([FromBody] AgendaDto agenda)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            agenda.UserId = userId;

            await _repo.Add(agenda);

            if (await _uof.Commit()) {
                return StatusCode(201, new { message = "Atividade criada com sucesso!"});
            }

            throw new Exception("Houve um erro interno ao salvar uma atividade");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var schedule = await _repo.GetSchedule(id);

            if(schedule == null) return NotFound(new { message = "Atividade não encontrada"});
            if(schedule.UserId != userId) 
                return Forbid();

            return Ok(new { schedule });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] AgendaDto agenda, int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var schedule = await _repo.GetSchedule(id);

            if(schedule == null) return NotFound(new { message = "Atividade não encontrada"});
            if(schedule.UserId != userId) 
                return Forbid();
            
            agenda.Id = schedule.Id;
            agenda.UserId = schedule.UserId;

            await _repo.Update(agenda);

            if(await _uof.Commit()) 
                return NoContent();

            throw new Exception("Houve um erro ao atualizar uma atividade!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Destroy(int id) 
        {
             var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var schedule = await _repo.GetSchedule(id);

            if(schedule == null) return NotFound(new { message = "Atividade não encontrada"});
            if(schedule.UserId != userId) 
                return Forbid();
            
            await _repo.Delete(id);

            if(await _uof.Commit())
                return NoContent();
            
            throw new Exception("Houve um erro ao deletar uma atividade!");


        }
    }
}