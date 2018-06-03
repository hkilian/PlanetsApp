using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiHandler : MonoBehaviour {

	public Canvas canvas;
	
	public SolarSystem solarSystem;

	public GameObject buttonContainer;
	
	public GameObject modeToggle;

	public GameObject infoPanel;
	
	// Length of 
	public float lengthMultiplier = 0.01f;

	// How much to offset buttons from the center
	public float buttonCenterOffset = 0.2f;

	// Prefab for CelestialObject buttons
	public GameObject celestialObjectButtonPrefab;
	
	// Hold refs to CelestialObject buttons
	private List<GameObject> _celestialObjectButtons;

	void Start() {
		
		// Dont show info panel from start
		infoPanel.SetActive(false);
	}
	
	// Set ranges for info panel temperature and gravity scales by getting highest and lowest from all CelestialObjects
	public void SetScaleRanges() {

		float highestTemp = -99999;
		float lowestTemp = 999999;
		
		float highestGravity = -99999;
		float lowestGravity = 999999;
		
		for (var i = 0; i < solarSystem.CelestialObjects.Count; i++) {

			// Temperature
			float temp = solarSystem.CelestialObjects[i].GetComponent<CelestialObject>().Data.temperature_high;
			if (temp > highestTemp) { highestTemp = temp; }
			if (temp < lowestTemp) { lowestTemp = temp; }
			
			// Gravity
			float gravity = solarSystem.CelestialObjects[i].GetComponent<CelestialObject>().Data.gravity;
			if (gravity > highestGravity) { highestGravity = gravity; }
			if (gravity < lowestGravity) { lowestGravity = gravity; }
		
		}
		
		// Set range in scale
		infoPanel.transform.Find("TemperatureScale").GetComponent<Scale>().SetRange(lowestTemp, highestTemp);
		infoPanel.transform.Find("GravityScale").GetComponent<Scale>().SetRange(lowestGravity, highestGravity);
		
	}

	// Use this for initialization
	public void CreateButtons() {
				
		_celestialObjectButtons = new List<GameObject>();
		
		for (var i = 0; i < solarSystem.CelestialObjects.Count; i++) {
						
			GameObject button = (GameObject)Instantiate(celestialObjectButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
			button.transform.SetParent(buttonContainer.transform);
			
			String celestialObjectName = solarSystem.CelestialObjects[i].GetComponent<CelestialObject>().Data.name;
			button.name = celestialObjectName + "Button";
			button.GetComponentInChildren<Text>().text = celestialObjectName;
			
			// Set reference to CelestialObject
			button.GetComponent<CelestialObjectButton>().celestialObject = solarSystem.CelestialObjects[i].gameObject;
			
			// Set event listner for button
			button.GetComponent<Button>().onClick.AddListener(CelestialObjectPressed);
				
			// Add to list for accessing later
			_celestialObjectButtons.Add(button);
			
		}
		
	}
	
	// Update is called once per frame
	public void UpdateButtonPositions(float t) {

		for (var i = 0; i < solarSystem.CelestialObjects.Count; i++) {

			// Move button to the correct location
			GameObject celestialObject = solarSystem.CelestialObjects[i];
			GameObject celestialObjectButton = _celestialObjectButtons[i];

			Vector3 screenPosition = Camera.main.WorldToScreenPoint(celestialObject.transform.position);
			screenPosition.z = 0;
			
			// Use either width of height to determine button offset
			float offsetAmount = Screen.height * buttonCenterOffset;
			if (Screen.height > Screen.width) {
				offsetAmount = Screen.width * buttonCenterOffset;
			}

			// Offset position so every odd button is put on the other side
			float offset = -offsetAmount + ((offsetAmount*2) * (i % 2));
			Vector3 offsetScreenPosition = screenPosition;

			if (Screen.height > Screen.width) {
				
				offsetScreenPosition.x += offset;

				int total = solarSystem.CelestialObjects.Count;
				offsetScreenPosition.y += Mathf.Lerp(20, 40 - ((total - i) * 18), t);
				
			} else {
				
				// Move buttons so they dont overlap in horizontal screen mode
				float collisionOffset = -offsetAmount*0.1f;
				if (i % 4 == 0 || i % 4 == 1) {
					collisionOffset = offsetAmount * 0.1f * 2;
				}
				
				offsetScreenPosition.y += offset + collisionOffset;
				
			}
			
			offsetScreenPosition.z = 0;
			celestialObjectButton.transform.position = offsetScreenPosition;

		}
		
	}
	
	// Show the info panel (Name, temp, gravity etc..)
	public void CelestialObjectPressed() {
		
		// Get the game object assosiated with this button
		GameObject gameObject = EventSystem.current.currentSelectedGameObject.GetComponent<CelestialObjectButton>().celestialObject;
		CelestialObject celestialObject = gameObject.GetComponent<CelestialObject>();
				
		// Set panel active and set text
		infoPanel.SetActive(true);
		infoPanel.transform.Find("Name").GetComponent<Text>().text = celestialObject.Data.name;

		// Moons
		String moonsText = "Moons: " + celestialObject.Data.moons;
		infoPanel.transform.Find("Moons").GetComponent<Text>().text = moonsText;
		
		// Temp scale
		float tempHigh = celestialObject.Data.temperature_high;
		infoPanel.transform.Find("TemperatureScale").GetComponent<Scale>().SetValue(tempHigh, "C");
		
		// Gravity scale
		float gravity = celestialObject.Data.gravity;
		infoPanel.transform.Find("GravityScale").GetComponent<Scale>().SetValue(gravity, "m/s²");
		
		// Hide other ui
		buttonContainer.SetActive(false);
		modeToggle.SetActive(false);
		
	}

	public void HideInfoView() {
		
		infoPanel.SetActive(false);
		buttonContainer.SetActive(true);
		modeToggle.SetActive(true);
	}

	public void SetToggleText(string text) {
		modeToggle.transform.Find("Text").GetComponent<Text>().text = text;
	}
	
}
