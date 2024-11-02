namespace Dogs.Core.Models;
public class Dog
{
    private Dog(string name, string color , int tailLength, int weight)
    {
        Name = name;
        Color = color;
        TailLength = tailLength;
        Weight = weight;
    }
    public string Name { get;} = default!;
    public string Color { get;} = default!;
    public int TailLength { get;}
    public int Weight { get;}

    public static Dog Create(string name, string color, int tailLength, int weight)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        }
        if (string.IsNullOrWhiteSpace(color))
        {
            throw new ArgumentException("Color cannot be null or empty", nameof(color));
        }
        if (tailLength <= 0)
        {
            throw new ArgumentException("Tail length must be greater than 0", nameof(tailLength));
        }
        if (weight <= 0)
        {
            throw new ArgumentException("Weight must be greater than 0", nameof(weight));
        }
        return new Dog(name, color, tailLength, weight);
    }
}
