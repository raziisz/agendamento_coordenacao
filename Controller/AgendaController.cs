using System.Security.Claims;
using System.Threading.Tasks;
using agendamento_coordenacao.Helpers;
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
        public AgendaController(IAgendaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var schedules = await _repo.GetActualSchedules(userId);
            return Ok(new {
                schedules
            });
        }

        [HttpGet("atividades")] 
        public async Task<IActionResult> Tasks([FromQuery] AtividadesParams ap)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var schedules = await _repo.GetSchedules(ap, userId);
            return Ok(new { schedules });
        }
    }
}