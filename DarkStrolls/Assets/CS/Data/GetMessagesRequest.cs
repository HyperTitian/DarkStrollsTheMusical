

/// <summary>
/// Class for sending a request to retrieve all messages
/// </summary>
public class GetMessagesRequest
{
    /// <summary>
    /// Latitude of the player
    /// </summary>
    public double Latitude { get; set; }
    
    /// <summary>
    /// Longitude of the player
    /// </summary>
    public double Longitude { get; set; }
    
    /// <summary>
    /// Range of visible messages
    /// </summary>
    public double Range { get; set; }
}