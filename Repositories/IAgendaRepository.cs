using System.Collections.Generic;
using System.Threading.Tasks;
using agendamento_coordenacao.Models;

namespace agendamento_coordenacao.Repositories
{
    public interface IAgendaRepository
    {
        Task<IEnumerable<Schedule>> GetActualSchedules();
    }
}