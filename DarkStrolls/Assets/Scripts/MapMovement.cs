using System;
using Google.Maps;
using Google.Maps.Coord;
using Google.Maps.Event;
using Google.Maps.Examples.Shared;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public LatLng LatLng = new LatLng(47.1191, -88.5468);

    public MapsService mapsService;

    // Start is called before the first frame update
    void Start()
    {
        mapsService = GetComponent<MapsService>();
        mapsService.InitFloatingOrigin(LatLng);
        mapsService.Events.MapEvents.Loaded.AddListener(OnLoaded);
        
        mapsService.LoadMap(ExampleDefaults.DefaultBounds, ExampleDefaults.DefaultGameObjectOptions);
    }

    private void Update()
    {
        LatLng = new LatLng(GPS.Instance.latitude, GPS.Instance.longitude);
        mapsService.MoveFloatingOrigin(LatLng);
    }

    public void OnLoaded(MapLoadedArgs args) {
        // The Map is loaded - you can start/resume gameplay from that point.
        // The new geometry is added under the GameObject that has MapsService as a component.
    }
}
