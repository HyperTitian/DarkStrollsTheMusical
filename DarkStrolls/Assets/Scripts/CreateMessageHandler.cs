using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class CreateMessageHandler : MonoBehaviour
{
    // Input fields.
    public InputField message;

    // Error output field.
    public Text errorText;
    
    // Gamemanger
    public GameObject gameManager;

    // Variable to store user's data once it is retrieved.
    private Message messageData;

    public void CreateMessageAction()
    {
        string messageText = message.text;
        
        // Verify that the message is legal.
        if (string.IsNullOrWhiteSpace(messageText))
        {
            errorText.text = "You must enter a valid message!";
            return;
        }
        
        // Create the request and convert it to JSON.
        var userRequest = new CreateUserRequest();
        var messageRequest = new CreateMessageRequest();
        messageRequest.Username = userRequest.Username;
        // messageRequest.UserId = ""; // not yet implemented
        messageRequest.Text = messageText;
        messageRequest.Latitude = GPS.Instance.latitude.ToString();
        messageRequest.Longitude = GPS.Instance.longitude.ToString();
        
        string requestBody = JsonConvert.SerializeObject(messageRequest);
        
        
        // Create the API connection and start it.
        var apiConnection = new ApiConnection();
        StartCoroutine(apiConnection.PostRequest("createmessage", requestBody, rawBody =>
        {
            // Parse whether we succeeded.
            var newMessageSuccess = false;
            try
            {
                var response = JsonConvert.DeserializeObject<Message>(rawBody);

                if (response is null || response.Text is null)
                {
                    newMessageSuccess = false;
                    errorText.text = "Failed to create message!";
                }
                else
                {
                    newMessageSuccess = true;
                    messageData = response;
                }
            }
            catch (JsonReaderException)
            {
                newMessageSuccess = false;
                errorText.text = "Failed to create message!";
            }

            // Call the correct function depending on success.
            if (newMessageSuccess)
            {
                createMessageSucceeded();
            }
            else
            {
                createMessageFailed();
            }
        }));
    }
    
    /// <summary>
    /// Called when the user creation fails.
    /// </summary>
    private void createMessageFailed()
    {
        // Set the username field to red.
        var colorBlock = message.colors;
        colorBlock.normalColor = Color.red;
        message.colors = colorBlock;
    }

    /// <summary>
    /// Called when the user creation succeeds.
    /// </summary>
    private void createMessageSucceeded()
    {
        // Set the username field to green.
        var colorBlock = message.colors;
        colorBlock.normalColor = Color.green;
        message.colors = colorBlock;
        gameManager.GetComponent<GameManager>().SubmitMessage();
        gameManager.GetComponent<SpawnMessage>().SpawnMessageAtPlace();
    }
}
