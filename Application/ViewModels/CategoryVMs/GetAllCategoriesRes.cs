namespace Application.ViewModels.CategoryVMs
{
    public class GetAllCategoriesRes
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}

    public class CreateCategoryReq
    {
        public string CategoryName { get; set; }
    }

    public class CreateCategoryRes
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class UpdateCategoryReq
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

