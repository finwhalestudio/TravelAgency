using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace FinwhaleStudio.TravelAgency
{
    public class Location
    {
        public float x;
        public float y;

        public Location(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class Place
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public Location Pos { get; set; }
    }
    public class PlaceData
    {
        public List<Place> places;
        public string version;
    }

    public class DataManager : MonoBehaviour
    {
        void Start()
        {
            PlaceData placeData = new PlaceData();
            placeData.version = "1";
            placeData.places = new List<Place>();
            
            Place newPlace = new Place { ID = "U018X2", Name = "Oxford", Pos = new Location(-2.75f, 124.09f) };
            placeData.places.Add(newPlace);

            string serializedObject = JsonConvert.SerializeObject(placeData);

            string dataFolder = Directory.GetParent(Application.dataPath).FullName + @"\Data\";
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
            string locationFilePath = dataFolder + @"places.json";
            File.WriteAllText(locationFilePath, serializedObject);

            //var player = JsonConvert.DeserializeObject<DBPlayer>(serializedObject);
        }
    }
}