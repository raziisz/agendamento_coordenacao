using System;
using System.Threading.Tasks;
using agendamento_coordenacao.Helpers;
using agendamento_coordenacao.Models.Dtos;
using agendamento_coordenacao.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace agendamento_coordenacao.Controller
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repo;
        private readonly IUnityOfWork _uof;
        public UsuarioController(IUsuarioRepository repo, IUnityOfWork uof)
        {
            _uof = uof;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] UserParams up)
        {
            var users = await _repo.GetUsers(up);

            return Ok(new { users });
        }

        [HttpPost]
        public async Task<IActionResult> Store([FromBody] UserCreateDto newUser)
        {

            var user = await _repo.GetUserByEmail(newUser.Email);
            if (user != null) return BadRequest(new { message = "Este email já está em uso!" });

            await _repo.Add(newUser);

            if (await _uof.Commit())
            {
                return StatusCode(201, new { message = "Usuário criado com sucesso!" });
            }

            throw new Exception("Houve um erro ao salvar um novo usuário");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Show([FromRoute] int id)
        {
            var user = await _repo.GetUserById(id);
            if (user == null) return NotFound(new { message = "Usuário não encontrado" });

            return Ok(new
            {
                user.Id,
                user.Name,
                user.LastName,
                Role = user.TypeUser.ToString(),
                user.Email
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserCreateDto userDto)
        {
            var user = await _repo.GetUserById(id);
            if (user == null) return NotFound(new { message = "Usuário não encontrado" });

            await _repo.Update(userDto, id);

            if (await _uof.Commit())
            {
                return NoContent();
            }

            throw new Exception("Houve um erro ao atualizar usuário!");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Destroy([FromRoute] int id)
        {
            var user = await _repo.GetUserById(id);
            if (user == null) return NotFound(new { message = "Usuário não encontrado" });

            _repo.Delete(user);

            if (await _uof.Commit()) {
                return NoContent();
            }

            throw new Exception("Houve um erro ao deletar usuário!");
        }
    }
}