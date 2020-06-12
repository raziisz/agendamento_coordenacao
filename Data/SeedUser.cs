using System.Linq;
using agendamento_coordenacao.Models;
using agendamento_coordenacao.Models.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;


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

            if(!_context.Schedules.Any()) {
                var schedulesData = System.IO.File.ReadAllText("Data/SeedSchedules.json");
                List<AgendaDto> schedules = JsonConvert.DeserializeObject<List<AgendaDto>>(schedulesData);

                foreach (var schedule in schedules)
                {
                    if(schedule.StartProject != null) {
                        _context.Projects.Add(new Project
                        {
                            Title = schedule.Title,
                            Description = schedule.Description,
                            Local = schedule.Local,
                            StartProject = schedule.StartProject,
                            EndProject = schedule.EndProject,
                            Reschedule = schedule.Reschedule,
                            UserId = 1
                        });
                    } else if(schedule.DateReunion != null) {
                         _context.Reunions.Add(new Reunion
                        {
                            Title = schedule.Title,
                            Description = schedule.Description,
                            Local = schedule.Local,
                            Reschedule = schedule.Reschedule,
                            UserId = 1,
                            DateReunion = schedule.DateReunion,
                            HourEnd = schedule.HourEnd,
                            HourStart = schedule.HourStart
                        });
                    } else if(schedule.DateWork != null) {
                         _context.Works.Add(new Work
                        {
                            Title = schedule.Title,
                            Description = schedule.Description,
                            Local = schedule.Local,
                            Reschedule = schedule.Reschedule,
                            UserId = 1,
                            DateWork = schedule.DateWork
                        });
                    }
                }

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