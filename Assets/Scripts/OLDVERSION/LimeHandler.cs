using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using SimpleJSON;

public class LimeHandler: MonoBehaviour {

    private string limebikeURL = "https://web-production.lime.bike/api/rider";
    private string[] limeLoginFields = new string[] { "phone", "%2B14693522537", "login_code", "" };
    private string[] limeVehichleFields = new string[] { "ne_lat", "ne_lng", "sw_lat", "sw_lng", "user_latitude", "user_longitude", "zoom" };
    private string[] limeVehichleFieldAnswers = new string[] { "", "", "", "", "", "", "" };
    private string authKey;
    private static string myAuthAddress = "Lime";
    private string limeCookie = "jbqMuNYG72igDxBKiLIvtnvMZFiq40ZYCGAgpnUHJJKGufNMig%2FEquQfUE866rJ0QszjN%2BAtuXtZMp4OnEdt3sGM9USCbEZ3qoKPYz%2Bj99BFG07%2FnSN%2Fv4g4jzblvYRIUli%2BchrAFlbT5LOeiDsuy3XbUf9ARhPWgvIIz44fV3NT1iUcJu4mjZReuZP%2FcarHkZ7GncJb7hAOA9lOPiu8kDebg0ShHM6YIy46KDzc%2B1%2BZGARyTRXfL2spv1SlwwjfwTyIFVNC%2B6nrYSTp1pz9ye3oY%2Btm9UwxHWgV0vTBAsQa8QiHGOSCB2rUd69TXZ2AHS4UFxU%2B4x5Te1Vqgo0%2BerWozGN8Tys1g9MizJUVt3gHr%2BlQKLau1q5mvpKP64eUOmleX8v8KWWCrOirYDJZUV%2B%2BJeRuMLnxLRaRxU2saARLJfT%2F7piP1MAX7eX2%2BaKRcBYE9G3PDF2Z6JqlbiPHOgfVRJ8xIgGBA0lwnnZmf0t51FNCS5NhR2MtzSUhrtp3zGdNG1Qw433A%2B%2FuCbkP0RH3dzh%2B8od%2BR%2BLYv6JGDbVCTk2dcTvOvnlTMjrm8v%2F5dyb8EGvhdSD7Cx0bsggAajCzJ3Tt3K%2FuJ3sgGa05Fsra%2ByfMCrkUi8mimRwzey150D9olv%2Bnz6cB6cQdj8A%3D%3D--PkOtCOWoxgPYgFf5--rwz3oVGH8sbpYY6qojcjkQ%3D%3D";

    public List<Coordinates> limeBikesInTheArea = new List<Coordinates>();

    private string fullLimeURL;
    public TwilioMF twilioMF;

    public void beginAuth() {

        //TODO: Check if current code works

        StartCoroutine(LimeAuth1());
    }
    IEnumerator LimeAuth1() {
        //Auth1:

        fullLimeURL = limebikeURL + "/v1/login";
        for (int i = 0; i < 2; i += 2) {
            fullLimeURL += "?" + limeLoginFields[i];
            fullLimeURL += "=" + limeLoginFields[i + 1];
        }
        UnityWebRequest limeAuth1Request = UnityWebRequest.Get(fullLimeURL);
        yield return limeAuth1Request.SendWebRequest();
        if (limeAuth1Request.isNetworkError || limeAuth1Request.isHttpError)
            Debug.LogError(limeAuth1Request.error);
        else
            Debug.Log("Auth 1 Lime done!");
        //to make sure lime sends the code to Twilio on time      Needs an UPDATE

        yield return new WaitForSecondsRealtime(1);
        Debug.Log(limeAuth1Request.responseCode);
        twilioMF.ReadLastMessage(6);
        limeAuth1Request.Dispose();
    }

