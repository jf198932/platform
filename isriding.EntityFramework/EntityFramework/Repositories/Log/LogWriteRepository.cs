using Abp.Domain.Repositories;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using isriding.Log;

namespace isriding.EntityFramework.Repositories.Log
{
    public class LogWriteRepository : isridingRepositoryBase<Entities.Log,int>, ILogWriteRepository
    {
        public LogWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}