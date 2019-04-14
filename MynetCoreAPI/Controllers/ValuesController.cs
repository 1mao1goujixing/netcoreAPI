using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MynetCore.IServices;
using MynetCoreAPI.AuthHelper;

namespace MynetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ValuesController : Controller
    {
        //   UserInfoIServices userInfoIServices = new UserInfoServices();
        UserInfoIServices UserInfoServices;
        //构造器ioc
        public ValuesController(UserInfoIServices UserInfoServices)
        {

            this.UserInfoServices = UserInfoServices;
        }
        // GET api/values

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            TokenModel model = new TokenModel()
            {
                Uid = 1,
                Uname = "TT",
                Phone = "666",
                Icon = "23123",
                UNickname = "糖糖",
                Sub = "Admin"
            };
            TimeSpan time1 = (DateTime.Now.AddDays(1) - DateTime.Now);
            TimeSpan time2 = (DateTime.Now.AddMonths(1) - DateTime.Now);
            RayPIToken.IssueJWT(model, time1, time2);


          
            var result= UserInfoServices.Userstr();
           
            return result;
        }
        

        // POST api/values
        
        [HttpPost]
      
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
