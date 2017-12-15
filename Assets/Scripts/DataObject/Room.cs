using UnityEngine;

[System.Serializable]
public class Room
{
	public int rows;
	public int cols;
//	public int[][] map;

	public static Room CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<Room>(jsonString);
	}
}