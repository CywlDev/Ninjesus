using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadJSON : MonoBehaviour {

	// Use this for initialization
//	void Start ()
//	{
//		string jsonString = File.ReadAllText(Application.dataPath + "/Rooms/r1.json");
//		Debug.Log(jsonString);
////		Room room1 = Room.CreateFromJSON(jsonString);
//		Room room1 = Room.CreateFromJSON(jsonString);
////		Room room1 = JsonUtility.FromJson<Room>(jsonString);
//		Debug.Log(room1.cols);
////		object level = JsonUtility.FromJson(jsonString);
//	}

	public static Room loadRoomWithId(int id)
	{
		string jsonString = File.ReadAllText(Application.dataPath + "/Rooms/r"+ id.ToString() + ".json");
		return Room.CreateFromJSON(jsonString);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
