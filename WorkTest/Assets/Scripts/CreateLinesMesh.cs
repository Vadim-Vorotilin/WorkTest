using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CreateLinesMesh : MonoBehaviour {

	public List<Vector2> line1;
	public float radius;

	public Material material;

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<MeshFilter>().mesh = CreateLineMesh(line1, radius);
		gameObject.AddComponent<MeshRenderer>();

		renderer.material = material;
	}

	private static Mesh CreateLineMesh (List<Vector2> line, float radius) {
		int centalVertsCount = line.Count + 2;
		int vertsCount = (centalVertsCount) * 3;

		Vector3[] verts = new Vector3[vertsCount];
		Vector3[] norms = new Vector3[vertsCount];
		Vector2[] uv = new Vector2[vertsCount];
		int[] tris = new int[(centalVertsCount) * 4 * 3];

		Mesh mesh = new Mesh();

		Vector2 lastNorm = Vector2.zero;

		for (int i = 0; i != centalVertsCount; i++) {
			Vector2 dir1;
			Vector2 dir2;

			if (i == 0 || i == 1) {
				dir1 = line[0] - line[1];
				dir2 = -dir1;

				lastNorm = (new Vector2(dir1.y, -dir1.x)).normalized;
			}
			else if (i == centalVertsCount - 2 || i == centalVertsCount - 1) {
				dir1 = line[line.Count - 2] - line[line.Count - 1];
				dir2 = -dir1;
			}
			else {
				dir1 = line[i - 2] - line[i - 1];
				dir2 = line[i] - line[i - 1];
			}

			Vector2 central = (i != 0 && i != centalVertsCount - 1) ? line[i - 1] : (i == 0 ? line[0] + dir1.normalized * radius : line[line.Count - 1] + dir2.normalized * radius);

			Vector2 norm = dir1.normalized + dir2.normalized;

			if (norm.sqrMagnitude < 0.001f)
				norm = new Vector2(dir1.y, -dir1.x);

			norm.Normalize();

			if (IsCross(central, central + dir1, central + norm, central + dir1 + lastNorm))
				norm = -norm;

			lastNorm = norm;

			float angle = Vector2.Angle(dir1, dir2) / 2f;
			float currentRadius = radius / Mathf.Sin(angle * Mathf.Deg2Rad);

			verts[i * 3] = central;
			verts[i * 3 + 1] = central + norm * currentRadius;
			verts[i * 3 + 2] = central - norm * currentRadius;

			norms[i * 3] = norms[i * 3 + 1] = norms[i * 3 + 2] = Vector3.back;

			uv[i * 3] = (i != 0 && i != centalVertsCount - 1) ? new Vector2(0.5f, 0.5f) : (i == 0 ? new Vector2(0f, 0.5f) : new Vector2(1f, 0.5f));
			uv[i * 3 + 1] = (i != 0 && i != centalVertsCount - 1) ? new Vector2(0.5f, 1f) : (i == 0 ? new Vector2(0f, 1f) : new Vector2(1f, 1f));
			uv[i * 3 + 2] = (i != 0 && i != centalVertsCount - 1) ? new Vector2(0.5f, 0f) : (i == 0 ? new Vector2(0f, 0f) : new Vector2(1f, 0f));

			if (i == 0) 
				continue;

			tris[(i - 1) * 4 * 3] = i * 3;
			tris[(i - 1) * 4 * 3 + 1] = i * 3 + 2;
			tris[(i - 1) * 4 * 3 + 2] = (i - 1) * 3 + 2;

			tris[(i - 1) * 4 * 3 + 3] = i * 3;
			tris[(i - 1) * 4 * 3 + 4] = (i - 1) * 3 + 2;
			tris[(i - 1) * 4 * 3 + 5] = (i - 1) * 3;

			tris[(i - 1) * 4 * 3 + 6] = i * 3;
			tris[(i - 1) * 4 * 3 + 7] = (i - 1) * 3 + 1;
			tris[(i - 1) * 4 * 3 + 8] = i * 3 + 1;

			tris[(i - 1) * 4 * 3 + 9] = i * 3;
			tris[(i - 1) * 4 * 3 + 10] = (i - 1) * 3;
			tris[(i - 1) * 4 * 3 + 11] = (i - 1) * 3 + 1;
		}

		mesh.vertices = verts;
		mesh.normals = norms;
		mesh.uv = uv;
		mesh.triangles = tris;

		return mesh;
	}

	/// <summary>
	/// Check whether segments are crossing
	/// </summary>
	/// <param name="p1">Beginning of the 1st segment</param>
	/// <param name="p2">End of the 1st segment</param>
	/// <param name="p3">Beginning of the 2nd segment</param>
	/// <param name="p4">End of the 2nd segment</param>
	private static bool IsCross (Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
		float den = (p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y);

		if (Mathf.Abs(den) < 0.0001f)
			return false;

		float num1 = (p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x);
		float num2 = (p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x);

		float u1 = num1 / den;
		float u2 = num2 / den;

		return u1 >= 0 && u1 <= 1 && u2 >= 0 && u2 <= 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos () {
		int centalVertsCount = line1.Count + 2;
		
		Vector2 lastNorm = Vector2.zero;

		List<Vector2> line = new List<Vector2>();

		for (int i = 0; i != centalVertsCount; i++) {			
			Vector2 dir1;
			Vector2 dir2;

			if (i == 0 || i == 1) {
				dir1 = line1[0] - line1[1];
				dir2 = -dir1;

				lastNorm = (new Vector2(dir1.y, -dir1.x)).normalized;
			}
			else if (i == centalVertsCount - 2 || i == centalVertsCount - 1) {
				dir1 = line1[line1.Count - 2] - line1[line1.Count - 1];
				dir2 = -dir1;
			}
			else {
				dir1 = line1[i - 2] - line1[i - 1];
				dir2 = line1[i] - line1[i - 1];
			}

			Vector2 central = (i != 0 && i != centalVertsCount - 1) ? line1[i - 1] : (i == 0 ? line1[0] + dir1.normalized * radius : line1[line1.Count - 1] + dir2.normalized * radius);
			line.Add(central);
			//Vector2 dir1n = dir1.normalized;
			//Vector2 dir2n = dir2.normalized;

			Vector2 norm = dir1.normalized + dir2.normalized;

			if (norm.sqrMagnitude < 0.001f)
				norm = new Vector2(dir1.y, -dir1.x);

			norm.Normalize();

			//if (i == 3) {
			//	Gizmos.color = Color.gray;
			//	Gizmos.DrawLine(central, central + dir1);

			//	Gizmos.color = Color.black;
			//	Gizmos.DrawLine(central + norm * radius, central + dir1 + lastNorm * radius);
			//}

			if (IsCross(central, central + dir1, central + norm, central + dir1 + lastNorm)) {
				norm = -norm;
			}

			lastNorm = norm;

			float angle = Vector2.Angle(dir1, dir2) / 2f;
			float currentRadius = radius / Mathf.Sin(angle * Mathf.Deg2Rad);

			Gizmos.color = Color.blue;
			Gizmos.DrawLine(central, central + norm * currentRadius);

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(central, central - norm * currentRadius);

			if (i == 0)
				continue;

			Gizmos.color = Color.red;
			Gizmos.DrawLine(line[i], line[i - 1]);
		}
	}
}
