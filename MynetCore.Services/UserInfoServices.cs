using AutoMapper;
using MynetCore.IRepository;
using MynetCore.IServices;
using MynetCore.Model.DTO;
using MynetCore.Model.Entity;
using MynetCore.Services.Base;
using MynetCore.Tool.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MynetCore.Services
{
    public class UserInfoServices : BaseServices<UserInfo>,UserInfoIServices
    {
        UserInfoIRepository userInfoIRepository;
        IMapper IMapper;
        //构造器ioc
        public UserInfoServices(UserInfoIRepository UserInfoIRepository, IMapper IMapper)
        {

            this.userInfoIRepository = UserInfoIRepository;
            base.BaseDal = UserInfoIRepository;
            this.IMapper = IMapper;
        }
        public async Task<UserInfoDTO> GetUserInfoById(string Id)
        {
            UserInfoDTO models = new UserInfoDTO();
            try
            {
                UserInfo result = await userInfoIRepository.GetUserInfoById(Id);
                
                models = IMapper.Map<UserInfoDTO>(result);
               
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
           
            return models;
        }
        //加特性标记
        [Caching(AbsoluteExpiration = 10)]
        public async Task<List<UserInfo>> GetUserInfoList()
        {  
              //可以写在userInfoIRepository类里面但是有点麻烦 好处是比较规范
              // return await this.userInfoIRepository.GetUserInfoList();
              //直接调用base 的BaseServices 获取值里面实现了BaseRepository 的注入
            return await base.Query(o => o.userid == 1);
        }

        public string Userstr()
        {

            return userInfoIRepository.Userstr();
        }
    }
}
