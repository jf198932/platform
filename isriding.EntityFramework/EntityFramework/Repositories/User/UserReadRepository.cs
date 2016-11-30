using Abp.EntityFramework;
using isriding.User;
using System.Linq;
using System.Threading.Tasks;

namespace isriding.EntityFramework.Repositories.User
{
    public class UserReadRepository : ReadonlyisridingRepositoryBase<Entities.User, int>, IUserReadRepository
    {
        public UserReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}