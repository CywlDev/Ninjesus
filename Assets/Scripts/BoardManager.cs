using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RoomGen.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
	
	public int rows;
	public int columns;

	public GameObject ghost;

	public GameObject boss;

	
	public GameObject door;
	
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] enemies;


	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	public List<List<int>> level;
	public Room currentRoom;

	private List<GameObject> aliveEnemies = new List<GameObject>();

	public bool Cleared
	{
		get { return aliveEnemies.All(x => x == null);  }
	}
	
	private void InitializeGrid()
	{
		gridPositions.Clear();

		//Loop through x axis (columns).
		for(int x = 1; x < columns-1; x++)
		{
			//Within each column, loop through y axis (rows).
			for(int y = 1; y < rows-1; y++)
			{
				gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	private void fillFromMap(int[][] level)
	{
		// this does use the internal encoding for enemys and more
		// have a look at the github/wiki
		aliveEnemies = new List<GameObject>();
		for(int y = 0; y < level.Length; y++)
		{
			for(int x = 0; x < level[y].Length; x++)
			{
				GameObject toSpawn;
				bool isEnemy = false;
				int tileType = level[y][x]; //for encoding, see google drive

				switch (tileType) {

					case 1:
						toSpawn = wallTiles [Random.Range (0, wallTiles.Length)];
						break;
					case 6: // ghost
						toSpawn = ghost;
						isEnemy = true;
						break;
					case 5:
						toSpawn = boss;
						isEnemy = true;
						break;
//					case 4:
//						toSpawn = player;
//						break;
//					case 3:
//						toSpawn = items [Random.Range (0, items.Length)];
//						break;
					default:
						toSpawn = null;
						break;

				}

				//instanciate the chosen tile
				if (toSpawn != null)
				{
					if (isEnemy && currentRoom.cleared)
					{
						break;
					}
					GameObject instance = Instantiate (toSpawn, new Vector3 (x, rows - y - 1, 0f), Quaternion.identity) as GameObject;

					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
					instance.transform.SetParent (boardHolder);
					if (isEnemy)
					{
						aliveEnemies.Add(instance);
					}
				}
				
			}
		}
		
	}
	
	private void SetupBoard()
	{
		if (boardHolder != null)
		{
			Destroy(boardHolder.gameObject);
		}
		boardHolder = new GameObject ("Board").transform;

		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for(int x = -1; x < columns + 1; x++)
		{
			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for(int y = -1; y < rows + 1; y++)
			{
				//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
				GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
//				GameObject toInstantiate = null;
				//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
				if (x == -1 || x == columns || y == -1 || y == rows)
				{
					if (x == (columns) / 2 && y == -1 && currentRoom.realdoorBottom) // btm door
					{
						GameObject instance = Instantiate(door, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
						instance.AddComponent<Door>();
						Door d = instance.GetComponent<Door>();
						d.nextX = this.currentRoom.x;
						d.nextY = this.currentRoom.y - 1;
						instance.transform.SetParent(boardHolder);
					}
					else if (x == (columns) / 2 && y == rows && currentRoom.realdoorTop) // top door
					{
						GameObject instance = Instantiate(door, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
						instance.AddComponent<Door>();
						Door d = instance.GetComponent<Door>();
						d.nextX = this.currentRoom.x;
						d.nextY = this.currentRoom.y + 1;
						instance.transform.SetParent(boardHolder);
					}
					else if (y == (rows) / 2 && x == -1 && currentRoom.realDoorLeft) // left door
					{
						GameObject instance = Instantiate(door, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
						instance.AddComponent<Door>();
						Door d = instance.GetComponent<Door>();
						d.nextX = this.currentRoom.x - 1 ;
						d.nextY = this.currentRoom.y;
						instance.transform.SetParent(boardHolder);
					}
					else if (y == (rows) / 2 && x == columns && currentRoom.realdoorRight) // right door
					{
						GameObject instance = Instantiate(door, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
						instance.AddComponent<Door>();
						Door d = instance.GetComponent<Door>();
						d.nextX = this.currentRoom.x + 1 ;
						d.nextY = this.currentRoom.y;
						instance.transform.SetParent(boardHolder);
					}
					else
					{
						toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
					}
						
				}

				if (toInstantiate != null)
				{
					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
					GameObject instance =
						Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
					instance.transform.SetParent(boardHolder);
				}
				
			}
		}
	}
	
	public void LoadRoom(Room room)
	{
		level = new List<List<int>>();
		currentRoom = room;
		
		for (int i = 0; i < room.rows; i++)
		{
			level.Add(room.map.Skip(i * room.cols).Take(room.cols).ToList());
		}

		this.columns = room.cols;
		this.rows = room.rows;
		
		SetupBoard();
		InitializeGrid();
		
		fillFromMap(level.Select(x => x.ToArray()).ToArray());
	}
	
	public void SetupScene(Room r, Position playerPos)
	{
// 		generate random map with rooms
		
		LoadRoom(r);

		Vector3 pos = new Vector3((int)(columns / 2),(int)(rows / 2), 0f);
		if (playerPos == Position.Top)
		{
			pos = new Vector3((int)(columns / 2),0, 0f);
		} else if (playerPos == Position.Down)
		{
			pos = new Vector3((int)(columns / 2),rows-1, 0f);
		} else if (playerPos == Position.Left)
		{
			pos = new Vector3(0,(int)(rows / 2), 0f);
		} else if (playerPos == Position.Right)
		{
			pos = new Vector3(columns-1,(int)(rows / 2), 0f);
		}
		GameManager.instance.player.transform.SetPositionAndRotation(pos, Quaternion.identity);
//		GameObject instance = Instantiate (player, new Vector3 (x, rows - y - 1, 0f), Quaternion.identity) as GameObject;
//		player.transform.SetPositionAndRotation(new Vector3(0f,0f,0f), Quaternion.identity);
		

//		LoadLevelFromCSV(levelNr);
//		reset board
//		SetupBoard();
//		InitializeGrid();
		
		//Instantiate the cookie in the middle :>
//		Instantiate (cookieExit, new Vector3 (columns / 2, rows / 2, 0f), Quaternion.identity);
	}
	
//	private void loadLevelFromJSON()

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
