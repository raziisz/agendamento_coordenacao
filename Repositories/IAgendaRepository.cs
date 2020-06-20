using System.Collections.Generic;
using System.Threading.Tasks;
using agendamento_coordenacao.Helpers;
using agendamento_coordenacao.Models;
using agendamento_coordenacao.Models.Dtos;

namespace agendamento_coordenacao.Repositories
{
    public interface IAgendaRepository
    {
        Task<IEnumerable<AgendaDto>> GetActualSchedules(int id);
        Task<PagedList<AgendaDto>> GetSchedules(AtividadesParams ap, int id);
        Task Add(AgendaDto agenda);
        Task<AgendaDto> GetSchedule(int id);
        Task Update(AgendaDto agenda);
        Task Delete(int id);
    }
}