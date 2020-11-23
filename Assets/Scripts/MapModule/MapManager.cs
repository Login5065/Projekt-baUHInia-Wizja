using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    /// <summary>
    /// Saves passed mapData to folder as JSON.
    /// </summary>
    /// <param name="mapData">MapData to save</param>
    public static void SaveLocalMap(MapData mapData)
    {
        string jsonMapData = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(("Assets/LocalMaps/" + mapData.mapName + ".json"), jsonMapData);
    }

    /// <summary>
    /// Returns MapData with provided name from local folder.
    /// </summary>
    /// <param name="mapName">Name of map to load</param>
    /// <returns>MapData of map with passed name</returns>
    public static MapData LoadLocalMap(string mapName)
    {
        string savePath = "Assets/LocalMaps/" + mapName + ".json";
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<MapData>(json);
        }
        else
        {
            Debug.LogWarning("Saved map doesn't exist.");
            return null;
        }
    }
}
