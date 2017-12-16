using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RoomGen;
using RoomGen.Enums;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;	

	public BoardManager boardScript;

	public int playerLives = 8;
	private int numOfLevels = 4;

	public List<Room> rooms; // global room map
	
	// Use this for initialization
	void Awake () 
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		boardScript = GetComponent<BoardManager>();
		InitGame();
	}
	
	
	void InitGame()
	{
		rooms = new List<Room>(); // for new game empty rooms
		int[][] randLvls = LevelGenerator.Instance.GenerateLevel(1);
		List<Room> possibleRooms = new List<Room>();
		for (int i = 1; i <= numOfLevels; i++)
		{
			possibleRooms.Add(ReadJSON.loadRoomWithId(i));
		}
		
		for (int x = 0; x < randLvls.Length; x++)
		{
			for (int y = 0; y < randLvls[x].Length; y++)
			{
				RoomType type = (RoomType) randLvls[x][y];
				Room r = null;
				switch (type)
				{
					case RoomType.Spawn:
						List<Room> spawnList = new List<Room>();
						spawnList.Add(ReadJSON.loadRoomWithId(0));
						r = findFittingRoom(randLvls, spawnList, x, y);
						break;
					case RoomType.Normal:
						r = findFittingRoom(randLvls, possibleRooms, x, y);
						break;
					case RoomType.Bonus:
						r = findFittingRoom(randLvls, possibleRooms, x, y);
						break;
					case RoomType.Boss:
						r = findFittingRoom(randLvls, possibleRooms, x, y);
						break;
					case RoomType.Key:
						r = findFittingRoom(randLvls, possibleRooms, x, y);
						break;
						
				}
				if (r != null)
				{
					r.x = x;
					r.y = y;
					rooms.Add(r);
				}
				Debug.Log(rooms);
			}
		}
		LoadLevelWithCoords(7, 7);
	}

	private Room findFittingRoom(int[][] overmap, List<Room> possibleRooms, int x, int y)
	{
		bool left = overmap[x - 1][y] != (int) RoomType.Null;
		bool right = overmap[x + 1][y] != (int) RoomType.Null;
		bool top = overmap[x][y + 1] != (int) RoomType.Null;
		bool bottom = overmap[x][y - 1] != (int) RoomType.Null;

		var fitting = possibleRooms
			.Where(r => (r.doorBottom == bottom || r.doorBottom)
			            && (r.doorTop == top || r.doorTop)
			            && (r.doorLeft == left || r.doorLeft) 
			            && (r.doorRight == right || r.doorRight)).ToList();
		Room finalRoom = fitting[Util.GetRandomDistinctNumbers(fitting.Count).First()].Copy();
		finalRoom.realDoorLeft = left;
		finalRoom.realdoorRight = right;
		finalRoom.realdoorTop = top;
		finalRoom.realdoorBottom = bottom;
		return finalRoom;
	}

	public void LoadLevelWithCoords(int x, int y)
	{
		//TODO think about where to spawn the player!
		if (x == 7 && y == 7) // special case where player starts in the middle
		{
			//
		}
		if (rooms != null)
		{
			Room current = rooms.FirstOrDefault(r => r.x == x && r.y == y);
			
			if (current != null)
			{
				boardScript.SetupScene(current);
			}	
		}
		
	}

	public void GameOver()
	{
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
