using System.Collections.Generic;
using System.Threading.Tasks;
using agendamento_coordenacao.Models;
using agendamento_coordenacao.Models.Dtos;

namespace agendamento_coordenacao.Repositories
{
    public interface IAgendaRepository
    {
        Task<IEnumerable<AgendaDto>> GetActualSchedules(int id);
    }
}