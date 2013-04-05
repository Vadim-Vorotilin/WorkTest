using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

	public float destroyAltitude;
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y <= destroyAltitude)
			Destroy(gameObject);
	}
}
