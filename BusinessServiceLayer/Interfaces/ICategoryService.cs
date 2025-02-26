using BusinessServiceLayer.DTOs;
using DataAccessLayer.Specifications.Categories;

namespace BusinessServiceLayer.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDTO>> GetCategoriesAsync(CategorySpecParams specParams);
    Task<int> CountCategoriesAsync(CategorySpecParams specParams);
    Task<CategoryDTO> GetCategoryByIdAsync(int id);

    Task<bool> DeleteCategoryAsync(int id);

    Task<bool> EditCategoryAsync(int id, CategoryToAddOrUpdateDTO category);

    Task<bool> CreateCategoryAsync(CategoryToAddOrUpdateDTO category);
}