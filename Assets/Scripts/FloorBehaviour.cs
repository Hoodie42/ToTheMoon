using UnityEngine;
using System.Collections;

public class FloorBehaviour : MonoBehaviour {

	private bool astarted;
	private bool started;
	private bool inAir;
	private bool dead;
	private float posY;

	public void spawn() {
		astarted = false;
		started = false;
		inAir = false;
		dead = false;
		posY = 0f;

		transform.Find ("takeOff").GetComponent<ParticleSystem>().Stop();
	}
	
	void Start() {
		spawn();
		transform.Find("floorspr").GetComponent<SpriteRenderer>().enabled = true;
	}

	void Update () {
		//Retrieve information
		started = GameObject.Find("PlayerShip").GetComponent<PlayerShip>().getStarted();
		inAir = GameObject.Find("PlayerShip").GetComponent<PlayerShip>().getInAir();
		dead = GameObject.Find("PlayerShip").GetComponent<PlayerShip>().isDead();

		//Update Pos
		transform.position = new Vector3 (0f, posY, 0f);

		//Anim
		if (started && !astarted) {
			transform.Find("takeOff").GetComponent<ParticleSystem>().Play();
			astarted = true;
		}
		if (inAir && !dead) {
			posY -= 6f * Time.deltaTime;
			if (posY < -3f) posY = -3f;
		}
	}
}
