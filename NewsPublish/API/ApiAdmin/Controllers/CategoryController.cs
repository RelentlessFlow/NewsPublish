using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAdmin.Models.Category;
using NewsPublish.API.ApiAdmin.Models.User;
using NewsPublish.API.ApiAuthorization.Filter;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Infrastructure.Services.AdminServices;
using NewsPublish.Infrastructure.Services.AdminServices.DTO;
using NewsPublish.Infrastructure.Services.AdminServices.Interface;
using NewsPublish.Infrastructure.Services.CommonServices.DTO;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiAdmin.Controllers
{
    /// <summary>
    /// 文章分类CURD
    /// 过滤器：管理员、授权用户
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [ServiceFilter(typeof(AdminFilter))]
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly IArticleRepository _repository;
        private readonly IMapper _mapper;

        public CategoryController(IArticleRepository repository,IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
        
        /// <summary>
        /// 通过分类ID获取分类详细信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("{categoryId}", Name = nameof(GetCategory))]
        public async Task<ActionResult<Category>> GetCategory(Guid categoryId)
        {
            
            return Ok(_mapper.Map<CategoryDto>(await _repository.GetCategory(categoryId)) );
        }
        
        /// <summary>
        /// 创建文章分类
        /// </summary>
        /// <param name="category">添加分类的DTO</param>
        /// <returns>新增加分类的路由地址</returns>
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CategoryAddDto category)
        {
            if (await _repository.CategoryIsExists(category.Name))
            {
                return ValidationProblem("分类名已存在！");
            }
            
            var entitiesToAdd = _mapper.Map<Category>(category);
            _repository.AddCategory(entitiesToAdd);
            await _repository.SaveAsync();
            var dtoReturn = _mapper.Map<CategoryDto>(entitiesToAdd);
            return CreatedAtRoute(nameof(GetCategory), new {categoryId = entitiesToAdd.Id},dtoReturn);
        }
        
        /// <summary>
        /// 通过ID删除分类信息
        /// </summary>
        /// <param name="categoryId">分类ID</param>
        /// <returns>204状态码</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            Category entities = await _repository.GetCategory(categoryId);
            _repository.DeleteCategory(entities);
            return NoContent();
        }
        
        /// <summary>
        /// 通过分类ID更新一个分类信息
        /// </summary>
        /// <param name="categoryId">分类的ID</param>
        /// <param name="category">更新分类的DTO</param>
        /// <returns></returns>
        [HttpPut("{categoryId}")]
        public async Task<ActionResult<UserDto>> UpdateCategory(Guid categoryId, CategoryUpdateDto category)
        {
            var entities = await _repository.GetCategory(categoryId);
            if (entities == null)
            {
                if (await _repository.CategoryIsExists(category.Name))
                {
                    return ValidationProblem("分类名已存在！");
                }

                var addToEntities = _mapper.Map<Category>(category);
                addToEntities.Id = categoryId;
                _repository.AddCategory(addToEntities);
                await _repository.SaveAsync();
                var dtoToReturn = _mapper.Map<CategoryDto>(addToEntities);
                return CreatedAtRoute(nameof(GetCategory), new {categoryId = dtoToReturn.Id}, dtoToReturn);
            }
            
            _mapper.Map(category,entities);
            await _repository.SaveAsync();
            var updateToReturn = _mapper.Map<CategoryDto>(entities);
            return Ok(updateToReturn);
        }
    }
}