using UnityEngine;
using System.Collections;

public class Destructor : MonoBehaviour {
	
	public float timer;
	
	void Update () {
		if (timer > 0.0f) timer -= 1f * Time.deltaTime;
		else Destroy (this.gameObject);
	}
}
