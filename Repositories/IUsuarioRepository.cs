using System.Threading.Tasks;
using agendamento_coordenacao.Helpers;
using agendamento_coordenacao.Models;
using agendamento_coordenacao.Models.Dtos;

namespace agendamento_coordenacao.Repositories
{
    public interface IUsuarioRepository
    {
        Task Add(UserCreateDto user);
        Task Update(UserCreateDto user, int id);
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<PagedList<UserReturn>> GetUsers(UserParams up);
        void Delete(User user);
    }
}