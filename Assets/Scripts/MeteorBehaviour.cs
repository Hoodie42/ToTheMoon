﻿using UnityEngine;
using System.Collections;

public class MeteorBehaviour : MonoBehaviour {

	public float Speed = -0.01f;
	private float timer;
	private float scale;
	private float rotation;
	
	private bool stop;
	private bool reset;
	
	public void spawn() {
		timer = 12.5f;
		scale = Random.Range (0.3f, 0.9f);
		rotation = Random.Range (-0.5f, 0.5f);
		stop = false;
		reset = false;

		transform.localScale = new Vector3(scale, scale, scale);
		transform.Find("MeteorRot").Rotate (0f, 0f, Random.Range (0f, 360f));
	}
	
	void Start () {
		spawn ();
	}
	
	void Update () {
		stop = GameObject.Find("PlayerShip").GetComponent<PlayerShip>().isDead();
		reset = GameObject.Find("PlayerShip").GetComponent<PlayerShip>().getRespawnStuff();
		
		if (timer > 0.0f) timer -= 1f * Time.deltaTime;
		else if (!stop) Destroy(this.gameObject);
		if (reset) Destroy(this.gameObject);
		
		if (!stop) transform.Translate (0f, Speed * Time.deltaTime, 0f);
		if (!stop) transform.Find("MeteorRot").Rotate (0f, 0f, rotation);
	}
}
