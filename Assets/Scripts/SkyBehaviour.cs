using UnityEngine;
using System.Collections;

public class SkyBehaviour : MonoBehaviour {

	private float speed = 0.15f;
	private bool astarted;
	private bool started;
	private bool inAir;
	private bool dead;
	private float posY;
	private float timer;
	
	public void spawn() {
		astarted = false;
		started = false;
		dead = false;
		posY = 0f;
		timer = 2.5f;
	}
	
	void Start() {
		spawn();
	}
	
	void Update () {
		//Retrieve information
		started = GameObject.Find("PlayerShip").GetComponent<PlayerShip>().getStarted();
		dead = GameObject.Find("PlayerShip").GetComponent<PlayerShip>().isDead();
		
		//Update Pos
		transform.position = new Vector3 (0f, posY, 0f);
		
		//Anim
		if (started) {
			if (timer > 0.0f) timer -= 0.01f;
			else if (!dead) {
				posY -= speed;
				if (posY < -47f) posY = -47f;
			}
		}
	}
}
