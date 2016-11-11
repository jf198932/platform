using Abp.EntityFramework;
using isriding.Refound;

namespace isriding.EntityFramework.Repositories.Refound
{
    public class RefoundWriteRepository : isridingRepositoryBase<Entities.Refound, int>, IRefoundWriteRepository
    {
        public RefoundWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}