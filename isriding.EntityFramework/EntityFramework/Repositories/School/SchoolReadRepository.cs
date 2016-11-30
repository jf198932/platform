using Abp.EntityFramework;
using isriding.School;

namespace isriding.EntityFramework.Repositories.School
{
    public class SchoolReadRepository : ReadonlyisridingRepositoryBase<Entities.School, int>, ISchoolReadRepository
    {
        public SchoolReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}