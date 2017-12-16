using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

	//--------------------------------
	// 1 - Designer variables
	//--------------------------------

	/// <summary>
	/// Projectile prefab for shooting
	/// </summary>
	public Transform shotPrefab;
	private GameObject Player;

	/// <summary>
	/// Cooldown in seconds between two shots
	/// </summary>
	public float shootingRate = 0.25f;

	//--------------------------------
	// 2 - Cooldown
	//--------------------------------

	private float shootCooldown;

	void Start()
	{
		shootCooldown = 0f;
		Player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
		}
	}

	//--------------------------------
	// 3 - Shooting from another script
	//--------------------------------

	/// <summary>
	/// Create a new projectile if possible
	/// </summary>
	public void Attack(bool isEnemy,int direction)
     	{
     		if (CanAttack)
     		{
     			shootCooldown = shootingRate;
     
     			// Create a new shot
     			var shotTransform = Instantiate(shotPrefab) as Transform;
			    shotTransform.SetParent(gameObject.transform);
     			// Assign position
     			shotTransform.position = transform.position;
     
     			// The is enemy property
     			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
     			if (shot != null)
     			{
     				shot.isEnemyShot = isEnemy;
     			}
     
     			// Make the weapon shot always towards it
     			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
     			if (move != null)
     			{
                     if (direction == 2)
                     {
     	                
                         move.direction = this.transform.right; // towards in 2D space is the right of the sprite
                     }
                     if (direction == 0)
                     {
                         move.direction = this.transform.right + Vector3.left*2; // towards in 2D space is the left of the sprite
                     }
                     if (direction == 1)
                     {
                         move.direction = this.transform.right + Vector3.left + Vector3.up; // towards in 2D space is the up of the sprite
                     }
                     if (direction == 3)
                     {
                         move.direction = this.transform.right + Vector3.left + Vector3.down; // towards in 2D space is the down of the sprite
                     }
                 }
     		}
     	}
	
	public void AttackAngled(bool isEnemy,Vector2 direction)
	{
		if (direction == new Vector2(0, 0))
		{
			direction = new Vector2(1,0);
		}
		if (CanAttack)
		{
			shootCooldown = shootingRate;

			// Create a new shot
			var shotTransform = Instantiate(shotPrefab) as Transform;
			shotTransform.SetParent(gameObject.transform);

			// Assign position
			shotTransform.position = transform.position;

			// The is enemy property
			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
			if (shot != null)
			{
				shot.isEnemyShot = isEnemy;
			}

			// Make the weapon shot always towards it
			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
			if (move != null)
			{
				//new Vector2((transform.position.x - player.position.x) * speed, (transform.position.y - player.position.y) * speed);
				move.direction = direction;
			}
		}
	}
	
	public void AttackPlayer(bool isEnemy)
	{
		//Transform target = GameObject.FindWithTag ("Player").transform;
		if (CanAttack)
		{
			shootCooldown = shootingRate;

			// Create a new shot
			var shotTransform = Instantiate(shotPrefab) as Transform;

			// Assign position
			shotTransform.position = transform.position;

			// The is enemy property
			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
			if (shot != null)
			{
				shot.isEnemyShot = isEnemy;
			}

			// Make the weapon shot always towards it
			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
			if (move != null)
			{
				move.direction = new Vector2((-transform.position.x+Player.transform.position.x)
					,(-transform.position.y+Player.transform.position.y)).normalized;
				
			}
		}
	}

	/// <summary>
	/// Is the weapon ready to create a new projectile?
	/// </summary>
	public bool CanAttack
	{
		get
		{
			return shootCooldown <= 0f;
		}
	}
}
