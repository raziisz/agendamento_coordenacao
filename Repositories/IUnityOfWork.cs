using System.Threading.Tasks;
using agendamento_coordenacao.Data;

namespace agendamento_coordenacao.Repositories
{
    public interface IUnityOfWork
    {
        Task<bool> Commit();
        void Rollback();
    }
}