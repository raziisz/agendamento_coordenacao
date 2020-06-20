using System.Threading.Tasks;
using agendamento_coordenacao.Data;

namespace agendamento_coordenacao.Repositories
{
    public class UnityOfWork : IUnityOfWork
    {
        private readonly DataContext _context;
        public UnityOfWork(DataContext context)
        {
            _context = context;

        }
        public async Task<bool> Commit()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Rollback()
        {
            throw new System.NotImplementedException();
        }
    }
}