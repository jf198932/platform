using Abp.Domain.Repositories;

namespace isriding.Log
{
    public interface ILogWriteRepository : IRepository<Entities.Log, int>
    {
         
    }
}