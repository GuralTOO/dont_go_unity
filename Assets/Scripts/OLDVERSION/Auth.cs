using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Auth {
    //number of Auths
    public SortedDictionary<string, string> tokens;// = new SortedDictionary<string, string>[1];

    public Auth (StringPair a) {
        tokens = new SortedDictionary<string, string>();
        tokens[a.first] = a.second;
    }
}
