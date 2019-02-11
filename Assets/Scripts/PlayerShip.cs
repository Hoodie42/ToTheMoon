using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour {
	
	public GameObject Cloud;
	public GameObject Star;
	public GameObject EntityMeteor;
	public GameObject Flash;

	private float forceXDef = 0.006f;
	private float forceXMax = 0.04f;
	private float borderX = 2.8f;
	private float starsTimerDef = 0.2f;
	private float meteorsTimerDef = 1.5f;
	private float scoreTimerDef = 8.0f;

	private bool atouch;
	private bool touched;
	private bool started;
	private bool startswing;
	private bool dead;
	private bool deadOnce, GUIrespawnOnce, GUIrespawnOnce2;
	private bool goRight;
	private bool spawnCloud;
	private bool respawnStuff;
	private bool highscore;
	private float scoreTimer, deadTimer, deadTimer2;
	private float posX;
	private float posY;
	private float forceX;
	private float altitude;
	private float timerStart;
	private float starsTimer;
	private float meteorsTimer;
	private int score, bestscore, minusscore;

	private void spawn() {
		atouch = true;
		touched = false;
		started = false;
		startswing = false;
		dead = false;
		deadOnce = false;
		GUIrespawnOnce = false; GUIrespawnOnce2 = false;
		goRight = false;
		spawnCloud = true;
		respawnStuff = true;
		highscore = false;
		deadTimer = 1.4f; deadTimer2 = 0.5f;
		posY = -3.5f;
		posX = 0f;
		forceX = 0f;
		altitude = 0f;
		timerStart = 2f;
		starsTimer = starsTimerDef; meteorsTimer = meteorsTimerDef;
		score = 0; bestscore = PlayerPrefs.GetInt("TOTHEMOON_highscore", 0); minusscore = 0;
		scoreTimer = scoreTimerDef;

		transform.FindChild("ship").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("GUIdisppaear1").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("GUIdisppaear2").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("newscore").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("touchtorespawn").animation.Play("GUIrespawnInvisible");
		GameObject.Find("GUIscore").animation.Play("SCOREoutscreen");

		transform.FindChild("ship").FindChild("smoke").particleSystem.Stop();
		transform.FindChild("ship").FindChild("Splosion").particleSystem.Stop();
		transform.FindChild("ship").FindChild("Splosion2").particleSystem.Stop();
		GameObject.Find("ScoreSplos").particleSystem.Stop();

		transform.FindChild("ship").animation.Play("ShipIdle");
	}

	void Start() {
		spawn();
	}

	void Update () {
		//Update position all frames
		transform.position = new Vector3 (posX, posY, 0f);
		transform.FindChild("ship").rotation = new Quaternion(0f, 0f, -(forceX*2), 1f);
		if (started && !dead) posX += forceX;

		//Update Score
		GameObject.Find ("GUITextScore1").guiText.text = score.ToString();
		GameObject.Find ("GUITextBest1").guiText.text = bestscore.ToString();
		GameObject.Find ("GUITextScore2").guiText.text = score.ToString();
		GameObject.Find ("GUITextBest2").guiText.text = bestscore.ToString();

		//Touch Gestion
		touched = false;
		if (touch() && !atouch) {
			atouch = true;
			touched = true;
		}
		else if (!touch() && atouch) {
			atouch = false;
		}

		//Border Limits
		if (posY > -1.35f)
			posY = -1.35f;
		if (posX < -borderX)
			dead = true;
			//posX = -borderX;
		else if (posX > borderX)
			dead = true;
			//posX = borderX;

		//Lets go mayte
		if (!started && touched) {
			started = true;
			respawnStuff = false;
			transform.FindChild("ship").animation.Play("ShipAscension");
			transform.FindChild("ship").FindChild("smoke").particleSystem.Play();
			GameObject.Find("GUIdisppaear1").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("GUIdisppaear2").GetComponent<SpriteRenderer>().enabled = false;
		}
		//MAIN GAME
		else if (started && !dead) {
			posY += 0.08f;
			altitude += 0.01f;

			if (timerStart > 0.0f) timerStart-= 0.01f;
			else {
				//Increase Decrease forceX
				if (!goRight)
					forceX -= forceXDef;
				else if (goRight)
					forceX += forceXDef;
				if (forceX > forceXMax)
					forceX = forceXMax;
				else if (forceX < -forceXMax)
					forceX = -forceXMax;

				//Change Direction
				if (!goRight && touched) {
					goRight = true;
				}
				else if (goRight && touched) {
					goRight = false;
				}
			}

			//Score
			if (scoreTimer <= 0) {
				scoreTimer = scoreTimerDef;
				addPoint(1);
			} else {
				scoreTimer -= 0.01f;
			}
		}

		//Changing sky overtime
		if (altitude > 1f && spawnCloud) {
			spawnCloud = false;
			GameObject.Find("GUIscore").animation.Play("SCOREingame");
			createClouds();
		}
		if (altitude > 4f) {
			createStars();
		}
		if (altitude > 5f) {
			createMeteors();
		}

		//DEATH !!
		if (dead && !deadOnce) {
			deadOnce = true;

			transform.FindChild("ship").GetComponent<SpriteRenderer>().enabled = false;
			transform.FindChild("ship").FindChild("smoke").particleSystem.Stop();
			transform.FindChild("ship").FindChild("Splosion").particleSystem.Play();
			transform.FindChild("ship").FindChild("Splosion2").particleSystem.Play();

			if (bestscore < score) {
				bestscore = score;
				PlayerPrefs.SetInt("TOTHEMOON_highscore", bestscore);
				highscore = true;
			}
		}
		if (deadOnce) {
			if (deadTimer <= 0) {
				if (!GUIrespawnOnce) {
					GameObject.Find("touchtorespawn").animation.Play("GUIrespawnVisible");

					//HIGHSCORE ANIMATION
					if (highscore) {
						GameObject.Find("GUInewscore").animation.Play("SCOREnewscore");
						GameObject.Find("ScoreSplos").particleSystem.Play();
					}

					GUIrespawnOnce = true;
				}
			} else {
				deadTimer -= 0.01f;
			}
			if (deadTimer2 <= 0) {
				if (!GUIrespawnOnce2) {
					GameObject.Find("GUIscore").animation.Play("SCOREendgame");
					GUIrespawnOnce2 = true;
				}
			} else {
				deadTimer2 -= 0.01f;
			}
		}

		//RESPAWN
		if (dead && touched && deadTimer <= 0) {
			spawn();
			Instantiate(Flash, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
			GameObject.Find("Floor").GetComponent<FloorBehaviour>().spawn();
			GameObject.Find("Crepuscule").GetComponent<SkyBehaviour>().spawn();
		}
	}

	//TRIGGER
	void OnTriggerEnter2D(Collider2D objectCollision) {
		if (objectCollision.gameObject.tag == "Meteor") {
			dead = true;
		}
	}

	private bool touch() {
		if (Input.GetMouseButton(0)) {
			return true;
		}
		for (int i = 0; i < Input.touchCount; i++) {
			return true;
		}
		return false;
	}

	private void createClouds() {
		for (int i = 0 ; i < 10 ; i++)
			Instantiate(Cloud, new Vector3(Random.Range(-4.1f, 4.1f), 6.5f+i, 0.0f), Quaternion.identity);
	}

	private void createStars() {
		if (!dead && starsTimer <= 0f) {
			Instantiate(Star, new Vector3(Random.Range(-3.5f, 3.5f), 6.0f, 0.0f), Quaternion.identity);
			starsTimer = starsTimerDef;
		}
		else
			starsTimer -= 0.01f;
	}

	private void createMeteors() {
		if (!dead && meteorsTimer <= 0f) {
			Instantiate(EntityMeteor, new Vector3(Random.Range(-3.5f, 3.5f), 6.0f, 0.0f), Quaternion.identity);
			meteorsTimer = meteorsTimerDef;
		}
		else
			meteorsTimer -= 0.01f;
	}

	private void addPoint(int p) {
		GameObject.Find("GUIscore").animation.Play("SCOREaddpoint");
		score += p;
		minusscore = score - 1;
	}

	public bool getStarted() {
		return started;
	}

	public bool isDead() {
		return dead;
	}

	public bool getRespawnStuff(){
		return respawnStuff;
	}

	public bool getInAir() {
		if (posY >= -1.35f)
			return true;
		return false;
	}
}
