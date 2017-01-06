using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using AutoMapper;
using isriding.School.Dto;

namespace isriding.School
{
    public class SchoolAppService : isridingAppServiceBase, ISchoolAppService
    {
        private readonly ISchoolReadRepository _schoolReadRepository;

        public SchoolAppService(ISchoolReadRepository schoolReadRepository)
        {
            _schoolReadRepository = schoolReadRepository;
        }

        public async Task<List<SchoolOutput>> GetSchoolList()
        {
            var school = await _schoolReadRepository.GetAllListAsync(t => !string.IsNullOrEmpty(t.Gps_point) || t.Name == "社会");
            if (school == null)
            {
                throw new UserFriendlyException("没有学校");
            }
            Mapper.Initialize(t => t.CreateMap<Entities.School, SchoolOutput>());
            return new List<SchoolOutput>(Mapper.Map<List<SchoolOutput>>(school));
        }
    }
}