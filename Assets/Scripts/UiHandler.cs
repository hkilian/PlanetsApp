using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHandler : MonoBehaviour {

	public Canvas canvas;

	public GameObject celestialObjectButtonPrefab;

	public SolarSystem solarSystem;
	
	private List<GameObject> _celestialObjectButtons;

	// Use this for initialization
	public void CreateButtons() {
		
		print("making buttons amount = " + solarSystem.CelestialObjects.Count);
		
		for (var i = 0; i < solarSystem.CelestialObjects.Count; i++) {
			
			print("making button = " + i);
			
			GameObject celestialObject = (GameObject)Instantiate(celestialObjectButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
			celestialObject.transform.SetParent(canvas.gameObject.transform);
			
			//celestialObject.GetComponent<CelestialObject>().Data = _loadedCelestialObjectData[i];
				
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
