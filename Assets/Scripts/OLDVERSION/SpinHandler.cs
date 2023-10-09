using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
//using UnityEngine.JSONSerializeModule;


public class SpinHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI text;
    public string Auth1Token;
    public string SpinAuth2URL;
    string JSONString;

    public List<Coordinates> spinBikesinDC = new List<Coordinates>();


    public void Auth2() {

        StartCoroutine(GetAllVehichles());
    }

    IEnumerator GetAllVehichles() {
        UnityWebRequest getDCVehichles = UnityWebRequest.Get("https://s3.amazonaws.com/lyft-lastmile-production-iad/lbs/dca/free_bike_status.json");
        yield return getDCVehichles.SendWebRequest();
        if (getDCVehichles.isNetworkError || getDCVehichles.isHttpError)
            Debug.LogError(getDCVehichles.error);
        else
            Debug.Log("DC Spin Vehichles retrieved");
        JSONNode spinDCJSON1 = JSON.Parse(getDCVehichles.downloadHandler.text);
        JSONNode spinDCJSON2 = spinDCJSON1["data"]["bikes"];

        for (int i = 0; i < spinDCJSON2.Count; i++) {
            spinBikesinDC.Add(new Coordinates(spinDCJSON2[i]["lat"], spinDCJSON2[i]["lon"]));
        }
        Debug.Log("Printing Lyft DC bikes");        
        for(int i=0; i < spinBikesinDC.Count; i++) {
            Debug.Log(spinBikesinDC[i].lat + " " + spinBikesinDC[i].lon);
        }
    }


}
