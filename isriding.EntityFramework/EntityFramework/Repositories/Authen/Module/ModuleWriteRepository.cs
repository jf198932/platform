using Abp.EntityFramework;
using isriding.Authen.Module;

namespace isriding.EntityFramework.Repositories.Authen.Module
{
    public class ModuleWriteRepository : isridingRepositoryBase<Entities.Authen.Module, int>, IModuleWriteRepository
    {
        public ModuleWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}