[System.Serializable]
public class MapSaveRequest {
    public MapSaveRequest() {
        mapGameData = new GameData();
        mapGameData.allNeededData = new AllNeededData();
        mapGameData.allNeededData.mapData = new MapData();
    }
    
    public GameData mapGameData;
    public bool isSolution;
    public string accToken;
}