using Abp.Domain.Repositories;

namespace isriding.Log
{
    public interface ILogReadRepository : IRepository<Entities.Log, int>
    {
         
    }
}