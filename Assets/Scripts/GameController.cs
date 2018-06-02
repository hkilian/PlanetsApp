using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour {
	
	// 2 Types of viewing modes
	public enum ViewMode {ScaleCompare, DistanceCompare};
	
	// Hold ref to solar system
	public SolarSystem solarSystem;

	// Are we in scale mode or distance mode
	private static ViewMode _currentViewMode;
	public static ViewMode CurrentViewMode { get { return _currentViewMode; } }
	
	// Instance for singleton patturn
	private static GameController _instance;
	public static GameController Instance { get { return _instance; } }

	// Multiply distances to keep them within within draw distance
	private static float _distanceMultiplier = 1.0f;
	public static float DistanceMultiplier { get { return _distanceMultiplier; } }
	
	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

	private void Start() {
		
		// Set the view mode to scale compare at start
		SetViewMode(ViewMode.ScaleCompare);
			
	}

	private void SetViewMode(ViewMode mode) {

		_currentViewMode = mode;
		solarSystem.SetViewMode(mode);

	}
	
}
