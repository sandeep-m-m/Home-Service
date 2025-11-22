using System.ComponentModel.DataAnnotations;

public class SampleData
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }
    public string Genre { get; set; }
    public int Rating {get;set;}
    public string Description { get; set; }
    public decimal Price { get; set; }

}