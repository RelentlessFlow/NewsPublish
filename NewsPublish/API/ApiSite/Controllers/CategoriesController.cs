
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAdmin.Models.Category;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiSite.Controllers
{
    /// <summary>
    /// 获取全部分类 给前台使用的
    /// </summary>
    [Route("api_site/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IArticleRepository _repository;
        private readonly IMapper _mapper;
        public CategoriesController(IArticleRepository repository,IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }
        
        /// <summary>
        /// 获取全部文章分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(await _repository.GetCategories()));
        } 
    }
}