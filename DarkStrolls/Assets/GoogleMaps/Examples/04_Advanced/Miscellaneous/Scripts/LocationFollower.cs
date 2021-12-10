using System;
using System.Collections;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
using Google.Maps.Coord;
using Google.Maps.Examples.Shared;
using Newtonsoft.Json;
using UnityEngine.UI;

namespace Google.Maps.Examples
{
    /// <summary>
    /// Example showing how to have the player's real-world GPS location reflected by on-screen
    /// movements.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="ErrorHandling"/> component to display any errors encountered by the
    /// <see cref="MapsService"/> component when loading geometry.
    /// </remarks>
    [RequireComponent(typeof(MapsService), typeof(ErrorHandling))]
    public class LocationFollower : MonoBehaviour
    {
        // Dialog for location services request.
#if PLATFORM_ANDROID
        GameObject dialog = null;
#endif

        public GameObject messageBlueprint;
        public GameObject bonfireHandler;

        /// <summary>
        /// The maps service.
        /// </summary>
        private MapsService MapsService;

        /// <summary>
        /// The player's last recorded real-world location and the the current floating origin.
        /// </summary>
        private LatLng PreviousLocation;

        private DateTime lastUpdate = DateTime.Now;


        /// <summary>Start following player's real-world location.</summary>
        private void Start()
        {
            GetPermissions();
            StartCoroutine(Follow());
        }

        /// <summary>
        /// Follow player's real-world location, as derived from the device's GPS signal.
        /// </summary>
        private IEnumerator Follow()
        {
            // If location is allowed by the user, start the location service and compass, otherwise abort
            // the coroutine.
#if PLATFORM_IOS
      // The location permissions request in IOS does not seem to get invoked until it is called for
      // in the code. It happens at runtime so if the code is not trying to access the location
      // right away, it will not pop up the permissions dialog.
        Input.location.Start();
      
#endif
            while (!Input.location.isEnabledByUser)
            {
                Debug.Log("Waiting for location services to become enabled..");
                yield return new WaitForSeconds(1f);
            }
            Debug.Log("Location services is enabled.");

#if !PLATFORM_IOS
            Input.location.Start();
#endif

            Input.compass.enabled = true;

            // Wait for the location service to start.
            while (true)
            {
                if (Input.location.status == LocationServiceStatus.Initializing)
                {
                    // Starting, just wait.
                    yield return new WaitForSeconds(1f);
                }
                else if (Input.location.status == LocationServiceStatus.Failed)
                {
                    // Failed, abort the coroutine.
                    Debug.LogError("Location Services failed to start.");

                    yield break;
                }
                else if (Input.location.status == LocationServiceStatus.Running)
                {
                    // Started, continue the coroutine.
                    break;
                }
            }

            // Get the MapsService component and load it at the device location.
            PreviousLocation =
                new LatLng(Input.location.lastData.latitude, Input.location.lastData.longitude);
            MapsService = GetComponent<MapsService>();
            MapsService.InitFloatingOrigin(PreviousLocation);
            MapsService.LoadMap(ExampleDefaults.DefaultBounds, ExampleDefaults.DefaultGameObjectOptions);
        }

        /// <summary>
        /// Moves the camera and refreshes the map as the player moves.
        /// </summary>
        private void Update()
        {
            if (MapsService == null)
            {
                return;
            }

            // Get the current map location.
            LatLng currentLocation =
                new LatLng(Input.location.lastData.latitude, Input.location.lastData.longitude);
            Vector3 currentWorldLocation = MapsService.Projection.FromLatLngToVector3(currentLocation);

            // Move the camera to the current map location.
            Vector3 targetCameraPosition = new Vector3(currentWorldLocation.x,
                Camera.main.transform.position.y, currentWorldLocation.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                targetCameraPosition, (Time.deltaTime)); // (Time.deltaTime / 6)





            // Only move the map location if the device has moved more than 2 meters.
            if (DateTime.Now - lastUpdate >= TimeSpan.FromSeconds(10))
            {
                lastUpdate = DateTime.Now;
                MapsService.MoveFloatingOrigin(currentLocation, new[] { Camera.main.gameObject });
                MapsService.LoadMap(ExampleDefaults.DefaultBounds,
                    ExampleDefaults.DefaultGameObjectOptions);
                PreviousLocation = currentLocation;
                
                RetrieveMessages(GPS.Instance.longitude, GPS.Instance.latitude);
                RetrieveBonfires(GPS.Instance.longitude, GPS.Instance.latitude);

            }

            //transform.rotation = Quaternion.Euler(0, -Input.compass.magneticHeading, 0);
            Camera.main.transform.rotation = Quaternion.Euler(90, Input.compass.trueHeading, 0);

        }

