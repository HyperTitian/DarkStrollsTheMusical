using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for sending a request to create a new user.
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// Username of the user you would like to create.
    /// </summary>
    public string Username { get; set; }
}
