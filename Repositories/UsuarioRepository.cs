using System.Linq;
using System.Threading.Tasks;
using agendamento_coordenacao.Data;
using agendamento_coordenacao.Helpers;
using agendamento_coordenacao.Models;
using agendamento_coordenacao.Models.Dtos;
using agendamento_coordenacao.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace agendamento_coordenacao.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataContext _context;
        public UsuarioRepository(DataContext context)
        {
            _context = context;

        }
        public async Task Add(UserCreateDto user)
        {
            byte[] PasswordHash, PasswordSalt;
            var newUser = new User();
            newUser.Email = user.Email;
            newUser.LastName = user.LastName;
            newUser.Name = user.Name;
            newUser.TypeUser = (TypeUser)user.TypeUser;

            CreatePasswordHash(user.Password, out PasswordHash, out PasswordSalt);

            newUser.PasswordHash = PasswordHash;
            newUser.PasswordSalt = PasswordSalt;

            await _context.Users.AddAsync(newUser);

        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<UserReturn>> GetUsers(UserParams up)
        {
            var query = _context.Users.Select(x => new UserReturn
            {
                Email = x.Email,
                Id = x.Id,
                Name = x.Name,
                Role = x.TypeUser.ToString()
            }).AsQueryable();

            return await PagedList<UserReturn>.CreateAsync(query, up.PageNumber, up.PageSize);
        }

        public async Task Update(UserCreateDto user, int id)
        {
            byte[] PasswordHash, PasswordSalt;
            var newUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            
            newUser.Email = user.Email;
            newUser.LastName = user.LastName;
            newUser.Name = user.Name;
            newUser.TypeUser = (TypeUser)user.TypeUser;
            
            if (!string.IsNullOrEmpty(user.Password)) {
                CreatePasswordHash(user.Password, out PasswordHash, out PasswordSalt);

                newUser.PasswordHash = PasswordHash;
                newUser.PasswordSalt = PasswordSalt;
            }

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }
    }
}