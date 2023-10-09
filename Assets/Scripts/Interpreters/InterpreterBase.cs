using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class InterpreterBase: MonoBehaviour {

    public abstract void RetrieveVehicles(List<string> url_links, int x);

    
}
