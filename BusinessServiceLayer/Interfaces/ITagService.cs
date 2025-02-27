using BusinessServiceLayer.DTOs;
using DataAccessLayer.Specifications.Categories;
using DataAccessLayer.Specifications.Tags;

namespace BusinessServiceLayer.Interfaces
{
    public interface ITagService
    {
        Task<IReadOnlyList<TagDTO>> GetTagsAsync(TagSpecParams specParams);
        Task<int> CountTagsAsync(TagSpecParams specParams);
        Task<TagDTO> GetTagByIdAsync(int id);
        Task<bool> UpdateTagAsync(int id, TagToAddOrUpdateDTO tag);
        Task<bool> CreateTagAsync(TagToAddOrUpdateDTO tag);
    }
}
