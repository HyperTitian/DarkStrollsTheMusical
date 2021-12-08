using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageRadiusChecker : MonoBehaviour
{

    public Transform messageChecker;
    public float messageDistance = 0.4f;
    public LayerMask messageLayer;
    public bool isInRadius = false;
    
    public Message[] messages;

    void Start()
    {
        
    }
    
    
    // void Update()
    // {
    //     // messages = GameObject.FindGameObjectsWithTag("Message");
    //     messages[0] = GameObject.FindGameObjectsWithTag("Message");
    //     
    //     isInRadius = Physics.CheckSphere(messageChecker.position, messageDistance, messageLayer);
    //
    //     if (isInRadius == true)
    //     {
    //         
    //     }
    // }
}
