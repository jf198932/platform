using Abp.EntityFramework;
using isriding.Recharge;

namespace isriding.EntityFramework.Repositories.Recharge
{
    public class RechargeWriteRepository : isridingRepositoryBase<Entities.Recharge, int>, IRechargeWriteRepository
    {
        public RechargeWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}