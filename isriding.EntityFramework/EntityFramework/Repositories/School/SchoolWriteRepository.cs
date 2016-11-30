using Abp.EntityFramework;
using isriding.School;

namespace isriding.EntityFramework.Repositories.School
{
    public class SchoolWriteRepository : isridingRepositoryBase<Entities.School, int>, ISchoolWriteRepository
    {
        public SchoolWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}