using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace isriding.User
{
    public interface IUserReadRepository : IRepository<Entities.User, int>
    {
        //Task<bool> CheckIdentity(string phoneNum, string token);
        //Task<bool> CheckLogin(string phoneNum, string checkCode);
        //Task<bool> RegisterUser(string phoneNum);
    }
}