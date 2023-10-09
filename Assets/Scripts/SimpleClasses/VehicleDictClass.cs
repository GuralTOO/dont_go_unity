using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehicleDictClass {
    public VehicleClass vehicleClass;
    public int cnt = 0;
    public Transform transform;

    public VehicleDictClass(VehicleClass a, int b, Transform c) {
        vehicleClass = a;
        cnt = b;
        transform = c;
    }
}
