using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FeedbackManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Feedback_SO feedbackSO;
    public GameObject text;
    private string tmproText;

    public void RegisterPress(int questionIdButtonId) {
        feedbackSO.questions[questionIdButtonId/10] = questionIdButtonId%10;
    }

    public void RegisterComment() {
        tmproText = text.GetComponent<TextMeshProUGUI>().text;
        feedbackSO.comment = tmproText;
    }

    public void SendTheTranscript() {
        /*TODO:
        Send the data to the server
        */
    }

    void Start()
    {
    }

}
