using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour {

	// 1 - Designer variables
	public AudioClip collSound;
	/// <summary>
	/// Damage inflicted
	/// </summary>
	public int damage = 1;

	/// <summary>
	/// Projectile damage player or enemies?
	/// </summary>
	public bool isEnemyShot = false;

	void Start()
	{
		
//		this.transform.SetParent(gameObject.);
		// 2 - Limited time to live to avoid any leak
		Destroy(gameObject, (float)2.33); // 20sec
	}
	
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// Is this a shot?
		
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if(shot != null)
		{
			if(shot.isEnemyShot && !isEnemyShot){
				shot.destroy();
				destroy();
			}
			
				
		}
		if (otherCollider.CompareTag("Wall"))
		{
			SoundManager.instance.PlaySingleColl(collSound);
			destroy();
		}
		if (otherCollider.CompareTag("Door"))
		{
			SoundManager.instance.PlaySingleColl(collSound);
			destroy();
		}
		
	}

	void destroy()
	{
		Destroy(gameObject, 0.1f);
	}

}
