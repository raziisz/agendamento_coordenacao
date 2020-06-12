using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using agendamento_coordenacao.Data;
using agendamento_coordenacao.Models;
using agendamento_coordenacao.Models.Dtos;
using agendamento_coordenacao.Repositories;
using Microsoft.EntityFrameworkCore;

namespace agendamento_coordenacao.Repositories
{
    public class AgendaRepository : IAgendaRepository
    {
        private readonly DataContext _context;
        public AgendaRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<List<AgendaDto>> GetActualSchedules(int id)
        {
            var works = await _context.Works.Where(u => u.UserId == id).Select(w => new AgendaDto
            {
                Id = w.Id,
                Title = w.Title,
                Description = w.Description,
                DateWork = w.DateWork,
                Local = w.Local,
                Reschedule = w.Reschedule,
            }).ToListAsync();

            var projects = await _context.Projects.Where(u => u.UserId == id).Select(p => new AgendaDto 
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Local = p.Local,
                Reschedule = p.Reschedule,
                StartProject = p.StartProject,
                EndProject = p.EndProject
            }).ToListAsync();

            var reunions = await _context.Reunions.Where(r => r.UserId == id).Select(r => new AgendaDto 
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Local = r.Local,
                Reschedule = r.Reschedule,
                DateReunion = r.DateReunion,
                HourStart = r.HourStart,
                HourEnd = r.HourEnd,
            }).ToListAsync();

            var schedules = new List<AgendaDto>();
            schedules.AddRange(works);
            schedules.AddRange(projects);
            schedules.AddRange(reunions);
            
            return schedules;
        }
    }
}