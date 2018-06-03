using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SolarSystem : MonoBehaviour {

	// Name of the file where our planets data is stored
	public String celestialObjectDataFilename = "CelestialObjectData.json";
	
	// Prefab to use as celestial object
	public GameObject celestialObjectPrefab;
	
	// Orbit shader for updating t when lerping
	public Shader orbitShader;
	
	// Distance between CelestialObjects when in scale mode
	public float scaleModeSpacer = 30;
	
	// Location of data folder
	private String _dataFolder = "Data/";
	
	// List of data loaded from json
	private List<CelestialObjectData> _loadedCelestialObjectData;
	
	// List of CelestialObject instances
	private List<GameObject> _celestialObjects;
	public List<GameObject> CelestialObjects {
		get { return _celestialObjects; }
	}
	
	// Refrence to the sun
	private GameObject _sun;

	// Initialization
	public void Init() {
		
		// Load in the data for celestial objects from json
		LoadSolarSystemData();
		
		// Create instances of CelestialObject from loaded data
		CreateCelestialObjects();

		// Grab the sun gameobject
		_sun = transform.Find("Sun").gameObject;

	}
	
	// Update is called once per frame
	void Update () {

		// Only update this every frame if we are in the unity editor
		if (Application.isEditor) {

			for (var i = 0; i < _celestialObjects.Count; i++) {
				_celestialObjects[i].GetComponent<CelestialObject>().ScaleModeSpacer = scaleModeSpacer;
				
			}
			
		}

	}
	
	// Lerp between scale and distance modes
	public void LerpBetweenViewModes(float t) {
		
		// Interpolate between modes
		LerpViewModeSun(t);
		
		for (var i = 0; i < _celestialObjects.Count; i++) {
			_celestialObjects[i].GetComponent<CelestialObject>().LerpViewMode(t);
		}
				
	}
	
	// Loads CelestialObjectData from json
	private void LoadSolarSystemData() {
		
		// Get the location of the so CelestialObjectData file
		string filePath = Path.Combine(Application.streamingAssetsPath, celestialObjectDataFilename);

		if(File.Exists(filePath)) {
			
			// Read the json from the file into a string
			string jsonString = File.ReadAllText(filePath);
			
			// Parse json and produce a list of AstronomicalObjectData instances
			CelestialObjectData[] dataUnsorted = JsonHelper.FromJson<CelestialObjectData>(jsonString);
			
			// Sort list by distance
			_loadedCelestialObjectData = dataUnsorted.ToList().OrderBy(o=>o.distance).ToList();
			
		} else {
			Debug.LogError("Cannot find celestial object data at path " + filePath);
		}
	}
	
	private void CreateCelestialObjects() {
		
		// Store our gameobjects for later
		_celestialObjects = new List<GameObject>();
		
		for (var i = 0; i < _loadedCelestialObjectData.Count; i++) {
			
			//print("Making " + _loadedCelestialObjectData[i].name);
			
			GameObject celestialObject = (GameObject)Instantiate(celestialObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
			celestialObject.transform.parent = transform;
			celestialObject.GetComponent<CelestialObject>().Data = _loadedCelestialObjectData[i];

			celestialObject.GetComponent<CelestialObject>().Order = i;
			
			// Store instance in list
			_celestialObjects.Add(celestialObject);
			
		}
		
	}
	
	// Lerp between view modes for sun
	private void LerpViewModeSun(float t) {
		
		// Set scale of sun for each view mode
		float scaleSunSize = 250;
		float distanceSunScale = scaleSunSize * GameController.DistanceMultiplier / 1000;

		// Lerp between
		float scale = Mathf.Lerp(scaleSunSize, distanceSunScale, t);
		_sun.transform.localScale = new Vector3(scale, scale, scale);
		
	}

	
}
