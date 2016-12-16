using Abp.EntityFramework;
using isriding.TbTroubleFeedback;

namespace isriding.EntityFramework.Repositories.TbTroubleFeedback
{
    public class TbTroubleFeedbackReadRepository : ReadonlyisridingRepositoryBase<Entities.Tb_trouble_feedback, int>, ITbTroubleFeedbackReadRepository
    {
        public TbTroubleFeedbackReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}