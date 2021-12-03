using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to store message data.
/// </summary>
public class Message
{
    /// <summary>
    /// Id of the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Username of the user.
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// Text in the message.
    /// </summary>
    public string Text { get; set; }
    
    /// <summary>
    /// Longitude of the message.
    /// </summary>
    public string Longitude { get; set; }
    
    /// <summary>
    /// Latitude of the message.
    /// </summary>
    public string Latitude { get; set; }
    
}