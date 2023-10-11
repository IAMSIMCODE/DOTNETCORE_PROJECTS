using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;

namespace CodeCommand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;

        public TestController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        [HttpGet]
        public ActionResult getlist()
        {
            //HttpResponseMessage request = new();


             var methodName = MethodBase.GetCurrentMethod().Name;
            var clientIp = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            //var clientIp6 = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR");
            var clientIp2 = HttpContext.Connection.LocalIpAddress.ToString();
            //var clientIp3 = HttpContext.Connection..ServerVariables["REMOTE_ADDR"];
            var clientIp4 = _accessor.HttpContext.Request.Host.ToString();
            var clientIp5 = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            


            var color = new List<string>
            {
                "red", "blue", "green", methodName, clientIp, clientIp2, clientIp4, clientIp5
            };

            return Ok(color);   
        }


       
           

    
        

    }
}
