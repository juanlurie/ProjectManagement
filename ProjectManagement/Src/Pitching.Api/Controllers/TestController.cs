using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjectManagement.Persistence.Queries;

namespace Pitching.Api.Controllers
{
    public class TestController : ApiController
    {
        private readonly TestQuery testQuery;

        public TestController(TestQuery testQuery)
        {
            this.testQuery = testQuery;
        }

        [HttpGet]
        [Route("api/Test")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, testQuery.GetAll());
        }
    }
}