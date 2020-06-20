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

        public async Task Add(AgendaDto agenda)
        {
            
            Schedule schedule;
            
            if (agenda.DateReunion != null) {
                schedule = new Reunion(agenda);
                await _context.Schedules.AddAsync(schedule);

            } else if (agenda.DateWork != null) {
                schedule = new Work(agenda);
                await _context.Schedules.AddAsync(schedule);

            } else if (agenda.StartProject != null) {
                schedule = new Project(agenda);
                await _context.Schedules.AddAsync(schedule);

            }

        }

        public async Task Delete(int id)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(x => x.Id == id);

            _context.Remove(schedule);
        }

        public async Task<IEnumerable<AgendaDto>> GetActualSchedules(int id)
        {
            var dateCurrent = DateTime.Now;
            var worksQuery =  _context.Works
                .Where(u => u.UserId == id).AsQueryable();
            
            worksQuery =  worksQuery.Where(u => u.DateWork >= dateCurrent);


            var works = await worksQuery.Select(w => new AgendaDto
            {
                Id = w.Id,
                Title = w.Title,
                Description = w.Description,
                DateWork = w.DateWork,
                Local = w.Local,
                Reschedule = w.Reschedule,
                Tipo = "Tarefa"
            }).Skip(0).Take(5).ToListAsync();

            var projectsQuery = _context.Projects
                .Where(u => u.UserId == id).AsQueryable();
            projectsQuery = projectsQuery.Where(u => u.EndProject >= dateCurrent);
            var projects = await projectsQuery.Select(p => new AgendaDto
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

            var reunionsQuery = _context.Reunions
                .Where(r => r.UserId == id).AsQueryable();
            reunionsQuery = reunionsQuery.Where(u => u.DateReunion >= dateCurrent);
                
            var reunions = await reunionsQuery.Select(r => new AgendaDto
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

        public async Task<AgendaDto> GetSchedule(int id)
        {
            var work = await _context.Works
                .Where(x => x.Id == id)
                .Select(x => new AgendaDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Local = x.Local,
                    DateWork = x.DateWork,
                    Reschedule = x.Reschedule,
                    Tipo = "Tarefa",
                    UserId = x.UserId
                })
                .FirstOrDefaultAsync();
            
            if(work != null) return work;

            var project = await _context.Projects
                .Where(x => x.Id == id)
                .Select(x => new AgendaDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Local = x.Local,
                    Reschedule = x.Reschedule,
                    Tipo = "Projeto",
                    UserId = x.UserId,
                    StartProject = x.StartProject,
                    EndProject = x.EndProject
                })
                .FirstOrDefaultAsync();
            
            if(project != null) return project;
            
            var reunion = await _context.Reunions
                .Where(x => x.Id == id)
                .Select(x => new AgendaDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Local = x.Local,
                    Reschedule = x.Reschedule,
                    Tipo = "Reunião",
                    UserId = x.UserId,
                    HourStart = x.HourStart,
                    HourEnd = x.HourEnd,
                    DateReunion = x.DateReunion
                })
                .FirstOrDefaultAsync();
            
            if(reunion != null) return reunion;

            return null;
        }

        public async Task<PagedList<AgendaDto>> GetSchedules(AtividadesParams ap, int id)
        {
            var dateCurrent = DateTime.Now;

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
            
            worksQuery = worksQuery.Where(w => w.DateWork >= dateCurrent);
            projectsQuery = projectsQuery.Where(p => p.EndProject >= dateCurrent);
            reunionsQuery = reunionsQuery.Where(r => r.DateReunion >= dateCurrent);
            
            if (ap.StartDate != "" && ap.EndDate != "")
            {
                startDate = DateTime.ParseExact(ap.StartDate, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("pt-BR"));
                endDate = DateTime.ParseExact(ap.EndDate, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("pt-BR"));

                worksQuery = worksQuery.Where(w => 
                    startDate >= w.DateWork);
                
                projectsQuery = projectsQuery.Where(p =>
                    startDate >= p.StartProject && p.EndProject <= endDate);
                
                reunionsQuery = reunionsQuery.Where(r =>
                    startDate >= r.DateReunion);
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
                Tipo = "Tarefa",
                UserId = w.UserId
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
                Tipo = "Projeto",
                UserId = p.UserId
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
                Tipo = "Reunião",
                UserId = r.UserId
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

        public async Task Update(AgendaDto agenda)
        {
            var work = await _context.Works
                .Where(x => x.Id == agenda.Id)
                .FirstOrDefaultAsync();
            
            if(work != null) {
                work.Local = agenda.Local;
                work.Reschedule = agenda.Reschedule;
                work.Title = agenda.Title;
                work.Description = agenda.Description;
                work.UserId = agenda.UserId;
                work.DateWork = agenda.DateWork;
            }

            var project = await _context.Projects
                .Where(x => x.Id == agenda.Id)
                .FirstOrDefaultAsync();
            
            if(project != null) {
                project.Local = agenda.Local;
                project.Reschedule = agenda.Reschedule;
                project.Title = agenda.Title;
                project.Description = agenda.Description;
                project.UserId = agenda.UserId;
                project.StartProject = agenda.StartProject;
                project.EndProject = agenda.EndProject;
            }
            
            var reunion = await _context.Reunions
                .Where(x => x.Id == agenda.Id)
                .FirstOrDefaultAsync();
            
            if(reunion != null) {
                reunion.Local = agenda.Local;
                reunion.Reschedule = agenda.Reschedule;
                reunion.Title = agenda.Title;
                reunion.Description = agenda.Description;
                reunion.HourEnd = agenda.HourEnd;
                reunion.HourStart = agenda.HourStart;
                reunion.DateReunion = agenda.DateReunion;
                reunion.UserId = agenda.UserId;
            }
        }
    }
}