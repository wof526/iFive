using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class carDic : MonoBehaviour
{
    carData cardata;
    public Dictionary<string, carData> carList; // new dictionary

    

    public void Start()
    {
        cardata = GetComponent<carData>();

        carList = new Dictionary<string, carData>(); //edit

        string name;



        //////// add car

        // string _carName, int _carNum, float _maxSpeed, float _atk, float _hp, float _dashSpeed

        name = "Titan-3";
        carList.Add(name, new carData(name, 0, 60f, 4.5f, 800f, 30f,
            "111111 "));

        name = "M1 Abrams";
        carList.Add(name, new carData(name, 1, 50f, 5.0f, 1000f, 25f,
            "Invincible for eight seconds."));

        name = "Juggernaut";
        carList.Add(name, new carData(name, 2, 90f, 4.5f, 600f, 70f,
            "Stronger dash (double hp damage) 6 seconds."));

        name = "Rhino";
        carList.Add(name, new carData(name, 3, 120f, 5.0f, 500f, 80f,
            "Converted to atk 1 second, for 10 seconds."));        

        name = "Buggy-0";
        carList.Add(name, new carData(name, 4, 140f, 6.0f, 300f, 100f,
            "Right into dash speed."));

        name = "Acid Bike";
        carList.Add(name, new carData(name, 5, 160f, 7.0f, 150f, 120f,
            "Maximum speed increased by 1.3x."));

        name = "Sepia";
        carList.Add(name, new carData(name, 6, 80f, 4.0f, 250f, 40f,
            "Car heal + 150 in a nearby 10m circular radius."));

        name = "HoneyBee";
        carList.Add(name, new carData(name, 7, 70f, 3.0f, 350f, 50f,
            "When in dash state, hp + 300 on the hit friendly car and force up at full speed."));



        ////// car search
        /*
        if (carList.ContainsKey("Sepia"))
        {
            cardata = carList["Sepia"]; // change cardata.this???????
            cardata.Show();
        }
        */

        ////// car output
         
    }


    
    public void ShowCarNdata()
    {
        switch (this.name) // this == Button Name
        {
            case "Titan3Btn":
                Debug.Log("case titan");
                cardata = carList["Titan-3"]; // change cardata.this
                cardata.ShowData(); // show data
                cardata.ShowCar(); // show car

                // change gage value
                break;


            case "M1Btn":
                Debug.Log("case M1");
                cardata = carList["M1 Abrams"];
                cardata.ShowData();
                cardata.ShowCar();
                break;

            case "JuggerBtn":
                Debug.Log("case Jugger");
                cardata = carList["Juggernaut"];
                cardata.ShowData();
                cardata.ShowCar();
                break;

            case "RhinoBtn":
                Debug.Log("case Rhino");
                cardata = carList["Rhino"];
                cardata.ShowData();
                cardata.ShowCar();
                break;

            case "BuggyBtn":
                Debug.Log("case Buggy");
                cardata = carList["Buggy-0"];
                cardata.ShowData();
                cardata.ShowCar();
                break;

            case "AcidBtn":
                Debug.Log("case Acid Bike");
                cardata = carList["Acid Bike"];
                cardata.ShowData();
                cardata.ShowCar();
                break;

            case "SepiaBtn":
                Debug.Log("case Sepia");
                cardata = carList["Sepia"];
                cardata.ShowData();
                cardata.ShowCar();
                break;

            case "HoneyBeeBtn":
                Debug.Log("case HoneyBee");
                cardata = carList["HoneyBee"];
                cardata.ShowData();
                cardata.ShowCar();
                break;
        }
    }
    
    
}
