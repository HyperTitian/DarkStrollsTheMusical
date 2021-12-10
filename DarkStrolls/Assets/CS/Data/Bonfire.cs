

/// <summary>
/// A bonfire as seen in the database.
/// </summary>
public class Bonfire
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double Longitude { get; set; }

    public double Latitude { get; set; }
}
