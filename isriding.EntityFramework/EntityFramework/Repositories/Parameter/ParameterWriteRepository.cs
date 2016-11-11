using Abp.EntityFramework;
using isriding.Parameter;

namespace isriding.EntityFramework.Repositories.Parameter
{
    public class ParameterWriteRepository : isridingRepositoryBase<Entities.Parameter, int>, IParameterWriteRepository
    {
        public ParameterWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}