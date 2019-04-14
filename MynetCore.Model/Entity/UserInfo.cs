
using MynetCore.Model.EntityBaseBase;
using System;

namespace MynetCore.Model.Entity
{   
    //表名就这个类名 UserInfo字段下面
    public class UserInfo: EntityBase
    {
        /// <summary>
        /// ID
        /// </summary>
        public int userid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int userage { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string userpwd { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string usersex { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string usereamil{ get; set; }
    }
}
