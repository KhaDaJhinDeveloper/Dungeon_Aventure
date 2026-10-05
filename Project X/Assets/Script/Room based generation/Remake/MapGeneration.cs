using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class MapGeneration : MonoBehaviour
{

    #region Input
    //LayoutData
    public string MapID;
    [SerializeField] private MapLayout mapLayout;
    //LayoutMap
    [SerializeField] private int roomCountTarget;
    private List<RoomData> roomBody ;
    private List<RoomData> roomExit ;
    private RoomData[] wall;
    public Dictionary<Vector2Int, PlacedRoomNode> gridMap { get; private set; } = new();
    public Dictionary<string, RoomData> backUp{ get; private set; } = new();
    private List<OpenExit> exitsList = new();
    private List<EnemySpawnData> enemiesDataSpawn = new();
    private int boxQty;
    private int bonFireQty;
    //SpawnObject
    public List<Transform> boxPointsList {  get; private set; } = new();
    public List<Transform> enemiesPointsList { get; private set; } = new();
    public List<Transform> bonFiresPointList { get; private set; } = new();
    #endregion

    private void Start()
    {
        SetLayout();
        if (DungeonDataManager.Instance.HasData() && DungeonDataManager.Instance.HasMapData(this.MapID))
            DungeonDataManager.Instance.LoadData(this);
        else
            GenerateMap();
        EventManager.Instance?.TriggerEvent(NameEvent.Event_NavMeshSetUp);
    }
    #region GenerateMap
    private void GenerateMap()
    {
        PlaceRoomStart();
    }
    public void SetLayout()
    {
        this.backUp.Clear();
        this.gridMap.Clear();
        this.exitsList.Clear();
        this.enemiesDataSpawn.Clear();
        this.boxQty = this.mapLayout.boxQty;
        this.bonFireQty = this.mapLayout.bonFireQty;
        this.roomBody = new List<RoomData>(this.mapLayout.roomBody);
        this.roomExit = new List<RoomData>(this.mapLayout.roomExit);
        this.wall = new RoomData[this.mapLayout.wall.Length];
        this.mapLayout.wall.CopyTo(this.wall, 0);
        this.enemiesDataSpawn = new List<EnemySpawnData>(this.mapLayout.enemyDataSpawnList);
        foreach (var body in this.mapLayout.roomBody)
            this.backUp[body.gameObject.name] = body;
        foreach (var body in this.mapLayout.roomExit)
            this.backUp[body.gameObject.name] = body;
    }
    private void PlaceRoomStart()
    {
        Vector2Int startPos = Vector2Int.zero;
        RoomData roomStart = this.roomBody[0];
        PlacedRoomNode startNode = new PlacedRoomNode(roomStart, startPos);
        this.gridMap.Add(startPos, startNode);
        foreach(var exits in roomStart.exits)
        {
            OpenExit openExit = new OpenExit
            {
                fromGridPos = startPos, exitDirection = exits.exitDirections
            };
            this.exitsList.Add(openExit);
        }
        this.roomBody.Remove(roomStart);
        PlaceRoomBody();
    }
    private void PlaceRoomBody()
    {
        int maxLoop = 0;
        while (maxLoop < 500 && this.gridMap.Count < this.roomCountTarget && this.exitsList.Count > 0 && this.roomBody.Count > 0)
        {
            maxLoop++;
            int randomIndex = Random.Range(0, this.exitsList.Count);
            OpenExit currentExit = this.exitsList[randomIndex];
            this.exitsList.RemoveAt(randomIndex);

            Vector2Int targetPos = currentExit.fromGridPos + currentExit.exitDirection.ConvertVector2Int();
            if (this.gridMap.ContainsKey(targetPos))
                continue;
            Direction requiredEntrance = currentExit.exitDirection.GetOppositeDir();

            List<RoomData> prefabsValid = GetValidRoom(this.roomBody, requiredEntrance);
            if(prefabsValid.Count > 0)
            {
                RoomData selectPrefabs =prefabsValid[Random.Range(0, prefabsValid.Count)];
                PlacedRoomNode newNode = new PlacedRoomNode(selectPrefabs, targetPos);
                this.gridMap.Add(targetPos, newNode);
                this.roomBody.Remove(selectPrefabs);
                foreach (var exit in selectPrefabs.exits)
                {
                    OpenExit openExit = new OpenExit
                    {
                        fromGridPos = targetPos,
                        exitDirection = exit.exitDirections
                    };
                    this.exitsList.Add(openExit);
                }
            }                 
        }
        PlaceRoomExit();
    }    
    private List<RoomData> GetValidRoom(List<RoomData> listRoom, Direction requiredEntrance)
    {
        List<RoomData> valid = new();
        foreach(var roomValid in listRoom)
        {
            foreach(var exit in roomValid.exits)
            {
                if (exit.exitDirections == requiredEntrance)
                {
                    valid.Add(roomValid);
                    break;
                }
            }
        }
        return valid;
    }
    private void PlaceRoomExit()
    {
        if (mapLayout.roomExit == null || mapLayout.roomExit.Length == 0 || this.exitsList.Count == 0) return;
        for(int i = this.exitsList.Count - 1; i > 0; i--)
        {      
            Vector2Int targetPos = this.exitsList[i].fromGridPos + this.exitsList[i].exitDirection.ConvertVector2Int();
            if (!this.gridMap.ContainsKey(targetPos))
            {
                float dist = Vector2Int.Distance(Vector2Int.zero, targetPos);
                if(dist > 0)
                {
                    Direction requiredEntrance = this.exitsList[i].exitDirection.GetOppositeDir();
                    List<RoomData> prefabsValid = GetValidRoom(this.roomExit, requiredEntrance);
                    if (prefabsValid.Count > 0)
                    {
                        RoomData selectPrefabs = prefabsValid[Random.Range(0, prefabsValid.Count)];
                        PlacedRoomNode newNode = new PlacedRoomNode(selectPrefabs, targetPos);
                        this.gridMap.Add(targetPos, newNode);
                        this.roomExit.Remove(selectPrefabs);
                    }
                }
            }
        }
        RenderRoom();
    }
    public bool IsExitConnected(PlacedRoomNode currentRoom, Direction exitDir)
    {
        Vector2Int neighborPos = currentRoom.gridPos + exitDir.ConvertVector2Int();
        if (!this.gridMap.ContainsKey(neighborPos))
            return false; 

        PlacedRoomNode neighborRoom = this.gridMap[neighborPos];
        Direction oppositeDir = exitDir.GetOppositeDir();
        if (neighborRoom.HasExitDirection(oppositeDir))
            return true; 

        return false;
    }
    public void RenderRoom()
    {
        if (this.gridMap.Count == 0) return;
        foreach (var pair in this.gridMap)
        {
            Vector2Int gridPos = pair.Key;
            PlacedRoomNode node = pair.Value;
            Vector3 worldPos = new Vector3(gridPos.x * 26, gridPos.y * 16, 0f);
            RoomData room = Instantiate(node.prefab, this.transform);
            room.transform.localPosition = worldPos;
            room.transform.localRotation = Quaternion.identity;
            foreach (var exit in room.exits)
            {
                if (!IsExitConnected(node, exit.exitDirections))
                    BlockExit(exit);              
            }
            if (room.enemiesPoint.Length > 0 )
                this.enemiesPointsList.AddRange(room.enemiesPoint);
            if(room.boxPoints.Length > 0)
                this.boxPointsList.AddRange(room.boxPoints);
            if(room.bonFirePoint != null)
                this.bonFiresPointList.Add(room.bonFirePoint);
        }
        PlacedObject();
    }
    private void BlockExit(ExitsRoom exit)
    {
        Direction requiredEntrance = exit.exitDirections.GetOppositeDir();
        foreach (var wall in this.wall)
        {
            foreach (var e in wall.exits)
            {
                if (e.exitDirections == requiredEntrance)
                {
                    RoomData room = Instantiate(wall, exit.exitPoint.position,Quaternion.identity, this.transform);
                    return;
                }    
            }
        }
    }
    #region Place Object
    private void PlacedObject()
    {
        SpawnBonFire();
        SpawnEnemies();
        SpawnBox();

    }
    private void SpawnEnemies()
    {
        List<EnemySpawnData> valid = GetEnemiesValid();
        if (valid.Count <= 0) return;
        foreach (var point in this.enemiesPointsList)
        {
            int index = Random.Range(0, valid.Count);
            GameObject enemy = ObjectPooling.Instance.GetPool(valid[index].enemyPool);
            if(enemy != null) 
                enemy.transform.position = point.position;
        }
    }
    private void SpawnBox()
    {
        int index = 0;
        if (this.boxPointsList.Count <= 0) return;
        this.boxPointsList = ShuffleList(this.boxPointsList);
        while(index < this.boxQty)
        {
            foreach(var pos in this.boxPointsList)
            {
                GameObject boxObj = ObjectPooling.Instance.GetPool(KeyPool.KEY_INTERACT_BOX);
                boxObj.transform.position = pos.position;
                index++;
            }
        }
    }
    private void SpawnBonFire()
    {
        int index = 0;
        if (this.bonFiresPointList.Count <= 0)
            return;
        this.bonFiresPointList = ShuffleList(this.bonFiresPointList);
        while (index < this.bonFireQty)
        {
            foreach (var pos in this.bonFiresPointList)
            {
                GameObject bonfire = ObjectPooling.Instance.GetPool(KeyPool.KEY_INTERACT_BONFIRE);
                bonfire.transform.position = pos.position;
                index++;
            }
        }
    }
    private List<EnemySpawnData> GetEnemiesValid()
    {
        List<EnemySpawnData> valid = new();

        foreach(var enemyDataSpawn in this.enemiesDataSpawn)
        {
            if(enemyDataSpawn.unlockLevel >= GameControl.Instance.LevelOfDanger)
                valid.Add(enemyDataSpawn);
        }
        return valid;
    }
    private List<Transform> ShuffleList(List<Transform> list)
    {
        int i = list.Count - 1;
        for(int j = i ; j > 0; j --)
        {
            int inDex = Random.Range(0, j + 1);
            var temp = list[j];
            list[j] = list[inDex];
            list[inDex] = temp;
        }
        return list;
    }    
    #endregion
    #endregion
}
