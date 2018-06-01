using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[ExecuteInEditMode]
public class Ring : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		SetPoints();
	}
	
	// Set the points in the line renderer
	void SetPoints() {

		LineRenderer lineRenderer = GetComponent<LineRenderer>();

		float radius = Vector3.Distance(new Vector3(0, 0, 0), transform.position);
		int totalPoints = 150;

		lineRenderer.positionCount = totalPoints;
		
		var points = new Vector3[totalPoints];
		for (int i = 0; i < totalPoints; i++) {

			float t =  2 * Mathf.PI / totalPoints;
			float x = (Mathf.Sin(i * t) * radius) - transform.position.x;
			float z = (Mathf.Cos(i * t) * radius) - transform.position.z;
			points[i] = new Vector3(x, 0.0f, z);
			
		}
		
		lineRenderer.SetPositions(points);

	}
	
}
