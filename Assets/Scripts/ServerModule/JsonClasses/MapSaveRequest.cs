[System.Serializable]
public class MapSaveRequest {
    public MapSaveRequest() {
        mapGameData = new GameData();
        mapGameData.allNeededData = new AllNeededData();
        mapGameData.allNeededData.mapData = new MapData();
        mapGameData.allNeededData.buildings = new Buildings();
    }
    
    public GameData mapGameData;
    public bool isSolution;
    public string accToken;
}