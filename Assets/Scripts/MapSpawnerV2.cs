using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using TMPro;

public class MapSpawnerV2 : MonoBehaviour {
    RectTransform canvasRT;
    Vector2 viewPortPosition;
    Vector3 coordinatesinWorldLocation;
    Camera cam;
    private int cntBoss;
    private APIController apiControlla;

    private float westernMostPoint;     //subtract from the interested latitude
    private float northernMostPoint;    //subtract the interested longitude from this

    public GameObject objectToSpawn;
    [SerializeField]
    AbstractMap _map;
    [SerializeField]
    float _spawnScale = 0.5f;
    [SerializeField]
    private VehichleDB_SO vDB;
    private List<string> idsToRemove = new List<string>();
    private TextMeshPro tempTMPRO;

    void Start() {
        canvasRT = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        cam = Camera.main;
        apiControlla = FindObjectOfType<APIController>();
        westernMostPoint = -124.731679f;
        northernMostPoint = 49.349711f;
    }

/*    KeyValuePair<int, int> GetIJ(float a, float b) {
        a -= westernMostPoint;
        b = northernMostPoint - b;
//        return new createKey(a, b);
    }
*/
    public void Spawn(List<VehicleClass> coordinatesToSpawnAt, GameObject targetParent, int cntBoss) {
        for (int i = 0; i < coordinatesToSpawnAt.Count; i++) {
            if (vDB.activeVehiclesDictionary.ContainsKey(coordinatesToSpawnAt[i].id) == true)
                continue;
            //get desired location
//            KeyValuePair<int, int> iJ = GetIJ(coordinatesToSpawnAt[i].lat, coordinatesToSpawnAt[i].lon);
            coordinatesinWorldLocation = _map.GeoToWorldPosition(new Vector2d(coordinatesToSpawnAt[i].lat, coordinatesToSpawnAt[i].lon), true);
            //instantiate UI and change its location to desired location
            var instance = Instantiate(objectToSpawn);
            instance.transform.SetParent(targetParent.transform);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

            if (instance.transform.Find("Texti").GetComponent<TextMeshPro>() != null) {
                tempTMPRO = instance.transform.Find("Texti").GetComponent<TextMeshPro>();
                tempTMPRO.text = coordinatesToSpawnAt[i].company;
                if (coordinatesToSpawnAt[i].company == "Bird")
                    tempTMPRO.material.color = new Color32(255, 255, 255, 255);

            }
            /*
             *Experimenting: 

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = coordinatesinWorldLocation;
            cube.transform.parent = Camera.main.gameObject.transform;
             * Done experimenting
             */

            viewPortPosition = cam.WorldToViewportPoint(coordinatesinWorldLocation);

            instance.GetComponent<RectTransform>().position = new Vector2(
            (viewPortPosition.x * canvasRT.sizeDelta.x) - (canvasRT.sizeDelta.x * 0.5f),
            (viewPortPosition.y * canvasRT.sizeDelta.y) - (canvasRT.sizeDelta.y * 0.5f));
            //            Debug.Log("so we here");

            //add to the map
            vDB.activeVehiclesDictionary.Add(coordinatesToSpawnAt[i].id, new VehicleDictClass(coordinatesToSpawnAt[i], cntBoss, instance.transform));
        }

    }

    public Coordinates GiveCoordinates(Vector3 worldPosition) {
        Vector2d getPosition = _map.WorldToGeoPosition(worldPosition);
        return new Coordinates((float)getPosition.x, (float)getPosition.y);
    }

    private void Update() {
    }

    public void ManualUpdate() {
        cntBoss = apiControlla.mainCnt;
        //        Debug.Log("manually updatin, size: " + vDB.activeVehiclesDictionary.Count);
        canvasRT = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        foreach (var X in vDB.activeVehiclesDictionary) {
            //check if the object must be removed (not up to date)
            if (X.Value.cnt != cntBoss) {
                idsToRemove.Add(X.Value.vehicleClass.id);
                continue;
            }
            //get desired location
            coordinatesinWorldLocation = _map.GeoToWorldPosition(new Vector2d(X.Value.vehicleClass.lat, X.Value.vehicleClass.lon), true);
            //Get the UI and change its location to desired location

            viewPortPosition = cam.WorldToViewportPoint(coordinatesinWorldLocation);
            X.Value.transform.GetComponent<RectTransform>().position = new Vector2(
            (viewPortPosition.x * canvasRT.sizeDelta.x) - (canvasRT.sizeDelta.x * 0.5f),
            (viewPortPosition.y * canvasRT.sizeDelta.y) - (canvasRT.sizeDelta.y * 0.5f));
            //            spawnedObjects[i].transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        }
        for (int i = 0; i < idsToRemove.Count; i++)
            vDB.activeVehiclesDictionary.Remove(idsToRemove[i]);
        idsToRemove.Clear();
    }

}
