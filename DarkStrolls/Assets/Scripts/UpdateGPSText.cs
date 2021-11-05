using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGPSText : MonoBehaviour
{
    public Text coordinates;

    // Update is called once per frame
    void Update()
    {
        coordinates.text = "Latitude: "  +  GPS.Instance.latitude.ToString() + "\nLongitude: " + GPS.Instance.longitude.ToString();
    }
}
