using MynetCore.IRepository;
using MynetCore.Model.Entity;
using MynetCore.Repository.Base;
using MynetCore.Tool.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MynetCore.Repository
{
    public class UserInfoRepository : BaseRepository<UserInfo>, UserInfoIRepository
    {


        public async Task<UserInfo> GetUserInfoById(string Id)
        {
            return await base.QueryById(Id);
        }
        
        public async Task<List<UserInfo>> GetUserInfoList()
        {
            return await base.Query(o=>o.userid==1);
        }
        public string Userstr()
        {
            return "哈哈哈";
        }


    }
}
