﻿using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class DoSyouninMenu : MonoBehaviour {

	public int price = 0;
	public GameObject Money;

	//Kahou
	public string kahouCd = "";
	public int kahouId = 0;

	//Busshi
	public string busshiCd = "";
	public int busshiQty = 0;

	//Kengou
	public int kengouId = 0;

	//Tech
	public int techId = 0;

	//Cyakai
	public bool doneCyadouguFlg;
	public int targetKuniQty = 0;
	public List<string> cyakaiDouguHstlist;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		Color enableImageColor = new Color (35f / 255f, 35f / 255f, 35f / 255f, 155f / 255f);
		Color enableTextColor = new Color (125f / 255f, 125f / 255f, 125f / 255f, 255f / 255f);

		if (name == "DoKahouButton") {

			reduceActionQty ();

			int money = PlayerPrefs.GetInt ("money");
			int paiedMoney = int.Parse (Money.GetComponent<Text> ().text);
			Message msg = new Message ();
			Kahou kahou = new Kahou ();

			if (paiedMoney <= money) {
				audioSources [3].Play ();

				//reduce money
				int calc = money - paiedMoney;
				PlayerPrefs.SetInt ("money", calc);
				PlayerPrefs.Flush ();
				GameObject.Find ("MoneyValue").GetComponent<Text> ().text = calc.ToString ();

				if (kahouCd == "bugu") {
					kahou.registerBugu (kahouId);
				} else if (kahouCd == "gusoku") {
					kahou.registerGusoku (kahouId);
				} else if (kahouCd == "kabuto") {
					kahou.registerKabuto (kahouId);
				} else if (kahouCd == "meiba") {
					kahou.registerMeiba (kahouId);
				} else if (kahouCd == "cyadougu") {
					kahou.registerCyadougu (kahouId);
				} else if (kahouCd == "chishikisyo") {
					kahou.registerChishikisyo (kahouId);
				} else if (kahouCd == "heihousyo") {
					kahou.registerHeihousyo (kahouId);					
				}

				msg.makeMessage (msg.getMessage(92));

				//Close
				GameObject.Find ("MenuKahou").transform.FindChild ("Close").GetComponent<CloseMenu> ().OnClick ();

				GameObject btn = GameObject.Find ("SyouninBoard").transform.FindChild ("Kahou").gameObject;
				btn.GetComponent<Button> ().enabled = false;
				btn.GetComponent<Image> ().color = enableImageColor;
				btn.transform.FindChild ("Text").GetComponent<Text> ().color = enableTextColor;


			} else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(6));
			}


		} else if (name == "DoBusshiButton") {
			
			reduceActionQty ();

			int money = PlayerPrefs.GetInt ("money");
			int paiedMoney = int.Parse (Money.GetComponent<Text> ().text);
			Message msg = new Message ();

			if (paiedMoney <= money) {
				audioSources [3].Play ();

				//reduce money
				int calc = money - paiedMoney;
				PlayerPrefs.SetInt ("money", calc);
				PlayerPrefs.Flush ();
				GameObject.Find ("MoneyValue").GetComponent<Text> ().text = calc.ToString ();
				char[] delimiterChars = {','};

				if (busshiCd.Contains ("Cyouhei") == true) {
					if (busshiCd.Contains ("YR") == true) {
						string cyouheiYRString = PlayerPrefs.GetString ("cyouheiYR");

						string[] cyouheiYR_list = cyouheiYRString.Split (delimiterChars);
						string newCyouheiYRString = "";
						
						if (busshiCd == "CyouheiYR1") {
							int newQty = 0;
							newQty = int.Parse (cyouheiYR_list [0]) + busshiQty;
							newCyouheiYRString = newQty.ToString () + "," + cyouheiYR_list [1] + "," + cyouheiYR_list [2];
							
						} else if (busshiCd == "CyouheiYR2") {
							int newQty = 0;
							newQty = int.Parse (cyouheiYR_list [1]) + busshiQty;
							newCyouheiYRString = cyouheiYR_list [0] + "," + newQty.ToString () + "," + cyouheiYR_list [2];
							
						} else if (busshiCd == "CyouheiYR3") {
							int newQty = 0;
							newQty = int.Parse (cyouheiYR_list [2]) + busshiQty;
							newCyouheiYRString = cyouheiYR_list [0] + "," + cyouheiYR_list [1] + "," + newQty.ToString ();
							
						}
						PlayerPrefs.SetString ("cyouheiYR", newCyouheiYRString);
						
					} else if (busshiCd.Contains ("KB") == true) {
						string cyouheiKBString = PlayerPrefs.GetString ("cyouheiKB");
						string[] cyouheiKB_list = cyouheiKBString.Split (delimiterChars);
						string newCyouheiKBString = "";
						
						if (busshiCd == "CyouheiKB1") {
							int newQty = 0;
							newQty = int.Parse (cyouheiKB_list [0]) + busshiQty;
							newCyouheiKBString = newQty.ToString () + "," + cyouheiKB_list [1] + "," + cyouheiKB_list [2];
							
							
						} else if (busshiCd == "CyouheiKB2") {
							int newQty = 0;
							newQty = int.Parse (cyouheiKB_list [1]) + busshiQty;
							newCyouheiKBString = cyouheiKB_list [0] + "," + newQty.ToString () + "," + cyouheiKB_list [2];
							
						} else if (busshiCd == "CyouheiKB3") {
							int newQty = 0;
							newQty = int.Parse (cyouheiKB_list [2]) + busshiQty;
							newCyouheiKBString = cyouheiKB_list [0] + "," + cyouheiKB_list [1] + "," + newQty.ToString ();
							
						}
						PlayerPrefs.SetString ("cyouheiKB", newCyouheiKBString);
						
					} else if (busshiCd.Contains ("TP") == true) {
						string cyouheiTPString = PlayerPrefs.GetString ("cyouheiTP");
						string[] cyouheiTP_list = cyouheiTPString.Split (delimiterChars);
						string newCyouheiTPString = "";
						
						if (busshiCd == "CyouheiTP1") {
							int newQty = 0;
							newQty = int.Parse (cyouheiTP_list [0]) + busshiQty;
							newCyouheiTPString = newQty.ToString () + "," + cyouheiTP_list [1] + "," + cyouheiTP_list [2];
							
							
						} else if (busshiCd == "CyouheiTP2") {
							int newQty = 0;
							newQty = int.Parse (cyouheiTP_list [1]) + busshiQty;
							newCyouheiTPString = cyouheiTP_list [0] + "," + newQty.ToString () + "," + cyouheiTP_list [2];
							
						} else if (busshiCd == "CyouheiTP3") {
							int newQty = 0;
							newQty = int.Parse (cyouheiTP_list [2]) + busshiQty;
							newCyouheiTPString = cyouheiTP_list [0] + "," + cyouheiTP_list [1] + "," + newQty.ToString ();
							
						}
						PlayerPrefs.SetString ("cyouheiTP", newCyouheiTPString);
						
					} else if (busshiCd.Contains ("YM") == true) {
						string cyouheiYMString = PlayerPrefs.GetString ("cyouheiYM");
						string[] cyouheiYM_list = cyouheiYMString.Split (delimiterChars);
						string newCyouheiYMString = "";
						
						if (busshiCd == "CyouheiYM1") {
							int newQty = 0;
							newQty = int.Parse (cyouheiYM_list [0]) + busshiQty;
							newCyouheiYMString = newQty.ToString () + "," + cyouheiYM_list [1] + "," + cyouheiYM_list [2];
							
							
						} else if (busshiCd == "CyouheiYM2") {
							int newQty = 0;
							newQty = int.Parse (cyouheiYM_list [1]) + busshiQty;
							newCyouheiYMString = cyouheiYM_list [0] + "," + newQty.ToString () + "," + cyouheiYM_list [2];
							
						} else if (busshiCd == "CyouheiYM3") {
							int newQty = 0;
							newQty = int.Parse (cyouheiYM_list [2]) + busshiQty;
							newCyouheiYMString = cyouheiYM_list [0] + "," + cyouheiYM_list [1] + "," + newQty.ToString ();
							
						}
						PlayerPrefs.SetString ("cyouheiYM", newCyouheiYMString);
					}

				} else if (busshiCd.Contains ("Shinobi") == true) {
					if (busshiCd == "Shinobi1") {
						int newQty = 0;
						int shinobiQty = PlayerPrefs.GetInt ("shinobiGe");
						newQty = shinobiQty + busshiQty;
						PlayerPrefs.SetInt ("shinobiGe", newQty);
						
					} else if (busshiCd == "Shinobi2") {
						int newQty = 0;
						int shinobiQty = PlayerPrefs.GetInt ("shinobiCyu");
						newQty = shinobiQty + busshiQty;
						PlayerPrefs.SetInt ("shinobiCyu", newQty);
						
					} else if (busshiCd == "Shinobi3") {
						int newQty = 0;
						int shinobiQty = PlayerPrefs.GetInt ("shinobiJyo");
						newQty = shinobiQty + busshiQty;
						PlayerPrefs.SetInt ("shinobiJyo", newQty);
						
					}
					
				}
				PlayerPrefs.Flush ();


				
				msg.makeMessage (msg.getMessage(93));
				
				//Close
				GameObject.Find ("MenuBusshi").transform.FindChild ("Close").GetComponent<CloseMenu> ().OnClick ();
				
				GameObject btn = GameObject.Find ("SyouninBoard").transform.FindChild ("Busshi").gameObject;
				btn.GetComponent<Button> ().enabled = false;
				btn.GetComponent<Image> ().color = enableImageColor;
				btn.transform.FindChild ("Text").GetComponent<Text> ().color = enableTextColor;
				
				
			} else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(6));
			}


		} else if (name == "DoRouninButton") {
			
			reduceActionQty ();

			int money = PlayerPrefs.GetInt ("money");
			Message msg = new Message ();
			
			if (price <= money) {
				audioSources [3].Play ();

				//reduce money
				int calc = money - price;
				PlayerPrefs.SetInt ("money", calc);
				PlayerPrefs.Flush ();
				GameObject.Find ("MoneyValue").GetComponent<Text> ().text = calc.ToString ();


				string kengouString = PlayerPrefs.GetString ("kengouItem");
				List<string> kengouList = new List<string> ();
				char[] delimiterChars = {','};
				kengouList = new List<string> (kengouString.Split (delimiterChars));

				int qty = int.Parse (kengouList [kengouId - 1]);
				int newQty = qty + 1;
				kengouList [kengouId - 1] = newQty.ToString ();
				
				string newKengouString = "";
				for (int i=0; i<kengouList.Count; i++) {				
					if (i == 0) {
						newKengouString = kengouList [i];
					} else {
						newKengouString = newKengouString + "," + kengouList [i];
					}
				}
				
				PlayerPrefs.SetString ("kengouItem", newKengouString);
				PlayerPrefs.Flush ();

				msg.makeMessage (msg.getMessage(94));
				
				//Close
				GameObject.Find ("MenuRounin").transform.FindChild ("Close").GetComponent<CloseMenu> ().OnClick ();

				GameObject btn = GameObject.Find ("SyouninBoard").transform.FindChild ("Youjinbou").gameObject;
				btn.GetComponent<Button> ().enabled = false;
				btn.GetComponent<Image> ().color = enableImageColor;
				btn.transform.FindChild ("Text").GetComponent<Text> ().color = enableTextColor;
			
			} else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(6));
			
			}


		} else if (name == "DoYasenButton") {

			reduceActionQty ();

			Message msg = new Message ();

			float percent = Random.value;
			percent = percent * 100;
			if (percent > 10) {
				audioSources [3].Play ();
				int money = PlayerPrefs.GetInt ("money");
				int newMoney = money + price;
                if (newMoney < 0) {
                    newMoney = int.MaxValue;
                }
                PlayerPrefs.SetInt ("money", newMoney);
				PlayerPrefs.Flush ();
				GameObject.Find ("MoneyValue").GetComponent<Text> ().text = newMoney.ToString ();
                string yasenTxt = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    yasenTxt = "We levied money " + price + " on merchants.";
                }else {
                    yasenTxt = price + "貫の矢銭を供出させましたぞ。";
                }
				msg.makeMessage (yasenTxt);

			} else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(95));
			}

			//Close
			GameObject.Find ("MenuYasen").transform.FindChild ("Close").GetComponent<CloseMenu> ().OnClick ();

			GameObject btn = GameObject.Find ("SyouninBoard").transform.FindChild ("Yasen").gameObject;
			btn.GetComponent<Button> ().enabled = false;
			btn.GetComponent<Image> ().color = enableImageColor;
			btn.transform.FindChild ("Text").GetComponent<Text> ().color = enableTextColor;


		
		} else if (name == "DoTechButton") {

			reduceActionQty ();
			
			int money = PlayerPrefs.GetInt ("money");
			Message msg = new Message ();

			if (price <= money) {
				audioSources [3].Play ();

				//reduce money
				int calc = money - price;
				PlayerPrefs.SetInt ("money", calc);
				GameObject.Find ("MoneyValue").GetComponent<Text> ().text = calc.ToString ();

				//add tech item
				string txt = "";
				if(techId == 1){
					//TP
					int qty = PlayerPrefs.GetInt("transferTP",0);
					int newQty = qty + 1;
					PlayerPrefs.SetInt("transferTP",newQty);
					txt = msg.getMessage(96);

				}else if(techId == 2){
					int qty = PlayerPrefs.GetInt("transferKB",0);
					int newQty = qty + 1;
					PlayerPrefs.SetInt("transferKB",newQty);
                    txt = msg.getMessage(97);

                }else if(techId == 3){
					int qty = PlayerPrefs.GetInt("transferSNB",0);
					int newQty = qty  + 1;
					PlayerPrefs.SetInt("transferSNB",newQty);
                    txt = msg.getMessage(98);

                }
				PlayerPrefs.Flush ();

				msg.makeMessage (txt);

				GameObject btn = GameObject.Find ("SyouninBoard").transform.FindChild ("Gijyutsu").gameObject;
				btn.GetComponent<Button> ().enabled = false;
				btn.GetComponent<Image> ().color = enableImageColor;
				btn.transform.FindChild ("Text").GetComponent<Text> ().color = enableTextColor;

			}else{
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(6));
			}

			//Close
			GameObject.Find ("MenuTech").transform.FindChild ("Close").GetComponent<CloseMenu> ().OnClick ();
			


		} else if(name == "DoCyakaiButton"){
			reduceActionQty ();

			int money = PlayerPrefs.GetInt ("money");
			Message msg = new Message ();

			if (price <= money) {
				audioSources [3].Play ();
				//reduce money
				int calc = money - price;
				PlayerPrefs.SetInt ("money", calc);
				GameObject.Find ("MoneyValue").GetComponent<Text> ().text = calc.ToString ();

				int meiseiQty = PlayerPrefs.GetInt ("meisei");
				meiseiQty = meiseiQty + targetKuniQty;
				PlayerPrefs.SetInt ("meisei",meiseiQty);

				string newcyakaiDouguHst = "";
				for (int i = 0; i < cyakaiDouguHstlist.Count; i++) {
					if (i == 0) {
						newcyakaiDouguHst = cyakaiDouguHstlist[i];
					} else {
						newcyakaiDouguHst = newcyakaiDouguHst + "," + cyakaiDouguHstlist[i];
					}
				}
				PlayerPrefs.SetString ("cyakaiDouguHst",newcyakaiDouguHst);

				string finalTxt = "";
				if (!doneCyadouguFlg) {
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        finalTxt = "It was good tea party.\n You got " + targetKuniQty + " reputation item. \n Traveller will visit your country.";
                    }else {
                        finalTxt = "茶会は大盛況でした。\n名声を" + targetKuniQty + "個取得しました。\n旅人が来訪いたしますぞ。";
                    }
				} else {
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        finalTxt = "You have already shown your tea item so it didn't get lively. \n You got " + targetKuniQty + " reputation item.";
				    }else {
                        finalTxt = "以前お披露目した茶道具を使用したせいか、盛り上がりは今ひとつでしたな。\n名声を" + targetKuniQty + "個取得しました。";
                    }
                }

				msg.makeMessage (finalTxt);

				GameObject btn = GameObject.Find ("SyouninBoard").transform.FindChild ("Cyakai").gameObject;
				btn.GetComponent<Button> ().enabled = false;
				btn.GetComponent<Image> ().color = enableImageColor;
				btn.transform.FindChild ("Text").GetComponent<Text> ().color = enableTextColor;

			} else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(6));
			}

			//Close
			GameObject.Find ("MenuCyakai").transform.FindChild ("Close").GetComponent<CloseMenu> ().OnClick ();
		}

	}

	public void reduceActionQty(){
		bool actionOKFlg = false;
		GameObject acrionValue = GameObject.Find ("ActionValue").gameObject;
		int actionRemainQty = int.Parse(acrionValue.GetComponent<Text> ().text);
		
		actionRemainQty = actionRemainQty - 1;
		acrionValue.GetComponent<Text>().text = actionRemainQty.ToString();

		//Track
		int TrackSyouninNo = PlayerPrefs.GetInt("TrackSyouninNo",0);
		TrackSyouninNo = TrackSyouninNo + 1;
		PlayerPrefs.SetInt ("TrackSyouninNo", TrackSyouninNo);

	}
}
