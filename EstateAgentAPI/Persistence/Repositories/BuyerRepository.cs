using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Models;

namespace EstateAgentAPI.Persistence.Repositories
{
    public class BuyerRepository : RepositoryBase<Buyer>, IBuyerRepository
    {
        public BuyerRepository(EstateAgentContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
