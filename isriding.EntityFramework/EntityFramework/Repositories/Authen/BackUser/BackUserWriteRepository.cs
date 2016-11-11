using Abp.EntityFramework;
using isriding.Authen.BackUser;

namespace isriding.EntityFramework.Repositories.Authen.BackUser
{
    public class BackUserWriteRepository : isridingRepositoryBase<Entities.Authen.BackUser, int>, IBackUserWriteRepository
    {
        public BackUserWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}