using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject newUserPanel;
    public GameObject returningUserPanel;

    public InputField newUserEmail_Input;
    public InputField newUserPassword_Input;
    public InputField returningUserEmail_Input;
    public InputField returningUserPassword_Input;
    
    public string newUserEmail_Text;
    public string newUserPassword_Text;
    public string returningUserEmail_Text;
    public string returningUserPassword_Text;

    public void NewUserAction()
    {
        mainMenuPanel.SetActive(false);
        newUserPanel.SetActive(true);
    }

    public void ReturningUserAction()
    {
        mainMenuPanel.SetActive(false);
        returningUserPanel.SetActive(true);
    }

    public void CreateUserAction()
    {
        newUserEmail_Text = newUserEmail_Input.text;
        newUserPassword_Text = newUserPassword_Input.text;
    }
    
    public void LoginUserAction()
    {
        returningUserEmail_Text = returningUserEmail_Input.text;
        returningUserPassword_Text = returningUserPassword_Input.text;
    }
}
