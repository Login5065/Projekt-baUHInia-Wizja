[System.Serializable]
public class MapListResponse {
    public bool status;
    public string msg;
    public MapEntry[] entries;
}

[System.Serializable]
public class MapEntry {
    public string mapName;
    public string author;
    public string date;
    public string UUID;
}