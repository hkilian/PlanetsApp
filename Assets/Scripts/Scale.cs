using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Scale : MonoBehaviour {

	private GameObject _slider;

	private float _rangeLow;
	private float _rangeHigh;

	private GameObject _sliderText;

	// Use this for initialization
	void Awake () {

		_slider = transform.Find("Slider").gameObject;
		_sliderText = _slider.transform.Find("Text").gameObject;
	}
	
	// Set range
	public void SetRange(float low, float high) {

		// Set ranges with some breathing room
		_rangeLow = low * 1.1f;
		_rangeHigh = high * 1.1f;

	}
	
	// Set value
	public void SetValue(float value, string suffex) {
		
		_sliderText.GetComponent<Text>().text = value.ToString() + " " + suffex;

		// Value as a percentage in range
		float percent = ((value - _rangeLow) * 100) / (_rangeHigh - _rangeLow);

		// Move slider to correct location
		float xPosition = GetComponent<RectTransform>().rect.width * (percent / 100.0f);
		_slider.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition, 0);

	}
	
}
