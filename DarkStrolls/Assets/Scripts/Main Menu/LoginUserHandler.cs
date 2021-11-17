using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

/// <summary>
/// Script to handle logging in a user.
/// </summary>
public class LoginUserHandler : MonoBehaviour
{
    // Main canvas.
    public Canvas Canvas;

    // Input fields.
    public InputField Username;
    public InputField Password;

    // Error output field.
    public Text ErrorText;

    // Variable to store user's data once it is retrieved.
    private User userData;

    /// <summary>
    /// Called when the username and password have been entered.
    /// </summary>
    public void LoginUserAction()
    {
        // Get the username and password.
        string username = Username.text;
        string password = Password.text;

        // Verify they have been entered correctly.
        if (string.IsNullOrWhiteSpace(username))
        {
            ErrorText.text = "You must enter a valid username!";
            return;
        }

        // Create the request and convert it to JSON.
        var userRequest = new LoginRequest();
        userRequest.Username = username;
        userRequest.Password = password;
        
        string requestBody = JsonConvert.SerializeObject(userRequest);

        // Create the API connection and start it.
        var apiConnection = new ApiConnection();
        StartCoroutine(apiConnection.PostRequest("getusers", requestBody, rawBody =>
        {
            // Parse whether we succeeded.
            var newUserSuccess = false;
            try
            {
                var response = JsonConvert.DeserializeObject<User>(rawBody);

                if (response is null || response.Username is null)
                {
                    ErrorText.text = "Failed to login!";
                    newUserSuccess = false;
                }
                else
                {
                    newUserSuccess = true;
                    userData = response;
                }
            }
            catch (JsonReaderException)
            {
                ErrorText.text = "Failed to login!";
                newUserSuccess = false;
            }

            // Call the correct function depending on success.
            if (newUserSuccess)
            {
                loginUserSucceeded();
            }
            else
            {
                loginUserFailed();
            }
        }));

        Invoke("SceneInvoke", 1f);
    }

    /// <summary>
    /// Called when the user creation fails.
    /// </summary>
    private void loginUserFailed()
    {
        // Set the username field to red.
        var colorBlock = Username.colors;
        colorBlock.normalColor = Color.red;
        Username.colors = colorBlock;
    }

    /// <summary>
    /// Called when the user creation succeeds.
    /// </summary>
    private void loginUserSucceeded()
    {
        // Set the username field to green.
        var colorBlock = Username.colors;
        colorBlock.normalColor = Color.green;
        Username.colors = colorBlock;
        GameState.CurrentUser = userData;
        Canvas.GetComponent<MainMenuHandler>().LoadNextScene();
    }
}
