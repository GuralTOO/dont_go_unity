using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;

public class LyftITP : InterpreterBase  //must inherit from "InterpreterBase"
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
        parent = new GameObject("Lyft Vehicles");
//        parent.transform.SetParent(FindGameObjectsWithTag("Page1")[0].transform);

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
            UnityWebRequest getvehiclesRequest = UnityWebRequest.Get(url_links[k]);
            //Debug.Log(url_links[k] + " link given to Lyft" + url_links.Count);

            yield return getvehiclesRequest.SendWebRequest();
            if (getvehiclesRequest.isNetworkError || getvehiclesRequest.isHttpError)
                Debug.LogError(getvehiclesRequest.error);
            else
                Debug.Log("LyftITP connected");

            //parse the data into myvehicles
            fullJSON = JSON.Parse(getvehiclesRequest.downloadHandler.text);

            vehiclesJSON = fullJSON["data"]["bikes"];
            for (int i = 0; i < vehiclesJSON.Count; i++) {
                //changes made on Aug 23rd
                VehicleClass tempvehicle = new VehicleClass(float.Parse(vehiclesJSON[i]["lat"]),
                float.Parse(vehiclesJSON[i]["lon"]), 0, "Lyft", vehiclesJSON[i]["bike_id"]);
                //the code below is custom for every ITP, it assigns the correct type of vehicle (default = scooter)
                if (vehiclesJSON[i]["type"] == "electric_bike")
                    tempvehicle.type = 1;

                //add to the library if needed
                if (vDB.activeVehiclesDictionary.ContainsKey(tempvehicle.id) == false)
                    myVehicles.Add(tempvehicle);
                else
                    vDB.activeVehiclesDictionary[tempvehicle.id].cnt = cntBoss;
            }
            getvehiclesRequest.Dispose();
        }

        Debug.Log("Lyft has " + myVehicles.Count + " vehicles in this city" + vehiclesJSON.Count);

        //spawn The vehicles
        mapSpawner.Spawn(myVehicles, parent, cntBoss);

    }

    //Aug 23rd - new class "vehicleClass" introduced to replace "Coordinates" class in ITP scripts.
    //To make the change work, also change the Mapspawner script's "Spawn" function variable from Coordinates type to VehichlesClass

}
