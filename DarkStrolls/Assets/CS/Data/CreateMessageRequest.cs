using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for sending a request to create a message.
/// </summary>
public class CreateMessageRequest
{
    /// <summary>
    /// User ID of the user you would like to create the message.
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// Username of the user you would like to create the message.
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// Text in the message object you would like to create the message entry.
    /// </summary>
    public string Text { get; set; }
    
    /// <summary>
    /// Longitude of the message location you would like to create the message at.
    /// </summary>
    public string Longitude { get; set; }
    
    /// <summary>
    /// Latitude of the message location you would like to create the message at.
    /// </summary>
    public string Latitude { get; set; }
}
