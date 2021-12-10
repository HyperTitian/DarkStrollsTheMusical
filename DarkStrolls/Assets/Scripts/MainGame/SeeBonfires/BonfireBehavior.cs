using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireBehavior : MonoBehaviour
{

    public string BonfireName = "Cheese";

    public GameObject BonfirePrefab;

    public GameObject ReadMessagesMenu;

    void OnMouseDown()
    {
        //Object.Destroy(this.gameObject);
        ReadMessagesMenu.GetComponent<ReadMessagesBehavior>().OpenMessage($"Bonfire {BonfireName}");
    }

    public GameObject CreateBonfire(Vector3 position, string name)
    {
        GameObject x = Instantiate(BonfirePrefab, BonfirePrefab.transform, true);
        x.transform.position = position;
        x.GetComponent<BonfireBehavior>().BonfireName = name;
        x.GetComponent<BonfireBehavior>().BonfirePrefab = BonfirePrefab;
        x.GetComponent<BonfireBehavior>().ReadMessagesMenu = ReadMessagesMenu;

        x.SetActive(true);


        return x;
    }
}
