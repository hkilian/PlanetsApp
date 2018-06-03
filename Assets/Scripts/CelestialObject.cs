using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialObject : MonoBehaviour {
	
	// _data holds information like name, distance, temp etc...
	private CelestialObjectData _data;
	public CelestialObjectData Data {
		get { return _data; }
		set {
			_data = value;
			name = _data.name;
		}
	}
	
	// Our order in the solar system
	public int Order {get;set;}
	
	// Distance between Celestial objects when in scale compare mode
	private float _scaleModeSpacer = 30;
	public float ScaleModeSpacer {
		get { return _scaleModeSpacer; }
		set { 
			_scaleModeSpacer = value;
		}
	}
	
	// This objects 3d model
	private GameObject _model;

	// Runs before anything else
	void Awake () {
				
		_model = transform.Find("Model").gameObject;
		if (_model == null) {
			Debug.LogError("Could not find model for " + _data.name);
		}
		
	}

	// 0 being Scale mode and 1 being distance mode
	public void LerpViewMode(float t) {
		
		Vector3 position = transform.position;
		
		// Scale mode values
		float scalePositionX = (130) + ((Order + 1) * _scaleModeSpacer);
		float scaleLineWidth = 0.4f;
		float scaleModeScale = _data.size / 5000.0f;
		
		// Distance mode values
		float distancePositionX = _data.distance * GameController.DistanceMultiplier;
		float distanceLineWidth = 10.0f;
		float distanceModeScale = _data.size / 5000.0f * GameController.DistanceMultiplier;
		
		// Interpolate between
		transform.position = new Vector3(Mathf.Lerp(scalePositionX, distancePositionX, t), 0, 0);

		float scale = Mathf.Lerp(scaleModeScale, distanceModeScale, t);
		_model.transform.localScale = new Vector3(scale, scale, scale);
		
		// Set orbit fade t globally
		Shader.SetGlobalFloat("_FadeT", t);	
		
	}
	
	
}
