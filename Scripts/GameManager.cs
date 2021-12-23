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
        public RoadSegment[] RoadSegments;

        /// <returns>A JSON string containing all the relavent information.</returns>
        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [Serializable]
    public class RoadSegment
    {
        public int X;
        public int Y;
        public int Type;

        public RoadSegment(int X, int Y, int Type)
        {
            this.X = X;
            this.Y = Y;
            this.Type = Type;
        }
    }







    [SerializeField] private Transform RoadTransform;
    [SerializeField] private Transform RoadHolder; // will be parent to all the generated roads

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
                RoadSegments = new RoadSegment[]
                {
                    new RoadSegment(1, 1, 0),
                    new RoadSegment(1, 2, 0),
                    new RoadSegment(3, 2, 0)
                }
            };

            File.WriteAllText(filePath, exampleSave.ToString());
        }

        SaveDetails saveDetails = JsonUtility.FromJson<SaveDetails>(File.ReadAllText(filePath));
        saveDetails.RoadSegments.ToList().ForEach(roadSegment =>
        {
            Transform road = Instantiate(RoadTransform, RoadHolder);
            print(JsonUtility.ToJson(roadSegment));
            road.position = new Vector3(roadSegment.X, 0.5f, roadSegment.Y);
        });
    }

}
