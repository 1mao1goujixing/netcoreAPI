using System;
using System.Collections.Generic;
using System.Text;

namespace MynetCore.Model.DTO
{
    //放试图要用到的实体DTO
    public class UserInfoDTO
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
        /// 性別
        /// </summary>
        public string gender { get; set; }

    }
}
