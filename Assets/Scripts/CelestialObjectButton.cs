using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialObjectButton : MonoBehaviour {

	// Hold a ref to the CelestialObject this button is assosiated with
	public GameObject celestialObject;

	// Draw line from this button to CelestialObject
	private void Update() {
		
		if (celestialObject == null) {
			return;
		}

		LineRenderer lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.positionCount = 2;
		var points = new Vector3[2];

		//Vector3 direction = uiWorldPos - celestialObject.transform.position;
		//direction.Normalize();
		Vector3 objectWorldPos = celestialObject.transform.position;
		objectWorldPos.y = 1;
		points[0] = objectWorldPos;
		
		// Get button position in world units
		Vector3 uiWorldPos = Camera.main.ScreenToWorldPoint(transform.position);
		uiWorldPos.y = 1;
		points[1] = uiWorldPos;
		
		lineRenderer.SetPositions(points);
	
	}
}
