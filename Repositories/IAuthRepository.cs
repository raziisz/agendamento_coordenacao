using System.Threading.Tasks;
using agendamento_coordenacao.Models;
using agendamento_coordenacao.Models.Dtos;

namespace agendamento_coordenacao.Repositories
{
    public interface IAuthRepository
    {
        Task<UserReturn> Login(LoginDto login);
    }
}