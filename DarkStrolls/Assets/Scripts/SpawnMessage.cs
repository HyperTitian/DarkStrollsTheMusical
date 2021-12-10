using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnMessage : MonoBehaviour
{
    public GameObject messagePrefab;
    
    
    public void SpawnMessageAtPlace()
    {
        Vector3 cameraPos = new Vector3(Camera.main.transform.position.x, 20, Camera.main.transform.position.z);
        
        GameObject x = Instantiate(messagePrefab, cameraPos, Quaternion.identity);
    }
}
