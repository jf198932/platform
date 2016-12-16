using Abp.EntityFramework;
using isriding.TbTroubleFeedback;

namespace isriding.EntityFramework.Repositories.TbTroubleFeedback
{
    public class TbTroubleFeedbackWriteRepository : isridingRepositoryBase<Entities.Tb_trouble_feedback, int>, ITbTroubleFeedbackWriteRepository
    {
        public TbTroubleFeedbackWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}