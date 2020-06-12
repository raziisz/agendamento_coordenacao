using System.Linq;
using agendamento_coordenacao.Models;
using Newtonsoft.Json;

namespace agendamento_coordenacao.Data
{
    public class SeedUser
    {
        private readonly DataContext _context;
        public SeedUser(DataContext context)
        {
            _context = context;

        }
        public void SeedUserAdmin()
        {
            if(!_context.Users.Any()) {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var user = JsonConvert.DeserializeObject<User>(userData);
    
                byte[] passwordHash, passwordSalt;
                
                CreatePasswordHash("123", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Add(user);
            

                _context.SaveChanges();
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