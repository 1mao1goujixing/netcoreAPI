using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MynetCore.IServices;
using MynetCore.Model.DTO;
using MynetCore.Model.Entity;
using MynetCore.Tool.Helper;
using MynetCore.Tool.Redis;
using MynetCoreAPI.AuthHelper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MynetCoreAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/UserInfo")]
    public class UserInfoController : Controller
    {
        // UserInfoIServices userInfoIServices = new UserInfoServices();
        UserInfoIServices UserInfoServices;
        IRedisCacheManager RedisCacheManager;
        //构造器ioc
        public UserInfoController(UserInfoIServices UserInfoServices, IRedisCacheManager RedisCacheManager)
        {
            this.UserInfoServices = UserInfoServices;
            this.RedisCacheManager = RedisCacheManager;
        }

        // GET: api/UserInfo
        [HttpGet]
        public async Task<List<UserInfo>> Get()
        {
            //得到Readis配置连接
            var connect = Appsettings.app(new string[] { "AppSettings", "RedisCaching", "ConnectionString" });
            //查询数据
            var userinfoList = await UserInfoServices.GetUserInfoList();

            if (RedisCacheManager.Get<object>("redis") != null)
            {
                RedisCacheManager.Get<List<UserInfo>>("redis");
            }
            else
            {
                RedisCacheManager.Set("redis", userinfoList, TimeSpan.FromHours(2));
            }
            return userinfoList;
        }
        /// <summary>
        ///  根据ID得到信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/UserInfo/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<UserInfoDTO> Get(int id)
        {
            var userinfomodel = await UserInfoServices.GetUserInfoById(id.ToString());
           
            return userinfomodel;
        }

        // POST: api/UserInfo
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserInfo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sub"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiresAbsoulute"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("jsonp")]
        [EnableCors("LimitRequests")]
        public void Getjsonp(string callBack, long id = 1, string sub = "Admin", int expiresSliding = 30, int expiresAbsoulute = 30)
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
          string response=  RayPIToken.IssueJWT(model, time1, time2);
            string call = callBack + "({" + response + "})";
            Response.WriteAsync(call);
        }

        
    }
}
