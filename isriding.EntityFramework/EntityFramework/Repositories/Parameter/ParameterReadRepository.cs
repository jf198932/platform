using Abp.EntityFramework;
using isriding.Parameter;

namespace isriding.EntityFramework.Repositories.Parameter
{
    public class ParameterReadRepository : ReadonlyisridingRepositoryBase<Entities.Parameter, int>, IParameterReadRepository
    {
        public ParameterReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}