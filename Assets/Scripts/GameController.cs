using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour {
	
	public float t = 0.0f;
	
	// 2 Types of viewing modes
	public enum ViewMode {ScaleCompare, DistanceCompare};
	
	// Hold ref to solar system
	public SolarSystem solarSystem;

	// Ref to camera controller
	public CameraController cameraController;
	
	// Ref to uiHandler
	public UiHandler uiHandler;
	
	// Used to animate the tranition between scale mode and distance mode
	private float animationT = 0.0f;
	
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

		// Load data and create objects
		solarSystem.Init();
				
		// Crete UI from solarsystem
		uiHandler.CreateButtons();
		
		// Set the view mode to scale compare at start
		SetViewMode(ViewMode.ScaleCompare);
			
	}

	private void Update() {

		if (_currentViewMode == ViewMode.ScaleCompare) {

			if (animationT > 0) {
				animationT -= 0.01f;
			}
			
		} else {
			
			if (animationT < 1) {
				animationT += 0.01f;
			}
		
		}

		float t = EasingFunction.EaseInOutCubic(0, 1, animationT);
		cameraController.LerpBetweenViewModes(t);
		solarSystem.LerpBetweenViewModes(t);

	}

	private void SetViewMode(ViewMode mode) {

		_currentViewMode = mode;
		solarSystem.SetViewMode(mode);

	}

	public void ToggleViewMode() {
		
		if (_currentViewMode == ViewMode.ScaleCompare) {
			SetViewMode(ViewMode.DistanceCompare);
		} else {
			SetViewMode(ViewMode.ScaleCompare);
		}
		
	}
	
}
