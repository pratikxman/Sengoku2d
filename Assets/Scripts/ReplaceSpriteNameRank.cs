﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReplaceSpriteNameRank : MonoBehaviour {

	void Start () {
		BusyoInfoGet busyoInfoScript = new BusyoInfoGet ();
		string busyoName = busyoInfoScript.getName (int.Parse(name));
		string busyoRank = busyoInfoScript.getRank (int.Parse(name));

		transform.FindChild ("Text").GetComponent<Text> ().text = busyoName;
		transform.FindChild ("Rank").GetComponent<Text> ().text = busyoRank;

        if (Application.loadedLevelName != "preKaisen") {
                string imagePath = "Prefabs/Player/Sprite/unit" + name;
		    GetComponent<Image> ().sprite = 
			    Resources.Load (imagePath, typeof(Sprite)) as Sprite;
        }else {

            transform.FindChild("Text").GetComponent<Text>().fontSize = 70;
            Color white = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //white
            transform.FindChild("Text").GetComponent<Text>().color = white;
        }
		
	}
}
