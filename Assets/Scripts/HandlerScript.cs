using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    public void dodo(string a, List<string> aA, int bB) {
        InterpreterBase b = transform.GetComponent (a) as InterpreterBase;
        Debug.Log("Dodo received request for: " + a + "   " + aA[0]);
        if (b != null) {
            b.enabled = true;
            b.RetrieveVehicles(aA, bB);
            Debug.Log("and found it");
        }
        else
            Debug.Log("but ain't find it");
    }
}
