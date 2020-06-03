using System;
using System.ComponentModel.DataAnnotations;

namespace NewsPublish.API.ApiAdmin.Models.User
{
    /// <summary>
    /// 添加修改用户所用的DTO对象
    /// </summary>
    public abstract class UserAddOrUpdateDto
    {
        private string _roleId;
        [Display(Name = "用户角色ID")]
        [Required(ErrorMessage = "{0}这个属性是必填的，如果不清楚用户所在的用户组，可以先进行查询")]
        [MaxLength(100, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string RoleId
        {
            get => _roleId;
            set
            {
                _roleId = value;
                RoleIdGuid = Guid.Parse(_roleId);
            }
        }
        public Guid RoleIdGuid { get; set; }
        
        [Display(Name = "昵称")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MaxLength(20, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string NickName { get; set; }
        [Display(Name = "介绍")]
        [MaxLength(100, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string Introduce { get; set; }

        
    }
}