using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject leaveMessagePanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private float loadingTimer = 12f;
    [SerializeField] private Text loadingText;
    
    public bool isBeginning = true;

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Invoke("BackToMenuResetVars", 5f);
    }

    public void LeaveAMessage()
    {
        leaveMessagePanel.SetActive(true);
    }

    public void SubmitMessage()
    {
        leaveMessagePanel.SetActive(false);
    }

    void Update()
    {
        if (isBeginning == true)
        {
            if (loadingTimer > 0f)
            {
                if ((int) loadingTimer % 4 == 1)
                    loadingText.text = "";
                else if ((int) loadingTimer % 4 == 2)
                    loadingText.text = ".";
                else if ((int) loadingTimer % 4 == 3)
                    loadingText.text = "..";
                else if ((int) loadingTimer % 4 == 0)
                    loadingText.text = "...";
                    
                loadingTimer = loadingTimer - Time.deltaTime;
            }
            else
            {
                loadingTimer = 12f;
                isBeginning = false;
                loadingPanel.SetActive(false);
                mainPanel.SetActive(true);
            }
        }
    }

    private void BackToMenuResetVars()
    {
        loadingPanel.SetActive(true);
        mainPanel.SetActive(false);
        isBeginning = true;
    }
}
