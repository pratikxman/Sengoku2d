﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MaskButton : MonoBehaviour {

	public void OnClick(){
		//Get Naise
		GameObject naisei = GameObject.Find ("NaiseiView");
		naisei.transform.FindChild (name).GetComponent<AreaButton> ().OnClick();
	}
}