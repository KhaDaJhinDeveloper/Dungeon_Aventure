using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(SpawnEnemy))]
public class RoomManager : MonoBehaviour
{
    public  List<Room> roomPrefabs = new List<Room>();  
    public  List<Room> roomOneExitsPrefabs = new List<Room>();   
    public  List<Room> wallPrefabs = new List<Room>(); 
    [HideInInspector] public List<Room> placedRooms = new List<Room>();  
    List<GameObject> roomFailedList = new List<GameObject>();
    public int roomCount;

    private DungeonLayoutData dungeonLayoutData = new DungeonLayoutData();  
    private RoomInstanceData roomInstanceData = new RoomInstanceData();
    string sceneName;
    
    [Tooltip("ID duy nhất để phân biệt RoomManager này. Nếu để trống sẽ dùng tên GameObject.")]
    public string managerID = "";
    
    private void Awake()
    {
        // Luôn lấy sceneName trước
        this.sceneName = SceneManager.GetActiveScene().name;
        
        if (string.IsNullOrEmpty(managerID))
        {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            this.managerID = $"{gameObject.name}_Scene_{sceneName}_Index_{buildIndex}";
        }
        
        bool loadedFromSave = false;
        if (DungeonDataManager.Instance != null && DungeonDataManager.Instance.HasData())
        {
            // LoadData() với sceneName để chỉ giữ lại data của scene hiện tại, xóa data của scene khác
            DungeonDataManager.Instance.LoadData(this.sceneName);
            var layout = DungeonDataManager.Instance.GetLayout(this.sceneName, managerID);
            if (layout != null)
            {
                loadedFromSave = LoadDungeonLayout(layout);
                // Sau khi load, cần kiểm tra và reset các exit thừa (không có room kết nối)
                if (loadedFromSave)
                {
                    ValidateAndResetRedundantExits();
                }
            }
        }

        if (!loadedFromSave)
        {
            GenerateLevel();
        }

        MarkOverlappingExits();
        int unusedExitsCount = CountUnusedExits();
        while (unusedExitsCount > 0)
        {
            BlockEedundantExits();
            unusedExitsCount = CountUnusedExits();
        }
        foreach (Room room in placedRooms)
            room.transform.SetParent(this.gameObject.transform);
        DeleteRoomFailed();

        if (!loadedFromSave && DungeonDataManager.Instance != null)
        {
            SaveDungeonLayout();
        }
    }
    public void GenerateLevel()
    {       
        Room startingRoom = Instantiate(roomPrefabs[0], transform.position, Quaternion.identity);
        placedRooms.Add(startingRoom);      
        roomPrefabs.RemoveAt(0);
        for (int i = 0; i < roomCount; i++)
        {
            if (roomPrefabs.Count == 0)
            {
                break;
            }
            PlaceRoom();
        }
        for(int i = 0;i < CountUnusedExits(); i++)
        {
            if (roomOneExitsPrefabs.Count == 0)
            {
                break;
            }
            PlaceOneExitRoom();
        }
        CountUnusedExits();
    }
    void PlaceRoom()
    {
        int lastRoomIndex;
        if (roomPrefabs.Count == 0)
        {
            return;
        }
        if (placedRooms.Count >= 2)
        {
            lastRoomIndex = placedRooms.Count -1;
        }
        else lastRoomIndex = UnityEngine.Random.Range(0, placedRooms.Count); 
        Room lastRoom = placedRooms[lastRoomIndex];
        Room.Exits lastExit = GetUnusedExit(lastRoom);
        int count = 0;
        bool roomPlaced = false;

        while (count < roomPrefabs.Count*2 && !roomPlaced)
        {
            count++;
            int newRoomIndex =  UnityEngine.Random.Range(0, roomPrefabs.Count);
            Room newRoom = Instantiate(roomPrefabs[newRoomIndex]);
            Room.Exits newEntrance = GetUnusedExit(newRoom);
            if (CorrespondingEntrances(lastExit.exitDirections, newEntrance.exitDirections))
            {
                Vector2 newRoomPosition = lastExit.exitPoint.position - (newEntrance.exitPoint.position - newRoom.transform.position);
                if (!OccupiedCoordinates(newRoomPosition))
                {
                    newRoom.transform.position = newRoomPosition;

                    lastExit.isUsed = true;
                    newEntrance.isUsed = true;
                    placedRooms.Add(newRoom);                               
                    roomPrefabs.RemoveAt(newRoomIndex);
                    roomPlaced = true;
                }
                else
                {
                    newRoom.gameObject.SetActive(false);
                    roomFailedList.Add(newRoom.gameObject);
                }            
            }
            else
            {
                newRoom.gameObject.SetActive(false);
                roomFailedList.Add(newRoom.gameObject);
            }
        }
    }
    void PlaceOneExitRoom()
    {
        for (int i = placedRooms.Count - 1; i >= 0; i--)
        { 
            Room lastRoom = placedRooms[i];
            Room.Exits lastExit = GetUnusedExit(lastRoom);
            if (lastExit == null) continue;
            int count = 0;
            bool roomPlaced = false;
            while (count < roomOneExitsPrefabs.Count*2  &&  !roomPlaced)
            {
                count++;
                int newRoomIndex = UnityEngine.Random.Range(0, roomOneExitsPrefabs.Count);
                Room newRoom = Instantiate(roomOneExitsPrefabs[newRoomIndex]);
                Room.Exits newEntrance = GetUnusedExit(newRoom);

                if (newEntrance != null && CorrespondingEntrances(lastExit.exitDirections, newEntrance.exitDirections))
                {
                    Vector2 newRoomPosition = lastExit.exitPoint.position - (newEntrance.exitPoint.position - newRoom.transform.position);
                    if (!OccupiedCoordinates(newRoomPosition))
                    {
                        newRoom.transform.position = newRoomPosition;
                        lastExit.isUsed = true;
                        newEntrance.isUsed = true;
                        placedRooms.Add(newRoom);
                        roomOneExitsPrefabs.RemoveAt(newRoomIndex);
                        roomPlaced = true;
                    }
                    else
                    {
                        newRoom.gameObject.SetActive(false);
                        roomFailedList.Add(newRoom.gameObject);
                    }
                }
                else
                {
                    newRoom.gameObject.SetActive(false);
                    roomFailedList.Add(newRoom.gameObject);
                }
            }
            if (roomPlaced) break;
        }
    }
    void MarkOverlappingExits()
    {
        List<Room.Exits> allUnusedExits = new List<Room.Exits>();
        foreach (Room room in placedRooms)
        {
            foreach (Room.Exits exit in room.exits)
            {
                if (!exit.isUsed)
                {
                    allUnusedExits.Add(exit);
                }
            }
        }
        for (int i = 0; i < allUnusedExits.Count; i++)
        {
            for (int j = i + 1; j < allUnusedExits.Count; j++)
            {
                if (Vector2.Distance(allUnusedExits[i].exitPoint.position, allUnusedExits[j].exitPoint.position) < 0.1f)
                {
                    allUnusedExits[i].isUsed = true;
                    allUnusedExits[j].isUsed = true;
                }
            }
        }
    }
    void BlockEedundantExits()
    {
        if (wallPrefabs == null || wallPrefabs.Count == 0)
        {
            DebugLogger.LogWarning("WallPrefabs is empty! Cannot block redundant exits.");
            return;
        }
        foreach (Room room in placedRooms)
        {
            foreach (Room.Exits exit in room.exits)
            {
                if (!exit.isUsed)
                {                   
                    int wallIndex = UnityEngine.Random.Range(0, wallPrefabs.Count);
                    Room wallRoom = Instantiate(wallPrefabs[wallIndex]);    
                    Room.Exits wallEntrance = GetUnusedExit(wallRoom);                 
                    if (wallEntrance != null && CorrespondingEntrances(exit.exitDirections, wallEntrance.exitDirections))
                    {                       
                        Vector2 wallPosition = exit.exitPoint.position - (wallEntrance.exitPoint.position - wallRoom.transform.position);
                        wallRoom.transform.position = wallPosition;
                        wallRoom.transform.SetParent(this.gameObject.transform);
                        exit.isUsed = true;
                    }
                    else
                    {                       
                        wallRoom.gameObject.SetActive(false);
                        roomFailedList.Add(wallRoom.gameObject);                   
                        continue;
                    }
                }
            }
        }
    }    
    bool OccupiedCoordinates(Vector3 position)
    {
        foreach (Room placedroom in placedRooms)
        {
            if (Vector3.Distance( placedroom.transform.position,position) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }
    int CountUnusedExits()
    {
        int count = 0;
        foreach(Room room in placedRooms)
        {
            foreach (Room.Exits exit in room.exits)
            {
                if(!exit.isUsed) count++;
            }
        }    
        return count;
    }

    Room.Exits GetUnusedExit(Room room)
    {
        List<Room.Exits> UnusedExits = new List<Room.Exits>();  
        foreach(Room.Exits exit in room.exits)
        {
            if(!exit.isUsed)
            {
                UnusedExits.Add(exit);  
            }
        }
        if(UnusedExits.Count == 0)
        {
            return null;
        }
        return UnusedExits[UnityEngine.Random.Range(0, UnusedExits.Count)];   
    }
    void DeleteRoomFailed()
    {
        foreach (GameObject failedRoom in roomFailedList)
        {
            Destroy(failedRoom);  
        }
        roomFailedList.Clear();
    }
    bool CorrespondingEntrances(Room.Direction exit,Room.Direction entrance )
    {
        switch (exit)
        {
            case Room.Direction.Up: return entrance == Room.Direction.Down;
            case Room.Direction.Down: return entrance == Room.Direction.Up;
            case Room.Direction.Left: return entrance == Room.Direction.Right;
            case Room.Direction.Right: return entrance == Room.Direction.Left;
            default: return false;
        }
    }

    void SaveDungeonLayout()
    {
        if (DungeonDataManager.Instance == null) return;

        // Đảm bảo load data của scene hiện tại trước khi lưu để không ghi đè data của RoomManager khác trong cùng scene
        if (DungeonDataManager.Instance.HasData())
        {
            DungeonDataManager.Instance.LoadData(this.sceneName);
        }

        this.dungeonLayoutData = new DungeonLayoutData();
        foreach (Room room in placedRooms)
        {
            this.roomInstanceData = new RoomInstanceData
            {
                prefabName = GetPrefabName(room),
                position = room.transform.position,
                rotation = room.transform.rotation,
                exitsUsed = room.exits.Select(e => e.isUsed).ToArray()
            };
            this.dungeonLayoutData.rooms.Add(this.roomInstanceData);
        }

        // Lưu layout với ID của RoomManager này (merge với data hiện có)
        DungeonDataManager.Instance.SetLayout(sceneName, managerID, dungeonLayoutData);
        DungeonDataManager.Instance.SaveData();
    }

    private bool LoadDungeonLayout(DungeonLayoutData data)
    {
        if (data == null || data.rooms == null || data.rooms.Count == 0) return false;

        placedRooms.Clear();
        foreach (RoomInstanceData roomData in data.rooms)
        {
            Room prefab = FindPrefabByName(roomData.prefabName);
            if (prefab == null)
            {
                DebugLogger.LogWarning($"Prefab not found for saved room: {roomData.prefabName}");
                continue;
            }
            Room room = Instantiate(prefab, roomData.position, roomData.rotation);
            if (roomData.exitsUsed != null && roomData.exitsUsed.Length == room.exits.Length)
            {
                for (int i = 0; i < room.exits.Length; i++)
                {
                    room.exits[i].isUsed = roomData.exitsUsed[i];
                }
            }
            placedRooms.Add(room);
        }
        return placedRooms.Count > 0;
    }

    void ValidateAndResetRedundantExits()
    {
        foreach (Room room in placedRooms)
        {
            foreach (Room.Exits exit in room.exits)
            {
                if (exit.isUsed)
                {
                    bool hasConnectedRoom = false;                 
                    foreach (Room otherRoom in placedRooms)
                    {
                        if (otherRoom == room) continue;                       
                        foreach (Room.Exits otherExit in otherRoom.exits)
                        {
                            if (CorrespondingEntrances(exit.exitDirections, otherExit.exitDirections))
                            {
                                Vector2 expectedPosition = exit.exitPoint.position - (otherExit.exitPoint.position - otherRoom.transform.position);
                                if (Vector2.Distance(otherRoom.transform.position, expectedPosition) < 0.1f)
                                {
                                    hasConnectedRoom = true;
                                    break;
                                }
                            }
                        }
                        if (hasConnectedRoom) break;
                    }
                    if (!hasConnectedRoom)
                        exit.isUsed = false;
                }
            }
        }
    }

    private Room FindPrefabByName(string prefabName)
    {
        if (string.IsNullOrEmpty(prefabName)) return null;

        Room Search(List<Room> list) => list.FirstOrDefault(r => r != null && r.name == prefabName);

        Room found = Search(roomPrefabs);
        if (found != null) return found;
        found = Search(roomOneExitsPrefabs);
        if (found != null) return found;
        found = Search(wallPrefabs);
        return found;
    }

    private string GetPrefabName(Room room)
    {
        string name = room.name;
        int idx = name.IndexOf("(Clone)", System.StringComparison.Ordinal);
        if (idx >= 0)
        {
            name = name.Substring(0, idx);
        }
        return name.Trim();
    }
}
