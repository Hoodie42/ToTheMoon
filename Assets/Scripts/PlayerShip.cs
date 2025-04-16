using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerShip : MonoBehaviour {
	
	public GameObject Cloud;
	public GameObject Star;
	public GameObject EntityMeteor;
	public GameObject Flash;

	private float forceXDef = 6f;
	private float forceXMax = 2f;
	private float borderX = 2.8f;
	private float starsTimerDef = 0.2f;
	private float meteorsTimerDef = 1.5f;
	private float scoreTimerDef = 8.0f;

	private bool atouch;
	private bool touched;
	private bool started;
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

		transform.Find("ship").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("GUIdisppaear1").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("GUIdisppaear2").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("newscore").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("touchtorespawn").GetComponent<Animation>().Play("GUIrespawnInvisible");
		GameObject.Find("GUIscore").GetComponent<Animation>().Play("SCOREoutscreen");

		transform.Find("ship").Find("smoke").GetComponent<ParticleSystem>().Stop();
		transform.Find("ship").Find("Splosion").GetComponent<ParticleSystem>().Stop();
		transform.Find("ship").Find("Splosion2").GetComponent<ParticleSystem>().Stop();
		GameObject.Find("ScoreSplos").GetComponent<ParticleSystem>().Stop();

		transform.Find("ship").GetComponent<Animation>().Play("ShipIdle");
	}

	void Start() {
		spawn();
	}

	void Update () {
		//Update position all frames
		transform.position = new Vector3 (posX, posY, 0f);
		transform.Find("ship").rotation = new Quaternion(0f, 0f, -(forceX*.1f), 1f);
		if (started && !dead) posX += forceX * Time.deltaTime;

		//Update Score
		GameObject.Find("GUITextScore1").GetComponent<TextMeshPro>().text = score.ToString();
		GameObject.Find("GUITextBest1").GetComponent<TextMeshPro>().text = bestscore.ToString();
		// GameObject.Find ("GUITextScore2").guiText.text = score.ToString();
		// GameObject.Find ("GUITextBest2").guiText.text = bestscore.ToString();

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
		else if (posX > borderX)
			dead = true;

		//Lets go mayte
		if (!started && touched) {
			started = true;
			respawnStuff = false;
			transform.Find("ship").GetComponent<Animation>().Play("ShipAscension");
			transform.Find("ship").Find("smoke").GetComponent<ParticleSystem>().Play();
			GameObject.Find("GUIdisppaear1").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("GUIdisppaear2").GetComponent<SpriteRenderer>().enabled = false;
		}
		//MAIN GAME
		else if (started && !dead) {
			if (posY < -1.35f)
				posY += 8f * Time.deltaTime;
			altitude += 1f * Time.deltaTime;

			if (timerStart > 0.0f) timerStart -= 1f * Time.deltaTime;
			else {
				//Increase Decrease forceX
				if (!goRight)
					forceX -= forceXDef * Time.deltaTime;
				else if (goRight)
					forceX += forceXDef * Time.deltaTime;
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
				scoreTimer -= 1f * Time.deltaTime;
			}
		}

		//Changing sky overtime
		if (altitude > 1f && spawnCloud) {
			spawnCloud = false;
			GameObject.Find("GUIscore").GetComponent<Animation>().Play("SCOREingame");
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

			transform.Find("ship").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("ship").Find("smoke").GetComponent<ParticleSystem>().Stop();
			transform.Find("ship").Find("Splosion").GetComponent<ParticleSystem>().Play();
			transform.Find("ship").Find("Splosion2").GetComponent<ParticleSystem>().Play();

			if (bestscore < score) {
				bestscore = score;
				PlayerPrefs.SetInt("TOTHEMOON_highscore", bestscore);
				highscore = true;
			}
		}
		if (deadOnce) {
			if (deadTimer <= 0) {
				if (!GUIrespawnOnce) {
					GameObject.Find("touchtorespawn").GetComponent<Animation>().Play("GUIrespawnVisible");

					//HIGHSCORE ANIMATION
					if (highscore) {
						GameObject.Find("GUInewscore").GetComponent<Animation>().Play("SCOREnewscore");
						GameObject.Find("ScoreSplos").GetComponent<ParticleSystem>().Play();
					}

					GUIrespawnOnce = true;
				}
			} else {
				deadTimer -= 1f * Time.deltaTime;
			}
			if (deadTimer2 <= 0) {
				if (!GUIrespawnOnce2) {
					GameObject.Find("GUIscore").GetComponent<Animation>().Play("SCOREendgame");
					GUIrespawnOnce2 = true;
				}
			} else {
				deadTimer2 -= 1f * Time.deltaTime;
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
		bool b = false;
		if (Input.GetMouseButton(0))
			b = true;
		else if (Input.touchCount > 0)
			b = true;
		return b;
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
			starsTimer -= 1f * Time.deltaTime;
	}

	private void createMeteors() {
		if (!dead && meteorsTimer <= 0f) {
			Instantiate(EntityMeteor, new Vector3(Random.Range(-3.5f, 3.5f), 6.0f, 0.0f), Quaternion.identity);
			meteorsTimer = meteorsTimerDef;
		}
		else
			meteorsTimer -= 1f * Time.deltaTime;
	}

	private void addPoint(int p) {
		GameObject.Find("GUIscore").GetComponent<Animation>().Play("SCOREaddpoint");
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
