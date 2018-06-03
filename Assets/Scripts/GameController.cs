using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	
	// 2 Types of viewing modes
	public enum ViewMode {ScaleCompare, DistanceCompare};
	
	// Hold ref to solar system
	public SolarSystem solarSystem;

	// Ref to camera controller
	public CameraController cameraController;
	
	// Ref to uiHandler
	public UiHandler uiHandler;
	
	// Animation time used animate the transition between scale mode and distance mode
	private float animationT = 0.0f;
	
	// Are we in scale mode or distance mode
	private static ViewMode _currentViewMode;
	public static ViewMode CurrentViewMode { get { return _currentViewMode; } }
	
	// Instance for singleton patturn
	private static GameController _instance;
	public static GameController Instance { get { return _instance; } }
	
	// Multiply distances to keep them within within draw distance
	private static float _distanceMultiplier = 0.1f;
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
				
		// Crete UI from solarsystem and set ranges for info scales(temp,gravity)
		uiHandler.CreateButtons();
		uiHandler.SetScaleRanges();
		
		// Set the view mode to scale compare at start
		_currentViewMode = ViewMode.ScaleCompare;
		cameraController.LerpBetweenViewModes(0);
		solarSystem.LerpBetweenViewModes(0);

	}

	private void Update() {
		
		// Exit game
		if (Input.GetKey("escape"))
			Application.Quit();

		// Increase or decrease the animationT depending on ViewMode
		bool animating = false;
		if (_currentViewMode == ViewMode.ScaleCompare) {

			if (animationT > 0) {
				animating = true;
				animationT -= Time.deltaTime * 0.5f;
			}
			
		} else {
			
			if (animationT < 1) {
				animating = true;
				animationT += Time.deltaTime * 0.5f;
			}
		
		}

		// Add an easing to the transition
		float t = EasingFunction.EaseInOutCubic(0, 1, animationT);
		
		// Only run if animating or if we are in editor so we can see changes live
		if (animating || Application.isEditor) {
			
			cameraController.LerpBetweenViewModes(t);
			solarSystem.LerpBetweenViewModes(t);
			
		}
		
		uiHandler.UpdateButtonPositions(t);

	}

	// Switch between scale and distance modes if not in inspect mode
	public void ToggleViewMode() {
		
		if (_currentViewMode == ViewMode.ScaleCompare) {
			_currentViewMode = ViewMode.DistanceCompare;
			uiHandler.SetToggleText("SHOW SCALE");
		} else {
			_currentViewMode = ViewMode.ScaleCompare;
			uiHandler.SetToggleText("SHOW DISTANCE");
		}
		
	}
	
}
