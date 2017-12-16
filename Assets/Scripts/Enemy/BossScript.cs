using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossScript : MonoBehaviour {

	// 1 - Designer variables

	public float X_WallOffset = 2.0f;
	public float Y_WallOffset = 2.0f;
	
	/// <summary>
	/// Object speed
	/// </summary>
	public Vector2 speed = new Vector2(2, 2);

	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, -1);

	private Vector2 movement;
	private Rigidbody2D rigidbodyComponent;

	void Update()
	{
		// 2 - Movement
		movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y);
		
		//RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left);
		var raycastLeft = Physics2D.RaycastAll(transform.position, Vector2.left,X_WallOffset);
		var raycastRight = Physics2D.RaycastAll(transform.position, Vector2.right,X_WallOffset);
		var raycastTop = Physics2D.RaycastAll(transform.position, Vector2.up,Y_WallOffset);
		var raycastBot = Physics2D.RaycastAll(transform.position, Vector2.down,Y_WallOffset);
		if (raycastLeft.Length >= 1)
		{
        	if (raycastLeft.Any(x=>x.collider.name == "Wall(Clone)"))
			{
				direction = new Vector2(1,direction.y);
			}
		}
		if (raycastRight.Length >=1)
		{
			if (raycastRight.Any(x=>x.collider.name == "Wall(Clone)"))
			{
				direction = new Vector2(-1,direction.y);
			}
		}
		if (raycastTop.Length >=1)
		{
			if (raycastTop.Any(x=>x.collider.name == "Wall(Clone)"))
			{
				direction = new Vector2(direction.x,-1);
			}
		}
		if (raycastBot.Length >=1)
		{
			if (raycastBot.Any(x=>x.collider.name == "Wall(Clone)"))
			{
				direction = new Vector2(direction.x,1);
			}
		}
	}

	void FixedUpdate()
	{
		if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

		// Apply movement to the rigidbody
		rigidbodyComponent.velocity = movement;
		
	}
}
