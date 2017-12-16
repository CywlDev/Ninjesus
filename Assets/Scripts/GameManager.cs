using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RoomGen;
using RoomGen.Enums;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;	

	public BoardManager boardScript;

	public int playerLives = 8;
	//limit num of levels here (pls) dirty and fast
	private int numOfLevels = 15;
	private int numOfBossLevels = 2;
	private int numOfKeyLevels = 1;
	
	public GameObject player;
	public bool hasKey = false;

	public List<Room> rooms; // global room map
	public Text levelText;

	private int score;
	public Text scoreText;
	
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
		
		List<Room> possibleBossRooms = new List<Room>();
		for (int i = 1; i <= numOfBossLevels; i++)
		{
			possibleBossRooms.Add(ReadJSON.loadBossRoomWithId(i));
		}
		
		List<Room> possibleKeyRooms = new List<Room>();
		for (int i = 1; i <= numOfKeyLevels; i++)
		{
			possibleKeyRooms.Add(ReadJSON.loadKeyRoomWithId(i));
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
						r = findFittingRoom(randLvls, possibleRooms, x, y); //TODO throw not implemented exception ;)
						break;
					case RoomType.Boss:
						r = findFittingRoom(randLvls, possibleBossRooms, x, y);
						break;
					case RoomType.Key:
						r = findFittingRoom(randLvls, possibleKeyRooms, x, y);
						break;
					case RoomType.Trap:
						r = findFittingRoom(randLvls, possibleRooms, x, y); //TODO throw not implemented exception ;)
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
		player = Instantiate (player, new Vector3 (7, 7, 0f), Quaternion.identity) as GameObject;
		HealthScript hs = player.GetComponent<HealthScript>();
		hs.hp = playerLives;

		LoadLevelWithCoords(7, 7);
	}

	private string getHealthString(int lives)
	{
		string liveString = "";
		for (int i = 0; i < lives; i++)
		{
			liveString += "@";
		}
		return liveString;
	}
	
	public void addScore(int score)
	{
		this.score += score;
		scoreText.text = "Score: " + this.score.ToString();
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
		if (!boardScript.Cleared)
			return;
		
		//Destroy all shoooots
		foreach (var componentsInChild in player.GetComponentsInChildren<ShotScript>())
		{
			Destroy(componentsInChild.gameObject);
		}
			
		var oldX = boardScript.currentRoom.x;
		var oldY = boardScript.currentRoom.y;
		
		//set room where youre coming from to cleared

		var oldRoom = rooms.FirstOrDefault(r => r.x == oldX && r.y == oldY);
		if (oldRoom != null)
		{
			oldRoom.cleared = true;
		}
		
		
		Position playerPos = Position.Center;
		if (oldX < x) //went right, spawn left
		{
			playerPos = Position.Left;
		}
		if (oldX > x) //went left, spawn right
		{
			playerPos = Position.Right;
		}
		if (oldY < y) //went down, spawn up
		{
			playerPos = Position.Top;
		}
		if (oldY > y) //went up, spawn down
		{
			playerPos = Position.Down;
		}
		
		if (rooms != null)
		{
			Room current = rooms.FirstOrDefault(r => r.x == x && r.y == y);
			
			if (current != null)
			{
				boardScript.SetupScene(current, playerPos);
			}	
		}
		
	}

	public void GameOver()
	{
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null)
		{
			HealthScript hs = player.GetComponent<HealthScript>();
			levelText.text = getHealthString(hs.hp);
		}
	}
}
