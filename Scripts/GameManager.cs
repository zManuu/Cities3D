using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Serializable]
    public class SaveDetails
    {

        public string CityName;
        public List<RoadData.Data> Roads;

        /// <returns>A JSON string containing all the relavent information.</returns>
        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }







    [SerializeField] public Transform[] RoadPrefabs;
    [SerializeField] private Transform RoadHolder; // will be parent to all the generated roads

    private int CurrentRoadRotation = 0;
    public SaveDetails Save;

    private void Start()
    {
        string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cities3D";
        string filePath = dirPath + "\\world.citysave";

        if (!Directory.Exists(dirPath))
        {
            print("DATA DIR NOT FOUND; CREATING THE DIR...");
            Directory.CreateDirectory(dirPath);
        }

        if (!File.Exists(filePath))
        {
            print("CITY SAVE FILE NOT FOUND; CREATING EXAMPLE FILE...");
            File.Create(filePath).Close();

            SaveDetails exampleSave = new SaveDetails()
            {
                CityName = "TEST",
                Roads = new List<RoadData.Data>()
            };

            File.WriteAllText(filePath, exampleSave.ToString());
        }

        Save = JsonUtility.FromJson<SaveDetails>(File.ReadAllText(filePath));
        print(Save.Roads.Count);
        Save.Roads.ForEach(roadSegment =>
        {
            CreateRoad(false, roadSegment.X, roadSegment.Y, roadSegment.Rotation, roadSegment.Type);
        });
    }

    public void CreateRoad(bool addToSave, int x, int y, int rotation = 0, int type = 0)
    {

        // check if there is a road on the tile
        if (Save.Roads.Find(data => data.X == x && data.Y == y) != null && addToSave)
        {
            print("road segment detected!");
            return;
        }


        // create the road (BACKEND & FRONTEND)
        Transform road = Instantiate(RoadPrefabs[type], RoadHolder);
        road.position = new Vector3(x, 0.5f, y);
        road.gameObject.AddComponent(typeof(RoadData));
        road.GetComponent<RoadData>().SetData(x, y, type, rotation);

        if (addToSave)
            Save.Roads.Add(road.GetComponent<RoadData>().data);

        switch (road.GetComponent<RoadData>().data.Rotation)
        {
            case 1:
                road.Rotate(new Vector3(0, 90, 0), Space.Self);
                break;
            case 2:
                road.Rotate(new Vector3(0, 180, 0), Space.Self);
                break;
            case 3:
                road.Rotate(new Vector3(0, 270, 0), Space.Self);
                break;
        }
    }

    public void DeleteRoadSegment(int x, int y)
    {
        Save.Roads.Remove(Save.Roads.Find(d => d.X == x && d.Y == y));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) || (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)))
        {
            print("Saving...");
            SaveCity();
        }
    }

    private void SaveCity()
    {
        var saveDetails = new SaveDetails();
        saveDetails.CityName = "zfvailfawbliufwabv";
        saveDetails.Roads = new List<RoadData.Data>();

        for (var i=0; i<RoadHolder.childCount; i++)
        {
            saveDetails.Roads.Add(RoadHolder.GetChild(i).GetComponent<RoadData>().data);
        }

        File.WriteAllText(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cities3D\\world.citysave",
            saveDetails.ToString()
            );
    }

}
