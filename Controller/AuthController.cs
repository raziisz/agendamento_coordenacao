using System.Threading.Tasks;
using agendamento_coordenacao.Services;
using backend.Models.Dtos;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace agendamento_coordenacao.Controller
{
  [AllowAnonymous]
  [ApiController]
  [Route("v1/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _repo;
    private readonly IConfiguration _config;
    public AuthController(IAuthRepository repo, IConfiguration config)
    {
      _config = config;
      _repo = repo;

    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
      var user = await _repo.Login(login);

      if (user == null)
      {
        return NotFound(new { message = "username ou senha invalida" });
      }

      var token = TokenService.GenerateTokenUser(user, _config);

      return Ok(new
      {
        token,
        user
      });

    }
  }
}