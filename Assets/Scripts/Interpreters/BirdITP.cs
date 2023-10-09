using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;

[System.Serializable]
public class BirdITP : InterpreterBase  //must inherit from "InterpreterBase"
{
    public List<VehicleClass> myVehicles;
    [SerializeField]
    private VehichleDB_SO vDB;
    [SerializeField]
    private MapSpawner mapSpawner;  //to spawn objects
    public int cntBoss;

    private GameObject parent;
    private JSONNode fullJSON;
    private JSONNode vehiclesJSON;

    private void Start() {
        parent = new GameObject("Bird Vehicles");
        parent.transform.SetParent(FindObjectOfType<Canvas>().transform.Find("Vehicles").transform);
    }

    public override void RetrieveVehicles(List<string> url_links, int x) {
        cntBoss = x;
        StartCoroutine(Vehicles(url_links));
    }

    IEnumerator Vehicles(List<string> url_links) {

        myVehicles.Clear();
        for (int k = 0; k < url_links.Count; k++) {
            //send request & check for errors
            UnityWebRequest getVehiclesRequest = UnityWebRequest.Get(url_links[k]);
            //Debug.Log(url_links[k] + " link given to Bird" + url_links.Count);
            yield return getVehiclesRequest.SendWebRequest();
            if (getVehiclesRequest.isNetworkError || getVehiclesRequest.isHttpError)
                Debug.LogError(getVehiclesRequest.error);
            else
                Debug.Log("BirdITP connected");

            //parse the data into myVehichles
            fullJSON = JSON.Parse(getVehiclesRequest.downloadHandler.text);

            vehiclesJSON = fullJSON["data"]["bikes"];
            for (int i = 0; i < vehiclesJSON.Count; i++) {
                //changes made on Aug 23rd
                VehicleClass tempvehicle = new VehicleClass(float.Parse(vehiclesJSON[i]["lat"]),
                float.Parse(vehiclesJSON[i]["lon"]), 0, "Bird", vehiclesJSON[i]["bike_id"]);
                //the code below is custom for every ITP, it assigns the correct type of vehicle (default = scooter)
                //add to the library if needed
                if (vDB.activeVehiclesDictionary.ContainsKey(tempvehicle.id) == false)
                    myVehicles.Add(tempvehicle);
                else 
                    vDB.activeVehiclesDictionary[tempvehicle.id].cnt = cntBoss;
            }
            getVehiclesRequest.Dispose();
        }

        Debug.Log("Bird has " + myVehicles.Count + " vehichles in this city" + vehiclesJSON.Count);
        //spawn The Vehichles
        mapSpawner.Spawn(myVehicles, parent, cntBoss);

    }

}
