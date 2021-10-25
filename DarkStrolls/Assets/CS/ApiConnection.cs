using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

/// <summary>
/// Class to use for connecting to the API.
/// </summary>
public class ApiConnection
{

    /// <summary>
    /// Creates a post request to the given endpoint at the programmed server url and follows up with
    /// a given action to process it.
    /// </summary>
    /// <param name="endpoint">Endpoint at the server to use.</param>
    /// <param name="request">Request to send the server.</param>
    /// <param name="after">Action to take after data is recieved.</param>
    /// <returns></returns>
    public IEnumerator PostRequest(string endpoint, string request, Action<string> after)
    {
        // Create the web request at the endpoint of type POST.
        var uwr = new UnityWebRequest("https://keweenawcheese.com/darkstrolls/" + endpoint, "POST");
        
        //Convert the json into bytes so we can send it.
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(request);
        // Create the upload handler to send the data.
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        // Create the download handler to get the return data.
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // Set the request header to send json.
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();



        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            // Invoke the return function.
            after.Invoke(uwr.downloadHandler.text);
        }

        // Dispose of the web request.
        uwr.Dispose();
    }
}
