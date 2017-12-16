using System;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Room
{
	public int rows;
	public int cols;
	public int[] map;

	public bool doorLeft;
	public bool doorRight;
	public bool doorTop;
	public bool doorBottom;
	
	public bool realDoorLeft;
	public bool realdoorRight;
	public bool realdoorTop;
	public bool realdoorBottom;

	//location on overmap
	public int x;
	public int y;
	
	private bool cleared = false;

	public static Room CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<Room>(jsonString);
	}

	public Room Copy()
	{
		Room r = new Room
		{
			rows = rows,
			cols = cols,
			map = new int[map.Length],
			doorLeft = doorLeft,
			doorRight = doorRight,
			doorTop = doorTop,
			doorBottom = doorBottom,
			
			x = x,
			y = y
				
		};
		map.CopyTo(r.map, 0);
		return r;
	}
}