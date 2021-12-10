using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadMessagesBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    public GameObject TextComponent;

    public void OpenMessage(string message)
    {
        TextComponent.GetComponent<TextMeshProUGUI>().text = message;
        this.gameObject.SetActive(true);
    }

    public void CloseMessage()
    {
        gameObject.SetActive(false);
    }
}
