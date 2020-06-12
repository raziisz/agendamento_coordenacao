using System.Collections.Generic;
using agendamento_coordenacao.Models.Enums;

namespace agendamento_coordenacao.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public TypeUser TypeUser { get; set; }

        public User()
        {
            
        }
    }
}