using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.UI;
//using TMPro;



public class APIController : MonoBehaviour {

    #region old version
    //OLD TODO:
    //Set Up Lime account w Twilio  - Done
    //Use twilio to Auth with Lime to get Auth code     - Done
    //Create a class to store Auth code     - Done
    //Use binary coding to save and load the auth code  - Done

    #endregion

    //TODO:
    //Create SOs for every major city we want. - in process
    //Build a list of cities with their center coordinates. - in process
    //Create an interpreter for every company to display the data for any particular city. (bird done) (lyft done Bayram)

    //TODO:
    //Every 5 seconds, go through the list of cities, find the close ones (<100km from center) and update the list of total available vehichles.
    //Every 5 seconds, or by map trigger, update the list of vehichles displayed on the screen from the list created above.
    //only take vehichles that are within the radius of the map

    //TODO: UI design
    //The profile button
    //Highlight closest vehichles button
    //design change disc
    //for every company, we need a two-tone color scheme and logo.

    //TODO: locate screen borders and don't spawn vehichles out of screen

    public int mainCnt = 1;




    [SerializeField] public Coordinates userCoordinates;
    private static float maxDistanceFromCityCenter = 100;   //in KM
    public Link_SO[] myCities;
    public GameObject moveManager;
    private Dictionary<string, bool> companiesVisited;
    private List<StringPair> companiesList;
    private List<string> tempURLList;
    string myITPString;

    private void Start() {
        companiesVisited = new Dictionary<string, bool>();
        companiesList = new List<StringPair>();
        tempURLList = new List<string>();
    }

    public void Go() {
        mainCnt++;
        mainCnt %= 1009;
        companiesVisited.Clear();
        companiesList.Clear();
        for(int i=0; i<myCities.Length; i++) {
            if(DistanceCalculator.Calc(myCities[i].city_center, userCoordinates) <= maxDistanceFromCityCenter) {
                for(int j=0; j<myCities[i].company_link.Length-1; j++) {
                    myITPString = myCities[i].company_link[j].company + "ITP";
                    companiesList.Add(new StringPair(myITPString, myCities[i].company_link[j].link));
                }
            }
        }
        HandlerScript h = FindObjectOfType<HandlerScript>();

        foreach (StringPair X in companiesList) {
            //check to make sure this company was done already
            if (!companiesVisited.ContainsKey(X.first)) {
                //mark the company as done
                companiesVisited[X.first] = true;
                //compile all URLs for the company
                tempURLList.Clear();
                foreach (StringPair Y in companiesList)
                    if (X.first == Y.first) 
                        tempURLList.Add(Y.second);                                        
                h.dodo(X.first, tempURLList, mainCnt);
            }
        }

    }
    

    /*Old version
    #region Authenticate with bike provider(s)

    public LimeHandler lime;

    //Go through all providers and authenticate
    #endregion
    public void BeginAutherizations() {
        lime.beginAuth();
    }
    #region Send Requests to update transport in the zoomed area
    //update the map border long and lang
    //build a function to send requests update transport in zoomed area
    //build a function to erase old and display new transport in zoomer area



    public void GetAllVehichles() {
        //DC coord: 
        //NE: 38.940760, -77.052666
        //SW: 38.927572, -77.066368
        //user: 38.933544, -77.056516
        //1. Set Up Coordinates
        Debug.Log("Oh yeah");
        Coordinates NECorner = new Coordinates(38.940760, -77.052666);          //TODO: Get map corner coordinates
        Coordinates SWCorner = new Coordinates(38.927572, -77.066368);
        Coordinates userCoordinates = new Coordinates(38.933544, -77.056516);   //TODO: Get user or map center coordinates

        //2. Send requests
        StartCoroutine(GetAllVehichlesIterator(NECorner, SWCorner, userCoordinates));
    }
    IEnumerator GetAllVehichlesIterator(Coordinates NE, Coordinates SW, Coordinates userCoordinates) {

        lime.GetVehichles(NE, SW, userCoordinates);
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("Done Waiting!");
        Debug.Log(lime.limeBikesInTheArea[0].lat + " " + lime.limeBikesInTheArea[0].lon);
    }

    #endregion
    */
}

#region Courutine check GPS location and new transport

//Update userLong, userLang
//Call function to update transport in zoomed area
#endregion



/*
 * Current sit:
 * Every ITP updates its list of vehicles. If a vehicle is already on screen, just update its timer. If not on screen, add to spawners
 * The spawner goes through the dictionary and updates the locations of the vehicles that have the correct timer. If the timer is old, deletes the vehicle.
 * Next Steps:
    Every ITP manages its list of old and new vehicles (updated every 5 secs)
    The Spawned objects on the map are all in a single Dictionary that is in a SO.
    The dictionary has references to the instance of the spawned object and its coordinates
    MapSpawner is used to update the vehicles upon the movement of the screen.
 */


