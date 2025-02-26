using AutoMapper;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Microsoft.IdentityModel.Tokens;
using BusinessObjects.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Specifications.Categories;

namespace BusinessServiceLayer.Services;

public class CategoryService : ICategoryService
{
    private readonly IGenericRepository<Category> _repository;
    private readonly IMapper _mapper;

    public CategoryService(IGenericRepository<Category> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CategoryDTO>> GetCategoriesAsync(CategorySpecParams specParams)
    {
        var spec = new CategorySpecification(specParams);
        var categories = await _repository.ListAsync(spec);

        return _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryDTO>>(categories);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        bool result = false;
        var spec = new CategorySpecification(id);
        var category = await _repository.GetEntityWithSpec(spec);

        if (category == null || !category.NewsArticles.IsNullOrEmpty())
        {
            return result;
        }

        category.IsActive = false;

        _repository.Update(category);
        result = await _repository.SaveAllAsync();

        return result;
    }


    public async Task<bool> EditCategoryAsync(int id, CategoryToAddOrUpdateDTO category)
    {
        bool result = false;
        var categoryToUpdate = await _repository.GetByIdAsync(id);

        if (categoryToUpdate == null)
        {
            return result;
        }

        categoryToUpdate.CategoryName = category.CategoryName;
        categoryToUpdate.CategoryDescription = category.CategoryDescription;
        categoryToUpdate.ParentCategoryId = category.CategoryId;
        categoryToUpdate.IsActive = category.Status;

        _repository.Update(categoryToUpdate);
        result = await _repository.SaveAllAsync();

        return result;
    }


    public async Task<bool> CreateCategoryAsync(CategoryToAddOrUpdateDTO category)
    {
        var result = false;
        var categoryToAdd = _mapper.Map<CategoryToAddOrUpdateDTO, Category>(category);

        _repository.Add(categoryToAdd);
        result = await _repository.SaveAllAsync();

        return result;
    }

    public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        return _mapper.Map<Category, CategoryDTO>(category);
    }

    public async Task<int> CountCategoriesAsync(CategorySpecParams specParams)
    {
        var spec = new CategoryCountSpecification(specParams);
        var count = await _repository.CountAsync(spec);
        return count;
    }
}