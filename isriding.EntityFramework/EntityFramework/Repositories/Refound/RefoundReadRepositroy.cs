using Abp.EntityFramework;
using isriding.Refound;

namespace isriding.EntityFramework.Repositories.Refound
{
    public class RefoundReadRepositroy : ReadonlyisridingRepositoryBase<Entities.Refound, int>, IRefoundReadRepository
    {
        public RefoundReadRepositroy(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}