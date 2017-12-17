using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpScript : MonoBehaviour
{

	private WeaponScript[] weapons;
	public AudioClip fireball;
	void Awake()
	{
		// Retrieve the weapon only once
		weapons = GetComponentsInChildren<WeaponScript>();
	}

	void Update()
	{
		foreach (WeaponScript weapon in weapons)
		{
			// Auto-fire
			if (weapon != null && weapon.CanAttack)
			{
				SoundManager.instance.PlaySingleBoss(fireball); 
				weapon.AttackPlayer(true);
			}
		}
	}
}
