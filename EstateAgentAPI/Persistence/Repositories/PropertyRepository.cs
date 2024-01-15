using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Models;

namespace EstateAgentAPI.Persistence.Repositories
{
    public class PropertyRepository : RepositoryBase<Property>, IPropertyRepository
    {
        public PropertyRepository(EstateAgentContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
