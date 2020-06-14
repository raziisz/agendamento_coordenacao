using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using agendamento_coordenacao.Data;
using agendamento_coordenacao.Helpers;
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
        public async Task<IEnumerable<AgendaDto>> GetActualSchedules(int id)
        {
            var works = await _context.Works.Where(u => u.UserId == id).Select(w => new AgendaDto
            {
                Id = w.Id,
                Title = w.Title,
                Description = w.Description,
                DateWork = w.DateWork,
                Local = w.Local,
                Reschedule = w.Reschedule,
                Tipo = "Tarefa"
            }).Skip(0).Take(5).ToListAsync();

            var projects = await _context.Projects.Where(u => u.UserId == id).Select(p => new AgendaDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Local = p.Local,
                Reschedule = p.Reschedule,
                StartProject = p.StartProject,
                EndProject = p.EndProject,
                Tipo = "Projeto"
            }).Skip(0).Take(5).ToListAsync();

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
                Tipo = "Reunião"
            }).Skip(0).Take(5).ToListAsync();

            var schedules = new List<AgendaDto>();
            schedules.AddRange(works);
            schedules.AddRange(projects);
            schedules.AddRange(reunions);

            return schedules.OrderBy(s => s.DateReunion)
                .ThenBy(s => s.DateWork)
                .ThenBy(s => s.StartProject)
                .ThenBy(s => s.Tipo).ToArray();
        }

        public async Task<PagedList<AgendaDto>> GetSchedules(AtividadesParams ap, int id)
        {
            var worksQuery = _context.Works
                .OrderBy(w => w.DateWork)
                .AsNoTracking()
                .AsQueryable();

            var projectsQuery = _context.Projects
                .OrderBy(p => p.StartProject)
                .AsNoTracking()
                .AsQueryable();

            var reunionsQuery = _context.Reunions
                .OrderBy(r => r.DateReunion)
                .AsNoTracking()
                .AsQueryable();

            var startDate = new DateTime();
            var endDate = new DateTime();

            if (ap.StartDate != "" && ap.EndDate != "")
            {
                startDate = DateTime.ParseExact(ap.StartDate, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("pt-BR"));
                endDate = DateTime.ParseExact(ap.EndDate, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("pt-BR"));

                worksQuery = worksQuery.Where(w => 
                    startDate >= w.DateWork && w.DateWork <= endDate);
                
                projectsQuery = projectsQuery.Where(p =>
                    startDate >= p.StartProject && p.StartProject <= endDate);
                
                reunionsQuery = reunionsQuery.Where(r =>
                    startDate >= r.DateReunion && r.DateReunion <= endDate);
            }

            if (ap.Title != "") {
                worksQuery = worksQuery.Where(w => w.Title.Contains(ap.Title));
                projectsQuery = projectsQuery.Where(w => w.Title.Contains(ap.Title));
                reunionsQuery = reunionsQuery.Where(w => w.Title.Contains(ap.Title));
            }

            var countWorks = await worksQuery.CountAsync();
            var countProjects = await projectsQuery.CountAsync();
            var countReunions = await reunionsQuery.CountAsync();

            var count = countProjects + countReunions + countWorks;

            var works = await worksQuery.Select(w => new AgendaDto
            {
                Id = w.Id,
                Title = w.Title,
                Description = w.Description,
                DateWork = w.DateWork,
                Local = w.Local,
                Reschedule = w.Reschedule,
                Tipo = "Tarefa"
            }).ToListAsync();

            var projects =  await projectsQuery.Select(p => new AgendaDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Local = p.Local,
                Reschedule = p.Reschedule,
                StartProject = p.StartProject,
                EndProject = p.EndProject,
                Tipo = "Projeto"
            }).ToListAsync();

            var reunions =  await reunionsQuery.Select(r => new AgendaDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Local = r.Local,
                Reschedule = r.Reschedule,
                DateReunion = r.DateReunion,
                HourStart = r.HourStart,
                HourEnd = r.HourEnd,
                Tipo = "Reunião"
            }).ToListAsync();


            var schedules = new List<AgendaDto>();
            schedules.AddRange(reunions);
            schedules.AddRange(works);
            schedules.AddRange(projects);

            var itens =  schedules
                .Skip((ap.PageNumber - 1) * ap.PageSize)
                .Take(ap.PageSize)
                .OrderByDescending(s => s.Tipo)
                .ToList();

            return new PagedList<AgendaDto>(itens, count, ap.PageNumber, ap.PageSize);
        }
    }
}