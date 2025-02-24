using BusinessServiceLayer.DTOs;
using DataAccessLayer.Specifications.Categories;

namespace BusinessServiceLayer.Interfaces
{
    public interface ITagService
    {
        Task<IReadOnlyList<TagDTO>> GetTagsAsync(string? search);
        Task<TagDTO> GetTagByIdAsync(int id);
        Task<bool> UpdateTagAsync(int id, TagToAddOrUpdateDTO tag);
        Task<bool> CreateTagAsync(TagToAddOrUpdateDTO tag);
    }
}
