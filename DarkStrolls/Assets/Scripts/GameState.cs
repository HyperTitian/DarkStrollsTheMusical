using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static User CurrentUser { get; set; }
    
    public static GameObject[] MessageObjects { get; set; }

    public static GameObject CurrentMessage { get; set; }

    public static Bonfire[] BonfireArray { get; set; }
}
