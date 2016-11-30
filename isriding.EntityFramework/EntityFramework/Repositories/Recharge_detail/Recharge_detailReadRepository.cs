using Abp.EntityFramework;
using isriding.Recharge_detail;

namespace isriding.EntityFramework.Repositories.Recharge_detail
{
    public class Recharge_detailReadRepository : ReadonlyisridingRepositoryBase<Entities.Recharge_detail, int>, IRecharge_detailReadRepository
    {
        public Recharge_detailReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}