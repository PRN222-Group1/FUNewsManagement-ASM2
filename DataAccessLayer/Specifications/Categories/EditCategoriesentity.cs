namespace DataAccessLayer.Specifications.Categories;

public class EditCategoriesentity
{
    public int? ParentCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? CategoryDescription { get; set; }
}