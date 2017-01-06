using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using isriding.School.Dto;

namespace isriding.School
{
    public interface ISchoolAppService : IApplicationService
    {
        [HttpGet]
        Task<List<SchoolOutput>> GetSchoolList();
    }
}