using System.Threading.Tasks;

namespace Vendas.Core.DataObjects
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
