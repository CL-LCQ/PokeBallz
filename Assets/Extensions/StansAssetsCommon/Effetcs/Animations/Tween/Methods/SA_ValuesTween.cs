using UnityEngine;
using System;
using System.Collections;
using StansAssets.Animation;

public class SA_ValuesTween : MonoBehaviour {
	

	public event Action OnComplete = delegate {};
	public event Action<float> OnValueChanged = delegate {};
	public event Action<Vector3> OnVectorValueChanged = delegate {};


	public bool DestoryGameObjectOnComplete = true;

	private float FinalFloatValue;
	private Vector3 FinalVectorValue;
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public static SA_ValuesTween Create() {
		return new GameObject("SA_ValuesTween").AddComponent<SA_ValuesTween>();
	}
	
	
	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------
	
	void Update() {
		OnValueChanged(transform.position.x);
		OnVectorValueChanged(transform.position);
	}
	

	
	public void ValueTo(float from, float to, float time, SA_EaseType easeType = SA_EaseType.linear) {
		Vector3 pos = transform.position;
		pos.x = from;
		transform.position = pos;
		FinalFloatValue = to;
		
		SA_iTween.MoveTo(gameObject, SA_iTween.Hash("x", to,  "time", time, "easeType", easeType.ToString(), "oncomplete", "onTweenComplete", "oncompletetarget", gameObject));
	}
	

	public void VectorTo(Vector3 from, Vector3 to, float time,  SA_EaseType easeType = SA_EaseType.linear) {
		transform.position = from;
		FinalVectorValue = to;

		SA_iTween.MoveTo(gameObject, SA_iTween.Hash("position", to,  "time", time, "easeType", easeType.ToString(), "oncomplete", "onTweenComplete", "oncompletetarget", gameObject));

	}

	public void ScaleTo(Vector3 from, Vector3 to, float time,  SA_EaseType easeType = SA_EaseType.linear) {

		transform.localScale = from;
		FinalVectorValue = to;

		SA_iTween.ScaleTo(gameObject, SA_iTween.Hash("scale", to,  "time", time, "easeType", easeType.ToString(), "oncomplete", "onTweenComplete", "oncompletetarget", gameObject));

	}
	

	public void VectorToS(Vector3 from, Vector3 to, float speed, SA_EaseType easeType = SA_EaseType.linear) {
		transform.position = from;
		FinalVectorValue = to;
		SA_iTween.MoveTo(gameObject, SA_iTween.Hash("position", to,  "speed", speed, "easeType", easeType.ToString(), "oncomplete", "onTweenComplete", "oncompletetarget", gameObject));
	}

	public void Stop() {
		SA_iTween.Stop(gameObject);
		Destroy(gameObject);
	}

	
		
	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------
	
	private void onTweenComplete() {

		OnValueChanged(FinalFloatValue);
		OnVectorValueChanged(FinalVectorValue);

		OnComplete();

		if(DestoryGameObjectOnComplete) {
			Destroy(gameObject);
		} else {
			Destroy(this);
		}

	}
	
	
	
	
}

