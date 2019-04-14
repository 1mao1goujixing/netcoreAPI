using MynetCore.IRepository.Base;
using MynetCore.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MynetCore.IRepository
{
    public interface UserInfoIRepository : IBaseRepository<UserInfo>
    {
         Task<UserInfo> GetUserInfoById(string Id);

        Task<List<UserInfo>> GetUserInfoList();
        string Userstr();
       
    }
}
