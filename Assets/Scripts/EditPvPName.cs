﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditPvPName : MonoBehaviour {

    //public GameObject lightning;
    //public GameObject vs;

    void Start() {
        //lightning = GameObject.Find("lightning").gameObject;
        //vs = GameObject.Find("Vs").gameObject;
    }



    public void OnClick() {

        //lightning.SetActive(false);
        //vs.SetActive(false);
        GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "UI";

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        GameObject panel = GameObject.Find("Panel").gameObject;
        string pathOfBack = "Prefabs/Common/TouchBackForOne";
        GameObject back = Instantiate(Resources.Load(pathOfBack)) as GameObject;
        back.transform.SetParent(panel.transform);
        back.transform.localScale = new Vector2(1, 1);
        back.transform.localPosition = new Vector2(0, 0);
        string pathOfBoard = "Prefabs/PvP/FirstPvPBoard";
        GameObject board = Instantiate(Resources.Load(pathOfBoard)) as GameObject;
        board.transform.SetParent(panel.transform);
        board.transform.localScale = new Vector2(1, 0.9f);
        board.transform.localPosition = new Vector2(0, 0);

        back.GetComponent<CloseOneBoard>().deleteObj = board;
        board.transform.FindChild("NoButton").GetComponent<AddHyourou>().touchBackObj = back;

        //Adjust for 2nd Time
        board.transform.FindChild("YesButton").GetComponent<StartPvP>().secondTimeFlg = true;
        board.transform.FindChild("YesButton").GetComponent<StartPvP>().touchBackObj = back;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            board.transform.FindChild("YesButton").transform.FindChild("Text").GetComponent<Text>().text = "Edit";
        }else {
            board.transform.FindChild("YesButton").transform.FindChild("Text").GetComponent<Text>().text = "変更";
        }



    }
}
