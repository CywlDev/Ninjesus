using System.Collections;
using System.Collections.Generic;
using Completed;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

	public AudioSource jesusSource;
	public AudioSource enemySource;
	public AudioSource bossSource;
	public AudioSource collSource;
	public AudioSource hitSource;

	public AudioSource musicSource;

	public static SoundManager instance = null;

	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;
	
	// Use this for initialization
	void Awake ()
	{

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		
		DontDestroyOnLoad(gameObject);

	}

	public void PlaySingleJesus(AudioClip clip)
	{
		jesusSource.clip = clip;
		jesusSource.Play();
	}
	
	public void PlaySingleEnemy(AudioClip clip)
	{
		if (!enemySource.isPlaying)
		{
			enemySource.clip = clip;
			enemySource.Play();
		}
		
	}
	
	public void PlaySingleBoss(AudioClip clip)
	{
		if (!bossSource.isPlaying)
		{
			bossSource.clip = clip;
			bossSource.volume = 0.5f;
			bossSource.Play();
		}
		
	}

	public void PlaySingleColl(AudioClip clip)
	{
		collSource.clip = clip;
		collSource.Play();
	}
	
	public void PlaySingleHit(AudioClip clip)
	{
		hitSource.clip = clip;
		hitSource.Play();
	}

//	public void RandomizeSfx(params AudioClip[] clips)
//	{
//		int randomIndex = Random.Range(0, clips.Length);
//		float randomPitch = Random.Range(lowPitchRange, highPitchRange);
//
//		efxSource.pitch = randomPitch;
//		efxSource.clip = clips[randomIndex];
//		efxSource.Play();
//	}
	
}
