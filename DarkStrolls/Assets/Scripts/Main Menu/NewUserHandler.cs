using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

/// <summary>
/// Script to handle creating a new user.
/// </summary>
public class NewUserHandler : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
      // Canvas.GetComponent<MainMenuHandler>().Se
    }

    /// <summary>
    /// Called when the username and password have been entered.
    /// </summary>
    public void CreateUserAction()
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
        var userRequest = new CreateUserRequest();
        userRequest.Username = username;
        userRequest.Password = password;
        string requestBody = JsonConvert.SerializeObject(userRequest);

        // Create the API connection and start it.
        var apiConnection = new ApiConnection();
        StartCoroutine(apiConnection.PostRequest("createuser", requestBody, rawBody =>
        {
            // Parse whether we succeeded.
            var newUserSuccess = false;
            try
            {
                var response = JsonConvert.DeserializeObject<User>(rawBody);

                if (response is null || response.Username is null)
                {
                    newUserSuccess = false;
                    ErrorText.text = "Failed to create user!";
                }
                else
                {
                    newUserSuccess = true;
                    userData = response;
                }
            }
            catch (JsonReaderException)
            {
                newUserSuccess = false;
                ErrorText.text = "Failed to create user!";
            }

            // Call the correct function depending on success.
            if (newUserSuccess)
            {
                createUserSucceeded();
            }
            else
            {
                createUserFailed();
            }
        }));
    }

    /// <summary>
    /// Called when the user creation fails.
    /// </summary>
    private void createUserFailed()
    {
        // Set the username field to red.
        var colorBlock = Username.colors;
        colorBlock.normalColor = Color.red;
        Username.colors = colorBlock;
    }

    /// <summary>
    /// Called when the user creation succeeds.
    /// </summary>
    private void createUserSucceeded()
    {
        // Set the username field to green.
        var colorBlock = Username.colors;
        colorBlock.normalColor = Color.green;
        Username.colors = colorBlock;
        GameState.CurrentUser = userData;
        Canvas.GetComponent<MainMenuHandler>().LoadNextScene();
    }
}
