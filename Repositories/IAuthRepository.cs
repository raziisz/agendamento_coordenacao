using System.Threading.Tasks;
using backend.Models;
using backend.Models.Dtos;

namespace backend.Repositories
{
    public interface IAuthRepository
    {
        Task<UserReturn> Login(LoginDto login);
    }
}