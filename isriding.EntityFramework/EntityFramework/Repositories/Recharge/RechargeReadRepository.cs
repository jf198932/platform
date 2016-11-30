using Abp.EntityFramework;
using isriding.Recharge;

namespace isriding.EntityFramework.Repositories.Recharge
{
    public class RechargeReadRepository : ReadonlyisridingRepositoryBase<Entities.Recharge, int>, IRechargeReadRepository
    {
        public RechargeReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}