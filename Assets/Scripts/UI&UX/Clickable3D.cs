using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable3D : MonoBehaviour
{

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f)) {    //how far the ray will go
                if (hit.transform != null) {
                    PrintName(hit.transform.gameObject);
                }
            }
        }
    }

    private void PrintName(GameObject go) {
        BringUI miniUI = FindObjectOfType<BringUI>();
        miniUI.BringUIPlease();
    }
}
