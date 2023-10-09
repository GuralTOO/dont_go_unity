using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Coordinates{
    public float lat;
    public float lon;

    public Coordinates(float a, float b) {
        lat = a;
        lon = b;
    }
}
