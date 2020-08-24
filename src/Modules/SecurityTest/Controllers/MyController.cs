using Microsoft.AspNetCore.Mvc;

namespace SecurityTest.Controllers
{
    [Area("ActivityLog")]
    [Route("api/securitytest")]
    public class MyController : Controller
    {
        private readonly Class1 myClass;

        public MyController(Class1 myClass)
        {
            this.myClass = myClass;
        }

        [HttpGet("test")]
        public IActionResult Test(string title, string contents, string user)
        {
            myClass.Method1(title, contents, user);
            return Ok("ok");
        }
    }
}