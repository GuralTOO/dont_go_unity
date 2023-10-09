using UnityEngine;

[System.Serializable]
public class company_link {

    public string company;
    public string link;
    public GameObject myITP;
    public company_link (string a, string b) {
        company = a;
        link = b;
    }

}
