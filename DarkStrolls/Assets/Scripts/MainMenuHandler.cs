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

    public InputField newUserEmail_Input;
    public InputField newUserPassword_Input;
    public InputField returningUserEmail_Input;
    public InputField returningUserPassword_Input;
    
    public string newUserEmail_Text;
    public string newUserPassword_Text;
    public bool newUserSuccess;
    public string returningUserEmail_Text;
    public string returningUserPassword_Text;
    public bool returningUserSuccess;

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
    
    public void CreateUserAction()
    {
        newUserEmail_Text = newUserEmail_Input.text;
        newUserPassword_Text = newUserPassword_Input.text;

        var userRequest = new CreateUserRequest();
        userRequest.Username = newUserEmail_Text;
        string requestBody = JsonConvert.SerializeObject(userRequest);
        StartCoroutine(postRequest("createuser", requestBody, rawBody =>
        {
            try
            {
                Debug.Log(rawBody);
                var response = JsonConvert.DeserializeObject<UserResponse>(rawBody);

                if (response is null || response.Username is null)
                {
                    newUserSuccess = false;
                }
                else
                {
                    newUserSuccess = true;
                }
            }
            catch(JsonReaderException)
            {
                newUserSuccess = false;
            }
            

            var colorBlock = newUserEmail_Input.colors;

            if (newUserSuccess)
            {
                colorBlock.normalColor = Color.green;
            }
            else
            {
                colorBlock.normalColor = Color.red;
            }

            newUserEmail_Input.colors = colorBlock;
        }));

        Invoke("SceneInvoke", 1f);
    }
    
    public void LoginUserAction()
    {
        returningUserEmail_Text = returningUserEmail_Input.text;
        returningUserPassword_Text = returningUserPassword_Input.text;

        var userRequest = new GetUserRequest();
        userRequest.Usernames = new string[] { returningUserEmail_Text };
        string requestBody = JsonConvert.SerializeObject(userRequest);
        StartCoroutine(postRequest("getusers", requestBody, rawBody => 
        {
            try
            {
                if(rawBody == "User exists!")
                {
                    returningUserSuccess = false;
                }
                else
                {
                    var response = JsonConvert.DeserializeObject<UserResponse[]>(rawBody);

                    if (response is null || response.Length == 0 || response[0].Username is null)
                    {
                        returningUserSuccess = false;
                    }
                    else
                    {
                        returningUserSuccess = true;
                    }
                }
            }
            catch(JsonReaderException)
            {
                returningUserSuccess = false;
            }
            

            var colorBlock = returningUserEmail_Input.colors;

            if (returningUserSuccess)
            {
                colorBlock.normalColor = Color.green;
            }
            else
            {
                colorBlock.normalColor = Color.red;
            }

            returningUserEmail_Input.colors = colorBlock;
        }));
        
        Invoke("SceneInvoke", 1f);
    }

    public void ReturnToMainMenu()
    {
        setActivePanel(mainMenuPanel);
    }

    public void SceneInvoke()
    {
        if (returningUserSuccess == true && returningUserEmail_Input.colors.normalColor == Color.green)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        if (newUserSuccess == true && newUserEmail_Input.colors.normalColor == Color.green)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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


    private IEnumerator postRequest(string endpoint, string request, Action<string> after)
    {
        var uwr = new UnityWebRequest("https://keweenawcheese.com/darkstrolls/" + endpoint, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(request);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();



        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            after.Invoke(uwr.downloadHandler.text);
        }

        uwr.Dispose();
    }



    private class UserResponse
    {
        public int Id { get; set; }

        public int Souls { get; set; }

        public string Username { get; set; }
    }

    private class GetUserRequest
    {
        public string[] Usernames { get; set; }

        public int[] UserIds { get; set; }
    }
    
    private class CreateUserRequest
    {
        public string Username { get; set; }
    }
}
