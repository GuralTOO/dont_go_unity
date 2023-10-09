using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;

public class MoveManager : MonoBehaviour
{
    Vector3 touchStart;
    public float minZoom = 2;
    public float maxZoom = 350;
    Camera cam;
    AbstractMap _map;
    public GameObject randomGameObject;
    public MapSpawner mapSpawner;

    // Use this for initialization
    void Start() {
        cam = Camera.main;
        mapSpawner = FindObjectOfType<MapSpawner>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);   //save the position where the screen was initially pressed
        }
        if (Input.touchCount == 2) {
            Touch touchZero = Input.GetTouch(0);        //get touch 0 & 1
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;    //gather the last frame's touch location
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;   //get the distance or magnitude between the last two touches 
            float currMagnitude = (touchZero.position - touchOne.position).magnitude;   //get the distance between the current two touches
            float difference = currMagnitude - prevMagnitude;

            Zoom(difference * 0.2f);
        }
        else if (Input.GetMouseButton(0)) {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
        mapSpawner.ManualUpdate();
    }

    private void Zoom(float increment) {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, minZoom, maxZoom);
    }


}
