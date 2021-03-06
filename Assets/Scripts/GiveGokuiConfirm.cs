﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GiveGokuiConfirm : MonoBehaviour {

	public int sakuId = 0;
	public string sakuName = "";
	public int gokuiQty = 0;
	public GameObject boardObj;
	public GameObject boardBackObj;

	public void OnClick(){

		//Show Confirm Screen
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		string busyoId = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyo;

		audioSources [0].Play ();

		//Common Process
		//Back Cover
		string backPath = "Prefabs/Busyo/back";
		GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
		back.transform.SetParent(GameObject.Find ("Panel").transform);
		back.transform.localScale = new Vector2 (1, 1);
		RectTransform backTransform = back.GetComponent<RectTransform> ();
		backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

		//Message Box
		string msgPath = "Prefabs/Busyo/GokuiConfirm";
		GameObject msg = Instantiate (Resources.Load (msgPath)) as GameObject;
		msg.transform.SetParent(GameObject.Find ("Panel").transform);
		msg.transform.localScale = new Vector2 (1, 1);
		RectTransform msgTransform = msg.GetComponent<RectTransform> ();
		msgTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
		msgTransform.name = "GokuiConfirm";

        //Message Text Mod
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            msg.transform.FindChild("Text").GetComponent<Text>().text= "My lord, Do you want to transmit " + sakuName + "?\n He won't be able to use current active skill anymore.";
        }else {
            msg.transform.FindChild("Text").GetComponent<Text>().text = "御屋形様、" + sakuName + "を伝授なさいますか？\n現在の策は使用出来なくなりますぞ。";
        }
		//Add busyoId
		msg.transform.FindChild ("YesButton").GetComponent<GiveGokui> ().busyoId = int.Parse (busyoId);
		msg.transform.FindChild ("YesButton").GetComponent<GiveGokui> ().sakuId = sakuId;
		msg.transform.FindChild ("YesButton").GetComponent<GiveGokui> ().sakuName = sakuName;
		msg.transform.FindChild ("YesButton").GetComponent<GiveGokui> ().confirmObj = msg;
		msg.transform.FindChild ("NoButton").GetComponent<GiveGokui> ().confirmObj = msg;
		msg.transform.FindChild ("YesButton").GetComponent<GiveGokui> ().backObj = back;
		msg.transform.FindChild ("NoButton").GetComponent<GiveGokui> ().backObj = back;
		msg.transform.FindChild ("YesButton").GetComponent<GiveGokui> ().boardObj = boardObj;
		msg.transform.FindChild ("YesButton").GetComponent<GiveGokui> ().boardBackObj = boardBackObj;
	}
}
