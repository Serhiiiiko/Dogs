namespace Dogs.DataAccess.Entities;
public class DogEntity
{
    public string Name { get; set; } = default!;
    public string Color { get; set; } = default!;
    public int TailLength { get; set; }
    public int Weight { get; set; }
}
