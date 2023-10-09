using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehicleClass {
    public float lat;
    public float lon;
    public int type;
    //type = 0 -> scooter
    //type = 1 -> bicycle
    //type = 2+ -> ?
    public string company;
    public string id;

    public VehicleClass(float a, float b, int c, string d, string e) {
        lat = a;
        lon = b;
        type = c;
        company = d;
        id = e;
    }
}