    public void CodeReceivedTwilio(string code) {
        Debug.Log("we received this code: " + code);
        limeLoginFields[3] = code;
        StartCoroutine(LimeAuth2());
    }
    IEnumerator LimeAuth2() {
        fullLimeURL = limebikeURL + "/v1/login";
        for (int i = 0; i < 4; i += 2) {
            if (i < 2)
                fullLimeURL += "?" + limeLoginFields[i];
            else
                fullLimeURL += "&" + limeLoginFields[i];
            fullLimeURL += "=" + limeLoginFields[i + 1];
        }
        Debug.Log(fullLimeURL);

        WWWForm limeAuth2Form = new WWWForm();
        UnityWebRequest limeAuth2Request = UnityWebRequest.Post(fullLimeURL, limeAuth2Form);

        yield return limeAuth2Request.SendWebRequest();
        if (limeAuth2Request.isNetworkError || limeAuth2Request.isHttpError)
            Debug.LogError(limeAuth2Request.error);
        else
            Debug.Log("Auth 2 Lime done!");

        JSONNode auth2JSON = JSON.Parse(limeAuth2Request.downloadHandler.text);
        authKey = auth2JSON["token"];

        //save the new key
        StringPair authSP = new StringPair(myAuthAddress, authKey);
        AuthManager.SaveAuth(authSP);
        
        //dispose of the request
        limeAuth2Request.Dispose();
    }

    public void LoadAuthToken() {
        authKey = AuthManager.LoadAuth(myAuthAddress);
        Debug.Log("Loaded Key: " + authKey);
    }

    public void GetVehichles(Coordinates NE, Coordinates SW, Coordinates userCoordinates) {
        StartCoroutine(GetLimeVehichles(NE, SW, userCoordinates));
    }

    IEnumerator GetLimeVehichles(Coordinates NE, Coordinates SW, Coordinates userCoordinates) {
        #region build the URL
        fullLimeURL = limebikeURL + "/v1/views/map";
        limeVehichleFieldAnswers[0] = NE.lat.ToString();
        limeVehichleFieldAnswers[1] = NE.lon.ToString();
        limeVehichleFieldAnswers[2] = SW.lat.ToString();
        limeVehichleFieldAnswers[3] = SW.lon.ToString();
        limeVehichleFieldAnswers[4] = userCoordinates.lat.ToString();
        limeVehichleFieldAnswers[5] = userCoordinates.lon.ToString();
        limeVehichleFieldAnswers[6] = "17"; //TODO: set to the current zoom level of the map
        Debug.Log("say hi");
        for (int i = 0; i < 7; i++) {
            if (i == 0)
                fullLimeURL += "?";
            else
                fullLimeURL += "&";
            fullLimeURL += limeVehichleFields[i];
            fullLimeURL += "=";
            fullLimeURL += limeVehichleFieldAnswers[i];
        }
        Debug.Log(fullLimeURL);
//        yield return new WaitForSeconds(2); 
        #endregion



        LoadAuthToken();
        UnityWebRequest getVehichlesRequest = UnityWebRequest.Get(fullLimeURL);
        getVehichlesRequest.SetRequestHeader("authorization", "Bearer " + authKey);
        getVehichlesRequest.SetRequestHeader("cookie", limeCookie);

        yield return getVehichlesRequest.SendWebRequest();
        if (getVehichlesRequest.isNetworkError || getVehichlesRequest.isHttpError) 
            Debug.LogError(getVehichlesRequest.error);
        else
            Debug.Log("Lime Vehichle Request done!");

        JSONNode vehichleJSONFull = JSON.Parse(getVehichlesRequest.downloadHandler.text);
        JSONNode vehichleJSONParsed = vehichleJSONFull["data"]["attributes"]["bikes"];
        for (int i = 0; i < vehichleJSONParsed.Count; i++) {
            limeBikesInTheArea.Add(new Coordinates(vehichleJSONParsed[i]["attributes"]["latitude"], vehichleJSONParsed[i]["attributes"]["longitude"]));
        }
        
        //Print the Bikes
        for (int i = 0; i < Mathf.Min(limeBikesInTheArea.Count, 5); i++) {
                    Debug.Log(limeBikesInTheArea[i].lat + " : " + limeBikesInTheArea[i].lon);
        }
 
    }

}
