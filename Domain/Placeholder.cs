namespace Domain;

public class Placeholder
{
    public string Value { get; set; } = null!;

    public Placeholder(string value = "This is a placeholder from Domain.") => Value = value;
}
