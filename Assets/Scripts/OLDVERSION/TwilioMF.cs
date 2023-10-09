using UnityEngine;
using UnityEngine.Networking;
using System;
using SimpleJSON;
using System.Collections;
using UnityEngine.UI;
using TMPro;


public class TwilioMF : MonoBehaviour {
    private string accountSid = "AC00da2f8b5b8968bd35098e35d88e5942";
    string authToken = "442a53a40e6fbf32a36854a4c1118f3d";
    public string twilioNumber = "14693522537";
    string getURL = "https://api.twilio.com/2010-04-01/Accounts/";// +{{ACCOUNT_SID}}/Messages.json";
    public TextMeshProUGUI textBox;
    public string theCode = "";
    public LimeHandler limeHandler;

    private void Start() {
        Debug.Log("Oh fuck me");
    }

    public void ReadLastMessage(int x) {
        StartCoroutine(ReadLastMessageCoroutine(x));
    }

    public IEnumerator ReadLastMessageCoroutine(int xDigits) {
        getURL += accountSid;
        getURL += "/Messages.json";
        Debug.Log(getURL);
        UnityWebRequest getMessagesRequest = UnityWebRequest.Get(getURL);
        getMessagesRequest.SetRequestHeader("Authorization", "Basic QUMwMGRhMmY4YjViODk2OGJkMzUwOThlMzVkODhlNTk0Mjo0NDJhNTNhNDBlNmZiZjMyYTM2ODU0YTRjMTExOGYzZA==");
        //no idea where to get that code: got it from Postman
        yield return getMessagesRequest.SendWebRequest();
        if (getMessagesRequest.isNetworkError || getMessagesRequest.isHttpError) {
            Debug.LogError(getMessagesRequest.error);
            yield break;
        }
        JSONNode messagesJSON = JSON.Parse(getMessagesRequest.downloadHandler.text);
        JSONNode messagesArrayJSON = messagesJSON["messages"];
//        textBox.text = messagesArrayJSON[0]["body"];
        string fullText = messagesArrayJSON[0]["body"];
        theCode = "";
        for(int i=0; i<xDigits; i++)
            theCode += fullText[i];
        Debug.Log(theCode);
        //call foreign function CodeReceived(theCode);
        limeHandler.CodeReceivedTwilio(theCode);
        getMessagesRequest.Dispose();
    }

}



 