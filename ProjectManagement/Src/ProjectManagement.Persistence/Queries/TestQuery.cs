using System.Linq;
using Hermes.Persistence;
using ProjectManagement.Persistence.Models;

namespace ProjectManagement.Persistence.Queries
{
    public class TestQuery
    {
        private readonly IQueryable<Test> query;

        public TestQuery(IDatabaseQuery query)
        {
            this.query = query.GetQueryable<Test>();
        }

        public object GetAll()
        {
            return query.ToArray();
        }
    }
}
