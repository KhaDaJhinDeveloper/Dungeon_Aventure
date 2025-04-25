using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpawnEnemy))]
public class RoomManager : MonoBehaviour
{
    public  List<Room> roomPrefabs = new List<Room>();  
    public  List<Room> roomOneExitsPrefabs = new List<Room>();   
    public  List<Room> wallPrefabs = new List<Room>(); 
    [HideInInspector] public List<Room> placedRooms = new List<Room>();  
    List<GameObject> roomFailedList = new List<GameObject>();
    public int roomCount;  
    private void Awake()
    {
        GenerateLevel();
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
    }
    public void GenerateLevel()
    {       
        Room startingRoom = Instantiate(roomPrefabs[0], transform.parent.position, Quaternion.identity);
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
}
