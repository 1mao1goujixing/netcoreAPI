using System;
using System.Collections.Generic;
using System.Text;

namespace MynetCore.Model.DTO
{
    /// <summary>
    /// 一个Hresult
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public class MessageModel<T>
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 返回的结果集
        /// </summary>
        public List<T> Data { get; set; }
    }
}
