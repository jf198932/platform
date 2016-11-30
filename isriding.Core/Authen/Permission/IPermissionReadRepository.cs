using Abp.Domain.Repositories;

namespace isriding.Authen.Permission
{
    public interface IPermissionReadRepository : IRepository<Entities.Authen.Permission, int>
    {
         
    }
}