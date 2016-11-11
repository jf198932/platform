using Abp.EntityFramework;
using isriding.Recharge_detail;

namespace isriding.EntityFramework.Repositories.Recharge_detail
{
    public class Recharge_detailWriteRepository : isridingRepositoryBase<Entities.Recharge_detail, int>, IRecharge_detailWriteRepository
    {
        public Recharge_detailWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}