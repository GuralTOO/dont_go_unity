using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public static class AuthManager{
    
    public static void SaveAuth (StringPair authToSave) {
        //create a formatter + set the path + open the file
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Auths.Ibite";
        Auth auth;

        Debug.Log("Saving code: " + authToSave.second);

        //if save already exists
        if (File.Exists(path)) {
            FileStream filestreamOpen = new FileStream(path, FileMode.Open);
            //decode the file
            Debug.Log("File already exists, trying to re-write");
            auth = (Auth)formatter.Deserialize(filestreamOpen);
            //edit the file
            auth.tokens[authToSave.first] = authToSave.second;
            //close stream
            filestreamOpen.Close();
        }
        else {
            //create new auth
            auth = new Auth(authToSave);
        }
        //saving the auth files;
        FileStream filestreamSave = new FileStream(path, FileMode.Create);
        //encode the file
        formatter.Serialize(filestreamSave, auth);
        filestreamSave.Close();
    }
    public static string LoadAuth(string authAddress) {
        //open formatter + set the path + load the file
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Auths.Ibite";
        Auth auth;
        if (File.Exists(path)) {
            FileStream filestreamOpen = new FileStream(path, FileMode.Open);
            auth = (Auth)formatter.Deserialize(filestreamOpen);
            filestreamOpen.Close();
            return auth.tokens[authAddress];
        }
        else {
            Debug.LogError("No file to load from");
            return null;
        }
    }

}
