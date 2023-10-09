using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DistanceCalculator {

    const long R = 6371;
    public static float Calc(Coordinates x, Coordinates y) {
        float phi1 = x.lat * Mathf.PI / 180;
        float phi2 = y.lat * Mathf.PI / 180;
        float phi = (y.lat - x.lat) * Mathf.PI / 180;

        float delta = (y.lon - x.lon) * Mathf.PI / 180;


        float a = Mathf.Sin(phi / 2) * Mathf.Sin(phi / 2) +
            Mathf.Cos(phi1) * Mathf.Cos(phi2) *
            Mathf.Sin(delta / 2) * Mathf.Sin(delta / 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        return R * c;   //in KM
    }
}
