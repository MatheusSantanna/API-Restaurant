namespace backend.DTO;

public class CategoryDTO
{
    public int id { get; set; }
    public string Name { get; set; }
    public List<string> Products { get; set; }
}