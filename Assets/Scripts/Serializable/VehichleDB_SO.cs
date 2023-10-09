using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DictionaryDB", menuName = "ScriptableObjects/Dictionary", order = 1)]
public class VehichleDB_SO : ScriptableObject {

    public Dictionary<string, VehicleDictClass> activeVehiclesDictionary = new Dictionary<string, VehicleDictClass>();

}
