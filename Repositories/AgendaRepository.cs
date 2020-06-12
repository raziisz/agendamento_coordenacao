using System.Collections.Generic;
using System.Threading.Tasks;
using agendamento_coordenacao.Data;
using agendamento_coordenacao.Models;
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
        public async Task<IEnumerable<Schedule>> GetActualSchedules()
        {
            return await _context.Schedules.ToListAsync();
        }
    }
}