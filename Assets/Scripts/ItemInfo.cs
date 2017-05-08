﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemInfo : MonoBehaviour {
	
	public int posessQty;
	public int itemId = 0;
	public string syoukaiName = "";

	//Koueki
	public int yukoudo = 0;
	public int buyUnitPirce = 0;
	public GameObject buySlider;
	public GameObject Content;
	public GameObject buyBtn;

	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (Application.loadedLevelName == "souko") {
			audioSources [0].Play ();

			List<string> itemInfoList = new List<string> ();
			itemInfoList = getItemInfo (name);

			//Enable
			GameObject.Find ("GetMoney").GetComponent<Image> ().enabled = true;
			GameObject sellBtn = GameObject.Find ("SellButton");
			sellBtn.GetComponent<Image> ().enabled = true;
			sellBtn.GetComponent<Button> ().enabled = true;
			sellBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;

			GameObject.Find ("Background").GetComponent<Image> ().enabled = true;
			GameObject.Find ("Fill").GetComponent<Image> ().enabled = true;
			GameObject.Find ("Handle").GetComponent<Image> ().enabled = true;
			GameObject.Find ("SellQty").GetComponent<Image> ().enabled = true;
			
			//Pass value
			GameObject.Find ("ItemNameValue").GetComponent<Text> ().text = itemInfoList [1];

			string douguExp = itemInfoList [2];
			if (name == "cyoutei" || name == "koueki") {
				douguExp = douguExp.Replace ("A", syoukaiName);
				sellBtn.GetComponent<DoSell> ().itemId = itemId;
			}

			GameObject.Find ("DouguExpValue").GetComponent<Text> ().text = douguExp;

			//Slider value
			GameObject SellSlider = GameObject.Find ("SellSlider");
			SellSlider.GetComponent<Slider> ().maxValue = posessQty;
			SellSlider.GetComponent<Slider> ().minValue = 1;
			SellSlider.GetComponent<SellSlider> ().unitPrice = int.Parse (itemInfoList [5]);

			//Default value
			GameObject.Find ("SellQtyValue").GetComponent<Text> ().text = "1";
			GameObject.Find ("GetMoneyValue").GetComponent<Text> ().text = "+" + itemInfoList [5].ToString ();
			SellSlider.GetComponent<Slider> ().value = 1;


			//Delete Previous Icon
			GameObject itemView = GameObject.Find ("ItemView");
			foreach (Transform n in itemView.transform) {
				if (n.tag == "Kahou") {
					GameObject.Destroy (n.gameObject);
				}
			}


			//GameObject itemIconView = Instantiate (gameObject) as GameObject;
			GameObject itemIconView = Object.Instantiate (gameObject, itemView.transform) as GameObject;
			itemIconView.transform.localScale = new Vector2 (1, 1);
            itemIconView.transform.localPosition = new Vector3 (0, 120, 0);
            itemIconView.transform.FindChild ("Qty").GetComponent<Text> ().text = "";
			itemIconView.GetComponent<Button> ().enabled = false;
            RectTransform rt = itemIconView.GetComponent(typeof(RectTransform)) as RectTransform;
            rt.sizeDelta = new Vector2(150, 150);


            //Sell Button Set
            sellBtn.GetComponent<DoSell> ().kahouName = itemInfoList [1];
			sellBtn.GetComponent<DoSell> ().kahouType = name;
			sellBtn.GetComponent<DoSell> ().kahouSell = 0;


		} else if (Application.loadedLevelName == "mainStage") {
			audioSources [2].Play ();

			//Change Color
			Color Select = new Color (76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
			Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
			
			foreach (Transform obj in Content.transform) {
				obj.GetComponent<Image>().color = unSelect;
			}
			GetComponent<Image> ().color = Select;

			buySlider.GetComponent<Slider> ().maxValue = 10;
			buySlider.GetComponent<Slider> ().minValue = 1;

			buySlider.GetComponent<BuySlider> ().unitPrice = buyUnitPirce;
			buySlider.GetComponent<Slider> ().value = 1;

			GameObject.Find("BuyMenu").transform.FindChild("MoneyAmt").GetComponent<Text>().text = buyUnitPirce.ToString();
		
			buyBtn.GetComponent<DoBuy>().item = name;
		}
	}

	public List<string> getItemInfo(string itemCode){
		List<string> itemInfoList = new List<string>();

		Entity_item_mst itemMst  = Resources.Load ("Data/item_mst") as Entity_item_mst;

		for(int i=0; i<itemMst.param.Count; i++){
			string MstItemCode = itemMst.param [i].itemCode;
			if(itemCode == MstItemCode ){

				itemInfoList.Add (itemMst.param[i].itemCode);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    itemInfoList.Add (itemMst.param[i].itemNameEng);
                    itemInfoList.Add(itemMst.param[i].itemExpEng);
                }else {
                    itemInfoList.Add(itemMst.param[i].itemName);
                    itemInfoList.Add(itemMst.param[i].itemExp);
                }
				itemInfoList.Add (itemMst.param[i].effect.ToString());
				itemInfoList.Add (itemMst.param[i].buy.ToString());
				itemInfoList.Add (itemMst.param[i].sell.ToString());
				itemInfoList.Add (itemMst.param[i].itemRatio.ToString());
				break;
			}
		}
		return itemInfoList;
	}

	public string getItemName(string itemCode){
		string itemName = "";
		
		Entity_item_mst itemMst  = Resources.Load ("Data/item_mst") as Entity_item_mst;
		
		for(int i=0; i<itemMst.param.Count; i++){
			string MstItemCode = itemMst.param [i].itemCode;
			if(itemCode == MstItemCode ){
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    itemName =itemMst.param[i].itemNameEng;
                }else {
                    itemName = itemMst.param[i].itemName;
                }
			}
		}
		return itemName;
	}

	public int getItemEffect(string itemCode){
		int itemEffect = 0;
		
		Entity_item_mst itemMst  = Resources.Load ("Data/item_mst") as Entity_item_mst;
		
		for(int i=0; i<itemMst.param.Count; i++){
			string MstItemCode = itemMst.param [i].itemCode;
			if(itemCode == MstItemCode ){	
				itemEffect = itemMst.param[i].effect;
			}
		}
		return itemEffect;
	}



}