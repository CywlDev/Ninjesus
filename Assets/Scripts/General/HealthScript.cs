using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;

public class HealthScript : MonoBehaviour {

	/// <summary>
	/// Total hitpoints
	/// </summary>
	public int hp = 1;

	/// <summary>
	/// Enemy or player?
	/// </summary>
	public bool isEnemy = true;

	
	
	private void DmgColor()
	{
		var obj = this.GetComponentInParent(typeof(PlayerScript));
		obj.GetComponent<SpriteRenderer>().color = new Color(1f,1f, 1f);
	}
	
	/// <summary>
	/// Inflicts damage and check if the object should be destroyed
	/// </summary>
	/// <param name="damageCount"></param>
	public void Damage(int damageCount)
	{
		hp -= damageCount;

		var obj = this.GetComponentInParent(typeof(PlayerScript));
		if (obj != null)
		{
			obj.GetComponent<SpriteRenderer>().color = new Color(1f,0.30196078f, 0.30196078f);
			Invoke("DmgColor", 0.2f);

		}
		
		if (hp <= 0)
		{
			// Dead!
			if (obj != null)
			{
				// todo show game over
			}
			else
			{
				Destroy(gameObject);
				GameManager.instance.addScore(100);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// Is this a shot?
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			// Avoid friendly fire
			if (shot.isEnemyShot != isEnemy)
			{
				Damage(shot.damage);

				// Destroy the shot
				Destroy(shot.gameObject, 0.1f); // Remember to always target the game object, otherwise you will just remove the script
			}
		}
	}
}
