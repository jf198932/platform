using Abp.EntityFramework;
using isriding.Refound;

namespace isriding.EntityFramework.Repositories.Refound
{
    public class RefoundReadRepository : ReadonlyisridingRepositoryBase<Entities.Refound, int>, IRefoundReadRepository
    {
        public RefoundReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}