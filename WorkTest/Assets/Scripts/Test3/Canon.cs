using System;
using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour {

	private const float DerivDelta = 0.0001f;
	private const float NewtonError = 0.00001f;

	public float ammoSpeed;
	public Rigidbody ammo;

	public Transform launchPos;
	public Target target;

	private Vector2 canonStPos;
	private Vector2 targetStPos;
	private float gravity;

	private float FuncX (float k, float t) {
		return canonStPos.x - targetStPos.x - target.xSpeed + (1f / Mathf.Sqrt(1 + k * k)) * ammoSpeed * t;
	}

	private float FuncY (float k, float t) {
		return canonStPos.y - targetStPos.y + (k / Mathf.Sqrt(1 + k * k)) * ammoSpeed * t - (gravity * t * t) / 2f;
	}

	private float FunkXVec (Vector2 vec) {
		return FuncX(vec.x, vec.y);
	}

	private float FunkYVec (Vector2 vec) {
		return FuncY(vec.x, vec.y);
	}

	private void Launch () {
		canonStPos = launchPos.position;
		targetStPos = target.transform.position;
		gravity = -Physics.gravity.y;

		Debug.Log(string.Format("Canon: {0}, Target: {1}, Gravity: {2}", canonStPos, targetStPos.x, gravity));

		Vector2 initApprox = new Vector2(0.5f, Vector2.Distance(canonStPos, targetStPos) / (ammoSpeed - target.xSpeed));

		Vector2 newton = Newton(FunkXVec, FunkYVec, initApprox, NewtonError);

		float angle = Mathf.Atan(newton.x) * Mathf.Rad2Deg;

		Debug.Log(newton + " " + angle);

		Rigidbody ammoInst = Instantiate(ammo, launchPos.position, Quaternion.identity) as Rigidbody;

		if (ammoInst == null) {
			Debug.LogError("Ammo instance = null!");
			return;
		}

		ammoInst.AddForce(Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right * ammoSpeed, ForceMode.VelocityChange);
	}

	private static Vector2 Newton (Func<Vector2, float> func1, Func<Vector2, float> func2, Vector2 initialApproximation, float error) {
		Vector2 current = initialApproximation;
		Vector2 last;

		do {
			last = current;

			float deriv11 = PartDeriv(func1, last, 0);
			float deriv12 = PartDeriv(func1, last, 1);
			float deriv21 = PartDeriv(func2, last, 0);
			float deriv22 = PartDeriv(func2, last, 1);

			current = CramerRule(new Vector2(deriv11, deriv21),
			                     new Vector2(deriv12, deriv22),
			                     new Vector2(-func1(last) + deriv11 * last.x + deriv12 * last.y,
			                                 -func2(last) + deriv21 * last.x + deriv22 * last.y));

		} while (Vector2.Distance(last, current) > error);

		return current;
	}

	private static float PartDeriv (Func<Vector2, float> func, Vector2 point, int var) {
		return (func(new Vector2(point.x + (var == 0 ? DerivDelta : 0), point.y + (var == 1 ? DerivDelta : 0))) - func(point)) /
		       DerivDelta;
	}

	private static Vector2 CramerRule (Vector2 vec1, Vector2 vec2, Vector2 vecb) {
		float det = Det(vec1, vec2);

		return new Vector2(Det(vecb, vec2) / det, Det(vec1, vecb) / det);
	}

	private static float Det (Vector2 vec1, Vector2 vec2) {
		return vec1.x * vec2.y - vec1.y * vec2.x;
	}

	void OnGUI () {
		if (GUI.Button(new Rect(10, 10, 100, 50), "Launch")) {
			Launch();
		}
	}
}
