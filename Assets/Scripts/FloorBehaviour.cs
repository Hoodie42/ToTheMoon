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

		transform.FindChild ("takeOff").particleSystem.Stop();
	}
	
	void Start() {
		spawn();
		transform.FindChild("floorspr").GetComponent<SpriteRenderer>().enabled = true;
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
			transform.FindChild("takeOff").particleSystem.Play();
			astarted = true;
		}
		if (inAir && !dead) {
			posY -= 0.06f;
			if (posY < -3f) posY = -3f;
		}
	}
}
