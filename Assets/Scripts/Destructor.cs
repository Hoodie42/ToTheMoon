using UnityEngine;
using System.Collections;

public class Destructor : MonoBehaviour {
	
	public float timer;
	
	void Update () {
		if (timer > 0.0f) timer -= 0.01f;
		else Destroy (this.gameObject);
	}
}