        private void GetPermissions()
        {
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
                dialog = new GameObject();
            }
#endif // Android
        }

        void OnGUI()
        {
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                // The user denied permission to use location information.
                // Display a message explaining why you need it with Yes/No buttons.
                // If the user says yes then present the request again
                // Display a dialog here.
                dialog.AddComponent<PermissionsRationaleDialog>();
                return;
            }
            else if (dialog != null)
            {
                Destroy(dialog);
            }
#endif
        }

        private void RetrieveMessages(double longitude, double latitude)
        {
            var userRequest = new GetMessagesRequest();
            userRequest.Latitude = latitude;
            userRequest.Longitude = longitude;
            userRequest.Range = 0.0024;
            string requestBody = JsonConvert.SerializeObject(userRequest);
            // Create the API connection and start it.
            var apiConnection = new ApiConnection();
            StartCoroutine(apiConnection.PostRequest("getmessages", requestBody, rawBody =>
            {
                // Parse whether we succeeded.
                try
                {
                    var response = JsonConvert.DeserializeObject<Message[]>(rawBody);
                    if (response != null)
                    {
                        GameObject[] stuff = GameState.MessageObjects;
                        GameState.MessageObjects = null;
                        if(stuff != null)
                        {
                            for(int i = 0; i < stuff.Length; i++)
                            {
                                Destroy(stuff[i]);
                                stuff[i] = null;
                            }
                        }
                        if(GameState.CurrentMessage != null)
                        {
                            Destroy(GameState.CurrentMessage);
                            GameState.CurrentMessage = null;
                        }
                        var x = new GameObject[response.Length];


                        // place new message at lat and long -- for each loop

                        for (int i = 0; i < response.Length; i++)
                        {
                            var message = response[i];
                            LatLng coords = new LatLng(message.Latitude, message.Longitude);
                            Vector3 worldCoords = MapsService.Projection.FromLatLngToVector3(coords);
                            x[i] = messageBlueprint.GetComponent<MessageBehavior>().CreateMessage(worldCoords, message.Text);
                            Destroy(x[i], 12);
                        }
                        GameState.MessageObjects = x;
                    }
                    else
                    {

                    }
                }
                catch (JsonReaderException)
                {
                    // do nothing
                }
            }));
        }


        private void RetrieveBonfires(double longitude, double latitude)
        {
            var userRequest = new GetMessagesRequest();
            userRequest.Latitude = latitude;
            userRequest.Longitude = longitude;
            userRequest.Range = 0.024;
            string requestBody = JsonConvert.SerializeObject(userRequest);
            // Create the API connection and start it.
            var apiConnection = new ApiConnection();
            StartCoroutine(apiConnection.PostRequest("getbonfires", requestBody, rawBody =>
            {
                // Parse whether we succeeded.
                try
                {
                    var response = JsonConvert.DeserializeObject<Bonfire[]>(rawBody);
                    if (response != null)
                    {
                        GameObject[] stuff = GameState.BonfireObjects;
                        GameState.BonfireObjects = null;
                        if (stuff != null)
                        {
                            for (int i = 0; i < stuff.Length; i++)
                            {
                                Destroy(stuff[i]);
                                stuff[i] = null;
                            }
                        }
                        var x = new GameObject[response.Length];


                        // place new message at lat and long -- for each loop

                        for (int i = 0; i < response.Length; i++)
                        {
                            var fire = response[i];
                            LatLng coords = new LatLng(fire.Latitude, fire.Longitude);
                            Vector3 worldCoords = MapsService.Projection.FromLatLngToVector3(coords);
                            x[i] = bonfireHandler.GetComponent<BonfireBehavior>().CreateBonfire(worldCoords, fire.Name);
                            Destroy(x[i], 12);
                        }
                        GameState.MessageObjects = x;
                    }
                    else
                    {

                    }
                }
                catch (JsonReaderException)
                {
                    // do nothing
                }
            }));
        }
    }
}
