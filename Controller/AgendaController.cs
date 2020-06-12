using System.Threading.Tasks;
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

        // [HttpGet]
        // public async Task<IActionResult> Index() 
        // {
        //     // var schedules = await _repo.GetActualSchedules();
        //     // return Ok(new {
        //     //     schedules
        //     // });
        // }
    }
}