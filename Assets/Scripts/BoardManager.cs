using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
	
	public int rows;
	public int columns;

	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] enemies;


	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	public List<List<int>> level;
	
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
	
	private void SetupBoard()
	{
		boardHolder = new GameObject ("Board").transform;

		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for(int x = -1; x < columns + 1; x++)
		{
			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for(int y = -1; y < rows + 1; y++)
			{
				//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
				GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];

				//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
				if(x == -1 || x == columns || y == -1 || y == rows)
					toInstantiate = wallTiles [Random.Range (0, wallTiles.Length)];

				//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
				GameObject instance =
					Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				instance.transform.SetParent (boardHolder);
			}
		}
	}
	
	public void SetupScene()
	{
		level = new List<List<int>> ();

//		LoadLevelFromCSV(levelNr);
//		reset board
		SetupBoard();
		InitializeGrid();
		
		//Instantiate the cookie in the middle :>
//		Instantiate (cookieExit, new Vector3 (columns / 2, rows / 2, 0f), Quaternion.identity);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
