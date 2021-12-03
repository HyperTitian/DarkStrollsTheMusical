using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject leaveMessagePanel;

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LeaveAMessage()
    {
        leaveMessagePanel.SetActive(true);
    }

    public void SubmitMessage()
    {
        leaveMessagePanel.SetActive(false);
    }
}
