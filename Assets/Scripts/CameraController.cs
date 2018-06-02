using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour {

	public SolarSystem solarSystem;
	[Space(10)]
	
	// Space from the top of the screen to nearest CelestialObject in world units (Scale Mode and Distance Mode)
	public float ScaleFurthestObjectSpacer = 50.0f;
	
	// Space from the center of sun to the bottom of the screen in world units (Scale Mode and Distance Mode)
	public float ScaleSunSpacer = 50.0f;
	[Space(10)]
	
	// Same as above but for distance mode
	public float DistanceFurthestObjectSpacer = 500.0f;
	public float DistanceSunSpacer = 500.0f;

	[Space(10)]

	// Used for lerping between modes
	private float furthestObjectSpacer = 0;
	private float sunSpacer = 0;
	
	// Zoom level (Scale Mode)
	public float zoomLevel = 1.0f;
	public float minZoomLevel = 0.5f;

	// Use this for initialization
	void Start () {
		
		

	}
	
	// Lerp between scale and distance modes
	public void LerpBetweenViewModes(float t) {

		furthestObjectSpacer = Mathf.Lerp(ScaleFurthestObjectSpacer, DistanceFurthestObjectSpacer, t);
		sunSpacer = Mathf.Lerp(ScaleSunSpacer, DistanceSunSpacer, t);

	}

	// Update is called once per frame
	void Update () {
			
		DrawDebugLines();
				
		Vector3 position = transform.position;
		float orthoHeight = 270;
		
		if (solarSystem.CelestialObjects != null && solarSystem.CelestialObjects.Count > 1) {
			
			// Get the CelestialObject which is furthest from the sun
			float furthestObject = solarSystem.CelestialObjects.Last().transform.position.x;
			
			// Draw a line through all planets
			Debug.DrawLine(new Vector3(0,0,0), new Vector3(furthestObject,0,0), Color.white);

			orthoHeight = furthestObject;
						
		}

		// If screen is wider than height then rotate cam by 90 and multiply orthographicSize by the screen ratio
		float ratio = 1;
		if (Screen.width >= Screen.height) {
			ratio = (float) Screen.height / (float) Screen.width;
			Quaternion rotatedBy90 = Quaternion.Euler(90,90,90);
			transform.rotation = rotatedBy90;
		} else {
			Quaternion rotation = Quaternion.Euler(90,90,0);
			transform.rotation = rotation;
		}

		Camera camera = GetComponent<Camera>();
		camera.orthographicSize = ((orthoHeight + furthestObjectSpacer - sunSpacer) / 2) * ratio;

		// Set position to center of planets
		position.x = (orthoHeight + sunSpacer)/ 2;
		transform.position = position;
		

	}
	
	// Find the top and bottom of the 

	// Draws lines in the scene view to show where the screen is
	private void DrawDebugLines() {
		
		Camera camera = GetComponent<Camera>();
		
		
		
	}
	
}
