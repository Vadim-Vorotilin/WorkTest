using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	public float xSpeed;

	void FixedUpdate () {
		transform.position += Vector3.right * xSpeed * Time.fixedDeltaTime;
	}
}
