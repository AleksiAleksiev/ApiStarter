namespace NewInterlex.Core.Interfaces.Gateways.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Shared;

    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetById(int id);

        Task<IReadOnlyList<T>> GetAll();

        Task<T> GetSingleBySpec(Expression<Func<T, bool>> spec);

        Task<IReadOnlyList<T>> GetListBySpec(Expression<Func<T, bool>> spec);

        Task<T> Add(T entity);

        Task Update(T entity);

        Task Delete(T entity);
    }
}