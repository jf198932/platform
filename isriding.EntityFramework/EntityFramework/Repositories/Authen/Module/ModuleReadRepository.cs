using Abp.EntityFramework;
using isriding.Authen.Module;

namespace isriding.EntityFramework.Repositories.Authen.Module
{
    public class ModuleReadRepository : ReadonlyisridingRepositoryBase<Entities.Authen.Module, int>, IModuleReadRepository
    {
        public ModuleReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}