﻿using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class ZukanMenu : MonoBehaviour {

	public Color pushedTabColor = new Color (118f / 255f, 103f / 255f, 16f / 255f, 255f / 255f);
	public Color pushedTextColor = new Color (219f / 255f, 219f / 255f, 212f / 255f, 255f / 255f);
	public Color normalTabColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
	public Color normalTextColor = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
    public bool pushedFlg = false;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

        //Not Pushed Tab Color
        if (!pushedFlg) {
            Resources.UnloadUnusedAssets();

            GameObject Content = GameObject.Find ("Content");
		    deletePrevious (Content);

		    changeTabColor (name);

		    if (name == "Busyo") {
			    showBusyoZukan(Content);
		    } else if (name == "Kahou") {
			    showKahouZukan(Content);
		    } else if (name == "Tenkahubu") {
			    showTenkahubuZukan(Content);
		    }
        }
    }

	public void changeTabColor(string pushedTabName){
		//Change Button Color
		GameObject UpperView = GameObject.Find ("UpperView").gameObject;
		foreach ( Transform obj in UpperView.transform ){
			if(obj.name == pushedTabName){
				obj.GetComponent<Image> ().color = pushedTabColor;
				obj.transform.FindChild ("Text").GetComponent<Text> ().color = pushedTextColor;	
                obj.GetComponent<ZukanMenu>().pushedFlg = true;
			}else{
				obj.GetComponent<Image> ().color = normalTabColor;
				obj.transform.FindChild ("Text").GetComponent<Text> ().color = normalTextColor;
                obj.GetComponent<ZukanMenu>().pushedFlg = false;

            }
		}
	}



	public void deletePrevious(GameObject Content){

		foreach ( Transform n in Content.transform ){
			GameObject.Destroy(n.gameObject);
		}
	}


	public void showBusyoZukan(GameObject Content){

		//Prepare Master & History
		Entity_busyo_mst tempBusyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		string zukanBusyoHst = PlayerPrefs.GetString ("zukanBusyoHst");
		List<string> zukanBusyoHstList = new List<string> ();
		char[] delimiterChars = {','};
		if (zukanBusyoHst != null && zukanBusyoHst != "") {
			if (zukanBusyoHst.Contains (",")) {
				zukanBusyoHstList = new List<string> (zukanBusyoHst.Split (delimiterChars));
			} else {
				zukanBusyoHstList.Add (zukanBusyoHst);
			}
		}

        //add temporary daimyo busyo
        int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        Daimyo daimyo = new Daimyo();
        int myDaimyoBusyoId = daimyo.getDaimyoBusyoId(myDaimyo);
        if(!zukanBusyoHstList.Contains(myDaimyoBusyoId.ToString())) {
            zukanBusyoHstList.Add(myDaimyoBusyoId.ToString());
        }

        //Sort Master by daimyo
        Entity_busyo_mst busyoMst = new Entity_busyo_mst();
		busyoMst.param.AddRange (tempBusyoMst.param);
		busyoMst.param.Sort((a, b) =>  a.daimyoId - b.daimyoId);

		//Show busyo
		string path = "Prefabs/Zukan/zukanBusyo";
		string nameRankPath = "Prefabs/Zukan/NameRank";

		int NowQty = 0;
		int AllQty = 0;

		for (int i=0; i<busyoMst.param.Count; i++) {

			int daimyoId = busyoMst.param[i].daimyoId;
			if (daimyoId == 0) {
				daimyoId = busyoMst.param[i].daimyoHst;
			}

			if(daimyoId != 0){
				AllQty = AllQty + 1;

                BusyoInfoGet busyoScript = new BusyoInfoGet();
				int busyoId = busyoMst.param[i].id;
                string busyoName = busyoScript.getName(busyoId);
                string busyoRank = busyoMst.param[i].rank;

				GameObject back = Instantiate (Resources.Load (path)) as GameObject;			
				back.transform.SetParent (Content.transform);
				back.transform.localScale = new Vector2 (1, 1);
				back.transform.localPosition = new Vector3 (0, 0, 0);

				GameObject kamon = back.transform.FindChild("kamon").gameObject;

				//Busyo Icon
				string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
				GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;	
				busyo.name = busyoId.ToString();
				busyo.transform.SetParent (back.transform);
				busyo.transform.localScale = new Vector2 (4, 4);
				busyo.GetComponent<DragHandler>().enabled = false;
				foreach(Transform n in busyo.transform){
					GameObject.Destroy(n.gameObject);
				}
				RectTransform busyoRect = busyo.GetComponent<RectTransform>();
				busyoRect.anchoredPosition3D = new Vector3(80,80,0);
				busyoRect.sizeDelta = new Vector2(40,40);

				//Kamon
				string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString ();
				kamon.GetComponent<Image> ().sprite = 
					Resources.Load (imagePath, typeof(Sprite)) as Sprite;

				//Name
				GameObject nameRank = Instantiate (Resources.Load (nameRankPath)) as GameObject;			
				nameRank.transform.SetParent (back.transform);
				nameRank.transform.localScale = new Vector2 (1, 1);
				nameRank.transform.localPosition = new Vector3 (0, 0, 0);


				GameObject rank = nameRank.transform.FindChild("Rank").gameObject;
				rank.GetComponent<Text>().text = busyoRank;


				//Have or not
				if(zukanBusyoHstList.Contains(busyoId.ToString())){
					NowQty = NowQty + 1;

					GameObject name = nameRank.transform.FindChild("Name").gameObject;
					name.GetComponent<Text>().text = busyoName;

					int hp = busyoMst.param[i].hp;
					int atk = busyoMst.param[i].atk;
					int dfc = busyoMst.param[i].dfc;
					int spd = busyoMst.param[i].spd;
					string heisyu = busyoMst.param[i].heisyu;
					int sendpouId = busyoMst.param[i].senpou_id;

					PopInfo popScript = back.GetComponent<PopInfo>();
					popScript.busyoId = busyoId;
					popScript.busyoName = busyoName;
					popScript.hp = hp;
					popScript.atk = atk;
					popScript.dfc = dfc;
					popScript.spd = spd;
					popScript.heisyu = heisyu;
					popScript.daimyoId = daimyoId;
					popScript.senpouId = sendpouId;

				}else{
					Color noBusyoColor = new Color (0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //Black
					Color backColor = new Color (95f / 255f, 95f / 255f, 95f / 255f, 240f / 255f); //Black
					Color kamonColor = new Color (105f / 255f, 105f / 255f, 105f / 255f, 135f / 255f); //Black

					back.GetComponent<Image>().color = backColor;
					back.GetComponent<Button>().enabled = false;
					busyo.GetComponent<Image>().color = noBusyoColor;
					kamon.GetComponent<Image>().color = kamonColor;
				}


			}

		}

		//Qty
		float percent = (float)NowQty/(float)AllQty * 100;
		GameObject.Find ("NowQty").GetComponent<Text> ().text = NowQty.ToString();
		GameObject.Find ("AllQty").GetComponent<Text> ().text = AllQty.ToString() +"("+ percent.ToString("F1") + "%)";

	}

	public void showKahouZukan(GameObject Content){

		int NowQty = 0;

		Entity_kahou_bugu_mst tmpbuguMst  = Resources.Load ("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;
		Entity_kahou_gusoku_mst tmpgusokuMst  = Resources.Load ("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
		Entity_kahou_kabuto_mst tmpkabutoMst  = Resources.Load ("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
		Entity_kahou_meiba_mst tmpmeibaMst  = Resources.Load ("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
		Entity_kahou_cyadougu_mst tmpcyadouguMst  = Resources.Load ("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
		Entity_kahou_chishikisyo_mst tmpchishikisyoMst  = Resources.Load ("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
		Entity_kahou_heihousyo_mst tmpheihousyoMst  = Resources.Load ("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;

		Entity_kahou_bugu_mst buguMst  = new Entity_kahou_bugu_mst();
		Entity_kahou_gusoku_mst gusokuMst = new Entity_kahou_gusoku_mst();
		Entity_kahou_kabuto_mst kabutoMst = new Entity_kahou_kabuto_mst();
		Entity_kahou_meiba_mst meibaMst = new Entity_kahou_meiba_mst();
		Entity_kahou_cyadougu_mst cyadouguMst = new Entity_kahou_cyadougu_mst();
		Entity_kahou_chishikisyo_mst chishikisyoMst = new Entity_kahou_chishikisyo_mst();
		Entity_kahou_heihousyo_mst heihousyoMst = new Entity_kahou_heihousyo_mst();

		buguMst.param.AddRange (tmpbuguMst.param);
		gusokuMst.param.AddRange (tmpgusokuMst.param);
		kabutoMst.param.AddRange (tmpkabutoMst.param);
		meibaMst.param.AddRange (tmpmeibaMst.param);
		cyadouguMst.param.AddRange (tmpcyadouguMst.param);
		chishikisyoMst.param.AddRange (tmpchishikisyoMst.param);
		heihousyoMst.param.AddRange (tmpheihousyoMst.param);


		//Bugu
		//Prepare Master & History
		string zukanBuguHst = PlayerPrefs.GetString ("zukanBuguHst");
		List<string> zukanBuguHstList = new List<string> ();
		char[] delimiterChars = {','};

		if (zukanBuguHst != "" && zukanBuguHst != null) {
			if (zukanBuguHst.Contains (",")) {
				zukanBuguHstList = new List<string> (zukanBuguHst.Split (delimiterChars));
			} else {
				zukanBuguHstList.Add (zukanBuguHst);
			}
		}

		//Sort Master by daimyo
		buguMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });

		//Show Kahou
		string noBuguPath = "Prefabs/Item/Sprite/NoBugu";
		for (int i=0; i<buguMst.param.Count; i++) {
			int kahouId = buguMst.param[i].id;

			//Get Status
			string kahouPath = "Prefabs/Item/Kahou/bugu" + kahouId; 
			GameObject kahouIcon = Instantiate (Resources.Load (kahouPath)) as GameObject;
			kahouIcon.transform.SetParent(Content.transform);
			kahouIcon.transform.localScale = new Vector2 (1, 1);
			kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

			if(!zukanBuguHstList.Contains(kahouId.ToString())){
				//Don't have
				kahouIcon.GetComponent<Image> ().sprite = 
					Resources.Load (noBuguPath, typeof(Sprite)) as Sprite;

				kahouIcon.GetComponent<Button>().enabled = false;
			}else{
				NowQty = NowQty + 1;
				kahouIcon.GetComponent<KahouInfo>().kahouType = "bugu";
				kahouIcon.GetComponent<KahouInfo>().kahouId =  kahouId;
			}
			
		}

		//Gusoku
		//Prepare Master & History
		string zukanGusokuHst = PlayerPrefs.GetString ("zukanGusokuHst");
		List<string> zukanGusokuHstList = new List<string> ();
		if (zukanGusokuHst != "" && zukanGusokuHst != null) {
			if (zukanGusokuHst.Contains (",")) {
				zukanGusokuHstList = new List<string> (zukanGusokuHst.Split (delimiterChars));
			} else {
				zukanGusokuHstList.Add (zukanGusokuHst);
			}
		}

		//Sort Master by daimyo
		gusokuMst.param.Sort((a, b) => { return a.kahouRank.CompareTo(b.kahouRank); });

		//Show Kahou
		string noGusokuPath = "Prefabs/Item/Sprite/NoGusoku";
		for (int i=0; i<gusokuMst.param.Count; i++) {
			int kahouId = gusokuMst.param[i].id;

			string kahouPath = "Prefabs/Item/Kahou/gusoku" + kahouId;
			GameObject kahouIcon = Instantiate (Resources.Load (kahouPath)) as GameObject;
			kahouIcon.transform.SetParent(Content.transform);
			kahouIcon.transform.localScale = new Vector2 (1, 1);
			kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

			if(!zukanGusokuHstList.Contains(kahouId.ToString())){
				//Don't have
				kahouIcon.GetComponent<Image> ().sprite = 
					Resources.Load (noGusokuPath, typeof(Sprite)) as Sprite;
				
				kahouIcon.GetComponent<Button>().enabled = false;
			}else{
				NowQty = NowQty + 1;
				kahouIcon.GetComponent<KahouInfo>().kahouType = "gusoku";
				kahouIcon.GetComponent<KahouInfo>().kahouId =  kahouId;
			}	
		}


		//Kabuto
		//Prepare Master & History
		string zukanKabutoHst = PlayerPrefs.GetString ("zukanKabutoHst");
		List<string> zukanKabutoHstList = new List<string> ();
		
		if (zukanKabutoHst != "" && zukanKabutoHst != null) {
			if (zukanKabutoHst.Contains (",")) {
				zukanKabutoHstList = new List<string> (zukanKabutoHst.Split (delimiterChars));
			} else {
				zukanKabutoHstList.Add (zukanKabutoHst);
			}
		}
		
		//Sort Master by daimyo
		kabutoMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });
		
		//Show Kahou
		string noKabutoPath = "Prefabs/Item/Sprite/NoKabuto";
		for (int i=0; i<kabutoMst.param.Count; i++) {
			int kahouId = kabutoMst.param[i].id;

			string kahouPath = "Prefabs/Item/Kahou/kabuto" + kahouId; 
			GameObject kahouIcon = Instantiate (Resources.Load (kahouPath)) as GameObject;
			kahouIcon.transform.SetParent(Content.transform);
			kahouIcon.transform.localScale = new Vector2 (1, 1);
			kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

			if(!zukanKabutoHstList.Contains(kahouId.ToString())){
				//Don't have
				kahouIcon.GetComponent<Image> ().sprite = 
					Resources.Load (noKabutoPath	, typeof(Sprite)) as Sprite;
				
				kahouIcon.GetComponent<Button>().enabled = false;
			}else{
				NowQty = NowQty + 1;
				kahouIcon.GetComponent<KahouInfo>().kahouType = "kabuto";
				kahouIcon.GetComponent<KahouInfo>().kahouId =  kahouId;
			}	
		}


		// Meiba
		//Prepare Master & History
		string zukanMeibaHst = PlayerPrefs.GetString ("zukanMeibaHst");
		List<string> zukanMeibaHstList = new List<string> ();
		
		if (zukanMeibaHst != "" && zukanMeibaHst != null) {
			if (zukanMeibaHst.Contains (",")) {
				zukanMeibaHstList = new List<string> (zukanMeibaHst.Split (delimiterChars));
			} else {
				zukanMeibaHstList.Add (zukanMeibaHst);
			}
		}
		
		//Sort Master by daimyo
		meibaMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });
		
		//Show Kahou
		string noMeibaPath = "Prefabs/Item/Sprite/NoMeiba";
		for (int i=0; i<meibaMst.param.Count; i++) {
			int kahouId = meibaMst.param[i].id;

			string kahouPath = "Prefabs/Item/Kahou/meiba" + kahouId; 
			GameObject kahouIcon = Instantiate (Resources.Load (kahouPath)) as GameObject;
			kahouIcon.transform.SetParent(Content.transform);
			kahouIcon.transform.localScale = new Vector2 (1, 1);
			kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

			if(!zukanMeibaHstList.Contains(kahouId.ToString())){
				//Don't have
				kahouIcon.GetComponent<Image> ().sprite = 
					Resources.Load (noMeibaPath, typeof(Sprite)) as Sprite;
				
				kahouIcon.GetComponent<Button>().enabled = false;
			}else{
				NowQty = NowQty + 1;
				kahouIcon.GetComponent<KahouInfo>().kahouType = "meiba";
				kahouIcon.GetComponent<KahouInfo>().kahouId =  kahouId;
			}	
		}



		//Cyadougu
		//Prepare Master & History
		string zukanCyadouguHst = PlayerPrefs.GetString ("zukanCyadouguHst");
		List<string> zukanCyadouguHstList = new List<string> ();
		
		if (zukanCyadouguHst != "" && zukanCyadouguHst != null) {
			if (zukanCyadouguHst.Contains (",")) {
				zukanCyadouguHstList = new List<string> (zukanCyadouguHst.Split (delimiterChars));
			} else {
				zukanCyadouguHstList.Add (zukanCyadouguHst);
			}
		}
		
		//Sort Master by daimyo
		cyadouguMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });
		
		//Show Kahou
		string noCyadouguPath = "Prefabs/Item/Sprite/NoCyadougu";
		for (int i=0; i<cyadouguMst.param.Count; i++) {
			int kahouId = cyadouguMst.param[i].id;

			string kahouPath = "Prefabs/Item/Kahou/cyadougu" + kahouId; 
			GameObject kahouIcon = Instantiate (Resources.Load (kahouPath)) as GameObject;
			kahouIcon.transform.SetParent(Content.transform);
			kahouIcon.transform.localScale = new Vector2 (1, 1);
			kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

			if(!zukanCyadouguHstList.Contains(kahouId.ToString())){
				//Don't have
				kahouIcon.GetComponent<Image> ().sprite = 
					Resources.Load (noCyadouguPath, typeof(Sprite)) as Sprite;
				
				kahouIcon.GetComponent<Button>().enabled = false;
			}else{
				NowQty = NowQty + 1;
				kahouIcon.GetComponent<KahouInfo>().kahouType = "cyadougu";
				kahouIcon.GetComponent<KahouInfo>().kahouId =  kahouId;
			}	
		}


		// Chishikisyo
		//Prepare Master & History
		string zukanChishikisyoHst = PlayerPrefs.GetString ("zukanChishikisyoHst");
		List<string> zukanChishikisyoHstList = new List<string> ();
		
		if (zukanChishikisyoHst != "" && zukanChishikisyoHst != null) {
			if (zukanChishikisyoHst.Contains (",")) {
				zukanChishikisyoHstList = new List<string> (zukanChishikisyoHst.Split (delimiterChars));
			} else {
				zukanChishikisyoHstList.Add (zukanChishikisyoHst);
			}
		}
		
		//Sort Master by daimyo
		chishikisyoMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });
		
		//Show Kahou
		string noChishikisyoPath = "Prefabs/Item/Sprite/NoChishikisyo";
		for (int i=0; i<chishikisyoMst.param.Count; i++) {
			int kahouId = chishikisyoMst.param[i].id;

			string kahouPath = "Prefabs/Item/Kahou/chishikisyo" + kahouId; 
			GameObject kahouIcon = Instantiate (Resources.Load (kahouPath)) as GameObject;
			kahouIcon.transform.SetParent(Content.transform);
			kahouIcon.transform.localScale = new Vector2 (1, 1);
			kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

			if(!zukanChishikisyoHstList.Contains(kahouId.ToString())){
				//Don't have
				kahouIcon.GetComponent<Image> ().sprite = 
					Resources.Load (noChishikisyoPath, typeof(Sprite)) as Sprite;
				
				kahouIcon.GetComponent<Button>().enabled = false;
			}else{
				NowQty = NowQty + 1;

				kahouIcon.GetComponent<KahouInfo>().kahouType = "chishikisyo";
				kahouIcon.GetComponent<KahouInfo>().kahouId =  kahouId;

			}	
		}


		
		// Heihousyo
		//Prepare Master & History
		string zukanHeihousyoHst = PlayerPrefs.GetString ("zukanHeihousyoHst");
		List<string> zukanHeihousyoHstList = new List<string> ();
		
		if (zukanHeihousyoHst != "" && zukanHeihousyoHst != null) {
			if (zukanHeihousyoHst.Contains (",")) {
				zukanHeihousyoHstList = new List<string> (zukanHeihousyoHst.Split (delimiterChars));
			} else {
				zukanHeihousyoHstList.Add (zukanHeihousyoHst);
			}
		}
		
		//Sort Master by daimyo
		heihousyoMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });
		
		//Show Kahou
		string noHeihousyoPath = "Prefabs/Item/Sprite/NoHeihousyo";
		for (int i=0; i<heihousyoMst.param.Count; i++) {
			int kahouId = heihousyoMst.param[i].id;

			string kahouPath = "Prefabs/Item/Kahou/heihousyo" + kahouId; 
			GameObject kahouIcon = Instantiate (Resources.Load (kahouPath)) as GameObject;
			kahouIcon.transform.SetParent(Content.transform);
			kahouIcon.transform.localScale = new Vector2 (1, 1);
			kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

			if(!zukanHeihousyoHstList.Contains(kahouId.ToString())){
				//Don't have
				kahouIcon.GetComponent<Image> ().sprite = 
					Resources.Load (noHeihousyoPath, typeof(Sprite)) as Sprite;
				
				kahouIcon.GetComponent<Button>().enabled = false;
			}else{
				NowQty = NowQty + 1;

				kahouIcon.GetComponent<KahouInfo>().kahouType = "heihousyo";
				kahouIcon.GetComponent<KahouInfo>().kahouId =  kahouId;
			}	
		}



		//Qty
		int AllQty = buguMst.param.Count + gusokuMst.param.Count + kabutoMst.param.Count + meibaMst.param.Count + cyadouguMst.param.Count + chishikisyoMst.param.Count + heihousyoMst.param.Count; 
		float percent = (float)NowQty/(float)AllQty * 100;
		GameObject.Find ("NowQty").GetComponent<Text> ().text = NowQty.ToString();
		GameObject.Find ("AllQty").GetComponent<Text> ().text = AllQty.ToString() +"("+ percent.ToString("F1") + "%)";

	}

	public void showTenkahubuZukan(GameObject Content){

		Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;
		Color backColor = new Color (95f / 255f, 95f / 255f, 95f / 255f, 210f / 255f); //Black
		Color kamonColor = new Color (105f / 255f, 105f / 255f, 105f / 255f, 135f / 255f); //Black

		string gameClearDaimyo = PlayerPrefs.GetString ("gameClearDaimyo");
		List<string> gameClearDaimyoList = new List<string> ();
		char[] delimiterChars = {','};
		if (gameClearDaimyo != null && gameClearDaimyo != "") {
			if (gameClearDaimyo.Contains (",")) {
				gameClearDaimyoList = new List<string> (gameClearDaimyo.Split (delimiterChars));
			} else {
				gameClearDaimyoList.Add (gameClearDaimyo);
			}
		}

        string gameClearDaimyoHard = PlayerPrefs.GetString("gameClearDaimyoHard");
        List<string> gameClearDaimyoHardList = new List<string>();
        if (gameClearDaimyoHard != null && gameClearDaimyoHard != "") {
            if (gameClearDaimyoHard.Contains(",")) {
                gameClearDaimyoHardList = new List<string>(gameClearDaimyoHard.Split(delimiterChars));
            }else {
                gameClearDaimyoHardList.Add(gameClearDaimyoHard);
            }
        }

        string tenkahubuPath = "Prefabs/Item/Tenkahubu/tenkahubu";
		string nameWhitePath = "Prefabs/Zukan/NameWhite";
		string nameBlackPath = "Prefabs/Zukan/NameBlack";
		for (int i=0; i<daimyoMst.param.Count; i++) {
			int daimyoId = daimyoMst.param[i].daimyoId;
			GameObject tenkahubuIcon = Instantiate (Resources.Load (tenkahubuPath)) as GameObject;
			tenkahubuIcon.transform.SetParent(Content.transform);
			tenkahubuIcon.transform.localScale = new Vector2 (1, 1);
			tenkahubuIcon.transform.localPosition = new Vector3 (0, 0, 0);

			GameObject kamonIcon = tenkahubuIcon.transform.FindChild("kamon").gameObject;
			string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString ();
			kamonIcon.GetComponent<Image> ().sprite = 
				Resources.Load (imagePath, typeof(Sprite)) as Sprite;

			RectTransform kamonRect = kamonIcon.GetComponent<RectTransform>();
			kamonRect.anchoredPosition3D = new Vector3(80,-95,0);
			kamonRect.sizeDelta = new Vector3(100,100,0);

			GameObject tenkaIcon = tenkahubuIcon.transform.FindChild("Image").gameObject;
			RectTransform tenkaRect = tenkaIcon.GetComponent<RectTransform>();
			tenkaRect.anchoredPosition3D = new Vector3(20,10,0);
			tenkaRect.sizeDelta = new Vector3(80,100,0);


			if(!gameClearDaimyoList.Contains(daimyoId.ToString())){
				tenkahubuIcon.GetComponent<Image>().color = backColor;
				kamonIcon.GetComponent<Image>().color = kamonColor;
				tenkahubuIcon.transform.FindChild("Image").GetComponent<Image>().enabled = false;

				GameObject nameObj = Instantiate (Resources.Load (nameWhitePath)) as GameObject;
				nameObj.transform.SetParent(tenkahubuIcon.transform);
				nameObj.transform.localScale = new Vector2 (0.25f, 0.25f);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    nameObj.GetComponent<Text>().text = daimyoMst.param[i].daimyoNameEng;
                }else {
                    nameObj.GetComponent<Text>().text = daimyoMst.param[i].daimyoName;
                }
				nameObj.transform.localPosition = new Vector2(0,60);
                Destroy(tenkahubuIcon.transform.FindChild("Hard").gameObject);

            }else{
				GameObject nameObj = Instantiate (Resources.Load (nameBlackPath)) as GameObject;
				nameObj.transform.SetParent(tenkahubuIcon.transform);
				nameObj.transform.localScale = new Vector2 (0.25f, 0.25f);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    nameObj.GetComponent<Text>().text = daimyoMst.param[i].daimyoNameEng;
                }else {
                    nameObj.GetComponent<Text>().text = daimyoMst.param[i].daimyoName;
                }
				nameObj.transform.localPosition = new Vector2(0,60);
               
               
                if (!gameClearDaimyoHardList.Contains((daimyoId.ToString()))) {
                    Destroy(tenkahubuIcon.transform.FindChild("Hard").gameObject);
                }
                

            }


		}

		//Qty
		int NowQty = gameClearDaimyoList.Count;
		int AllQty = daimyoMst.param.Count;
		float percent = (float)NowQty/(float)AllQty * 100;
		GameObject.Find ("NowQty").GetComponent<Text> ().text = NowQty.ToString();
		GameObject.Find ("AllQty").GetComponent<Text> ().text = AllQty.ToString() +"("+ percent.ToString("F1") + "%)";

	}



}
