using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace isriding.User
{
    public interface IUserWriteRepository : IRepository<Entities.User, int>
    {////////////////IUserReadRepository
        //Task<bool> CheckIdentity(string phoneNum, string token);
        //Task<bool> CheckLogin(string phoneNum, string checkCode);
        //Task<bool> RegisterUser(string phoneNum);
    }
}