namespace Domain;

public class Placeholder(string value = "This is a placeholder from Domain.")
{
    public string Value { get; set; } = value;
}
