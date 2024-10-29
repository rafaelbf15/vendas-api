using Vendas.Core.DomainObjects;
using System;

namespace Vendas.Core.DataObjects
{
    public interface IRepository<TEntity> : IDisposable where TEntity : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
