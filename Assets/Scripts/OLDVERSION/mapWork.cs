using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.Location;

public class mapWork : MonoBehaviour
{
    //get unity coordinates of camera corners
    //convert coordinates to latitude and long
    //send the coordinates to APIcontroller

    private Camera cam;
    
    public GameObject point;

    private void Start() {
        cam = Camera.main;
    }

    public class Corners {
        public Vector3 upperLeft;
        public Vector3 upperRight;
        public Vector3 lowerLeft;
        public Vector3 lowerRight;

        public Corners(Vector3 a, Vector3 b, Vector3 c, Vector3 d) {
            upperLeft = a;
            lowerLeft = b;
            upperRight = c;
            lowerRight = d;
        }
    }

    Corners GetCameraCorners() {
        Vector2 upperLeft;
        Vector2 lowerLeft;
        Vector2 upperRight;
        Vector2 lowerRight;

        Vector3 upperLeftWorld;
        Vector3 lowerRightWorld;
        Vector3 lowerLeftWorld;
        Vector3 upperRightWorld;

        upperLeft.x = 0;
        upperLeft.y = cam.pixelHeight;
        lowerLeft.x = 0;
        lowerLeft.y = 0;

        upperRight.x = cam.pixelWidth;
        upperRight.y = cam.pixelHeight;
        lowerRight.x = cam.pixelWidth;
        lowerRight.y = 0;

        upperLeftWorld = cam.ScreenToWorldPoint(new Vector3(upperLeft.x, upperLeft.y, cam.nearClipPlane));
        //upperLeftWorld.y = 0;
        lowerRightWorld = cam.ScreenToWorldPoint(new Vector3(lowerRight.x, lowerRight.y, cam.nearClipPlane));
        //lowerRightWorld.y = 0;
        lowerLeftWorld = cam.ScreenToWorldPoint(new Vector3(lowerLeft.x, lowerLeft.y, cam.nearClipPlane));
        //lowerLeftWorld.y = 0;
        upperRightWorld = cam.ScreenToWorldPoint(new Vector3(upperRight.x, upperRight.y, cam.nearClipPlane));
        //lowerRightWorld.y = 0;

        Corners myCorners = new Corners(upperLeftWorld, lowerLeftWorld, upperRightWorld, lowerRightWorld);
        return myCorners;
    }

    public void getMapCorners() {
        Mapbox.Utils.Vector2d myPos = Conversions.GeoFromGlobePosition(new Vector3(1, 5, 1), 6371000);
        Debug.Log(myPos);
/*        Corners myCorners = GetCameraCorners();
        float radius = 15;
        Vector3 whiteHouse = Conversions.GeoToWorldGlobePosition(38.8977, 77.0365, 15);
        Debug.Log(whiteHouse);
        Instantiate(point, whiteHouse, point.transform.rotation);
        Mapbox.Utils.Vector2d myXY = Conversions.GeoFromGlobePosition(whiteHouse, radius);
        Debug.Log(myXY.x + " : " + myXY.y);
        myCorners.upperRight.x--;
        myCorners.upperRight.y--;
        myCorners.upperRight.z--;
        myXY = Conversions.GeoFromGlobePosition(myCorners.upperRight, radius);
        Debug.Log(myXY.x + " : " + myXY.y);
        Debug.Log(myCorners.upperRight);
        //        var location = Conversions.GeoFromGlobePosition(myCorners.upperRight, 1);
        //        Debug.Log(location);
*/
    }
}
