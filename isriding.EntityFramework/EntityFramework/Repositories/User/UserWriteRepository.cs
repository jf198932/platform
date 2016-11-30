using Abp.EntityFramework;
using isriding.User;
using System.Linq;
using System.Threading.Tasks;

namespace isriding.EntityFramework.Repositories.User
{
    public class UserWriteRepository : isridingRepositoryBase<Entities.User, int>, IUserWriteRepository
    {
        public UserWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}