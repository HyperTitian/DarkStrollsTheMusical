using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    /// <summary>
    /// Globally available panels.
    /// </summary>
    public GameObject mainMenuPanel;
    public GameObject newUserPanel;
    public GameObject returningUserPanel;

    // The currently active panel.
    private GameObject activePanel = null;

    void Start()
    {
        mainMenuPanel.SetActive(true);
        newUserPanel.SetActive(false);
        returningUserPanel.SetActive(false);
        activePanel = mainMenuPanel;
    }

    public void NewUserAction()
    {
        setActivePanel(newUserPanel);
    }

    public void ReturningUserAction()
    {
        setActivePanel(returningUserPanel);
    }

    public void ReturnToMainMenu()
    {
        setActivePanel(mainMenuPanel);
    }

    public void LoadNextScene()
    {
        //SceneManager.LoadScene("MainGame");
        SceneManager.LoadScene("MapsServiceTest");
    }

    private void setActivePanel(GameObject panel)
    {
        if(activePanel != null)
        {
            activePanel.SetActive(false);
        }

        activePanel = panel;
        activePanel.SetActive(true);
    }
    
}
