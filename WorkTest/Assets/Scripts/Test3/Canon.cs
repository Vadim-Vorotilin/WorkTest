using System;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour {

	private const float DerivDelta = 0.0001f;
	private const float NewtonError = 0.0001f;

	public float ammoSpeed;
	public Rigidbody ammo;

	public Transform launchPos;
	public Target target;

	private Vector2 canonStPos;
	private Vector2 targetStPos;
	private float gravity;

	private float AmmoX (float k, float t) {
		return canonStPos.x + (1f / Mathf.Sqrt(1 + k * k)) * ammoSpeed * t;
	}

	private float TargetX (float k, float t) {
		return targetStPos.x + target.xSpeed;
	}

	private float FuncX (float k, float t) {
		return AmmoX(k, t) - TargetX(k, t);
	}

	private float AmmoY (float k, float t) {
		return canonStPos.y + (k / Mathf.Sqrt(1 + k * k)) * ammoSpeed * t - (gravity * t * t) / 2f;
	}

	private float TargetY (float k, float t) {
		return targetStPos.y;
	}

	private float FuncY (float k, float t) {
		return AmmoY(k, t) - TargetY(k, t);
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

		Debug.Log(string.Format("Canon: {0}, Target: {1}, Gravity: {2}", canonStPos, targetStPos, gravity));

		Vector2 initApprox = new Vector2(0.5f, Vector2.Distance(canonStPos, targetStPos) / (ammoSpeed - target.xSpeed));

		Vector2 newton = Newton(FunkXVec, FunkYVec, initApprox, NewtonError);

		Debug.Log(string.Format("X: {0}, Y: {1}", FunkXVec(newton), FunkYVec(newton)));

		if (float.IsInfinity(newton.x) || float.IsNaN(newton.x) || newton.y < 0) {
			Debug.LogWarning("Target can't be reached!");
			return;
		}

		float angle = Mathf.Atan(newton.x) * Mathf.Rad2Deg;

		Debug.Log(newton + " " + angle);

		Rigidbody ammoInst = Instantiate(ammo, launchPos.position, Quaternion.identity) as Rigidbody;

		if (ammoInst == null) {
			Debug.LogError("Ammo instance = null!");
			return;
		}

		points1.Add(new Vector2(TargetX(newton.x, newton.y), TargetY(newton.x, newton.y)));
		points2.Add(new Vector2(AmmoX(newton.x, newton.y), AmmoY(newton.x, newton.y)));

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

		} while (Mathf.Abs(last.x - current.x) > error || Mathf.Abs(last.y - current.y) > error);

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

	void Start () {
		points1 = new List<Vector2>();
		points2 = new List<Vector2>();
	}

	private List<Vector2> points1;
	private List<Vector2> points2;

	void OnDrawGizmos () {
		if (points1 == null)
			points1 = new List<Vector2>();

		if (points2 == null)
			points2 = new List<Vector2>();

		Gizmos.color = Color.red;

		for (int i = 0; i != points1.Count; i++) {
			Gizmos.DrawWireSphere(points1[i], 0.1f);
		}

		Gizmos.color = Color.green;

		for (int i = 0; i != points2.Count; i++) {
			Gizmos.DrawWireSphere(points2[i], 0.1f);
		}
	}
}
