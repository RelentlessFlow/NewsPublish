using System;
using System.Collections.Generic;
using NewsPublish.Infrastructure.Services.AuthorizeServices.DTO;
using NewsPublish.Infrastructure.Services.AuthorizeServices.Implementation;

namespace NewsPublish.Infrastructure.Services.AuthorizeServices.Interface
{
    public interface ITokenList
    {
        /// <summary>
        /// 删除登陆信息
        /// </summary>
        /// <param name="token"></param>
        void delUserToken(string token);

        /// <summary>
        /// 删除登陆信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="rightNames"></param>
        /// <returns></returns>
        string addUserAuthe(string account, List<string> rightNames);

        /// <summary>
        /// 登陆信息是否存在
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool isExistAuth(string token);

        /// <summary>
        /// 通过Token获取用户的登陆信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserTokenWithRight GetToken(string token);

        List<UserTokenWithRight> GetTokenLists();
    }
}