using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringUI : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject scooterImage;
    private APIController apiController;

    public void BringUIPlease(){
        apiController = FindObjectOfType<APIController>();
//        scooterPop.transform.position = mainCam.WorldToScreenPoint(new Vector3(540, 1080, mainCam.nearClipPlane));
        scooterImage.SetActive(true);
    }
}
