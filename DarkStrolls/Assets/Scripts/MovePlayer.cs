using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    void Update()
    {
        //transform.position = Quaternion.AngleAxis(GPS.Instance.longitude, -Vector3.up) * Quaternion.AngleAxis(GPS.Instance.latitude, -Vector3.right) * new Vector3(0,0,200);
        transform.position = new Vector3(GPS.Instance.longitude, 200, GPS.Instance.latitude);
    }
}
