using System;
using Abp.EntityFramework;
using isriding.Log;

namespace isriding.EntityFramework.Repositories.Log
{
    public class LogReadRepository : ReadonlyisridingRepositoryBase<Entities.Log, int>, ILogReadRepository
    {
        public LogReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}