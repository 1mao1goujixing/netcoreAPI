
using MynetCore.IServices.Base;
using MynetCore.Model.DTO;
using MynetCore.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MynetCore.IServices
{
    public interface UserInfoIServices: IBaseServices<UserInfo>
    {
        string Userstr();
        Task<UserInfoDTO> GetUserInfoById(string Id);
        Task<List<UserInfo>> GetUserInfoList();
    }
}
