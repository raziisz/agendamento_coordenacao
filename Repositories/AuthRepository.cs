using System.Threading.Tasks;
using agendamento_coordenacao.Data;
using agendamento_coordenacao.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace agendamento_coordenacao.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
           _context = context;

        }
        public async Task<UserReturn> Login(LoginDto login)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == login.Username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }



            return new UserReturn
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.TypeUser.ToString()
            };
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i< computedHash.Length; i++){
                    if(computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
    }
}