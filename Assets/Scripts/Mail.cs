﻿using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class Mail : MonoBehaviour {

    private const string NEW_LINE_STRING = "\n";
    private const string CAUTION_STATEMENT = "---------Keep below info.---------" + NEW_LINE_STRING;
    private const string CAUTION_STATEMENT2 = "---------Keep above info.---------" + NEW_LINE_STRING;

    public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [5].Play ();

		string email    =   "zeimoter@gmail.com";
        string subject = "";
        string body = "";

        if (Application.systemLanguage != SystemLanguage.Japanese) {
            subject = "【The Samurai Wars】";
		     body = "Body of the letter";
        }else {
            subject = "【合戦-戦国絵巻-】お問い合わせ";
            body = "お問い合わせ内容をご記載下さい。";
        }

        //App version
        string versionNo = Application.version;
        body += NEW_LINE_STRING + CAUTION_STATEMENT + NEW_LINE_STRING;
        body += "Version  : " + versionNo + NEW_LINE_STRING;
        body += "OS       : " + SystemInfo.operatingSystem + NEW_LINE_STRING;
        body += "User Id  : " + PlayerPrefs.GetString("userId") + NEW_LINE_STRING;

        //Data
        body += CAUTION_STATEMENT2;

        //エスケープ処理
        body = System.Uri.EscapeDataString(body);
		subject =System.Uri.EscapeDataString(subject);
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}
}
