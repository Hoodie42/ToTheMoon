using UnityEngine;
using System.Collections;

public class CloudBehaviour : MonoBehaviour {

	public float speed;
	public float timer;

	private bool stop;
	private bool reset;

	public void spawn() {
		stop = false;
		reset = false;
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

		if (!stop) transform.Translate (0f, speed, 0f);
	}
}
