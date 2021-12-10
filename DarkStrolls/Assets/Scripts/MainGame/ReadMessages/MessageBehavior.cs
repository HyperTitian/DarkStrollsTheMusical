using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject ReadMessagesMenu;

    public string Message = "Cheese";

    public GameObject MessagePrefab;

    void OnMouseDown()
    {
        //Object.Destroy(this.gameObject);
        ReadMessagesMenu.GetComponent<ReadMessagesBehavior>().OpenMessage(Message);
    }

    public GameObject CreateMessage(Vector3 position, string message)
    {
        GameObject x = Instantiate(MessagePrefab, MessagePrefab.transform, true);
        x.transform.position = position;
        x.GetComponent<MessageBehavior>().Message = message;
        x.GetComponent<MessageBehavior>().MessagePrefab = MessagePrefab;
        x.GetComponent<MessageBehavior>().ReadMessagesMenu = ReadMessagesMenu;

        x.SetActive(true);
        

        return x;
    }
}
