﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class ShisyaScene : MonoBehaviour {


	public GameObject firstSlot;

	void Start () {
		
		viewCurrentValue ();

		//Delete Previous Slot
		GameObject content = GameObject.Find("Content").gameObject;
		foreach (Transform obj in content.transform) {
			Destroy (obj.gameObject);
		}

		//Make Slot
		Shisya shisya = new Shisya();
		Daimyo daimyo = new Daimyo ();
		bool ClickFlg = false;

		for (int i = 1; i < 22; i++) {
			string tmp = "shisya" + i.ToString ();
			if (PlayerPrefs.HasKey (tmp)) {
				//Exist

				string shisyaString = PlayerPrefs.GetString (tmp);
				List<string> shisyaList = new List<string> ();
				char[] delimiterChars = {','};
				if (shisyaString != null && shisyaString != "") {
					if (shisyaString.Contains (",")) {
						shisyaList = new List<string> (shisyaString.Split (delimiterChars));
					} else {
						shisyaList.Add (shisyaString);
					}
				}

				for(int j=0; j<shisyaList.Count;j++){
					string shisyaParam = shisyaList [j];

					List<string> shisyaParamList = new List<string> ();
					char[] delimiterChars2 = {':'};
					if (shisyaParam.Contains (":")) {
						shisyaParamList = new List<string> (shisyaParam.Split (delimiterChars2));
					} else {
						shisyaParamList.Add (shisyaParam);
					}

					//Common
					string slotName = shisya.getSlot(i);
					string slotPath = "Prefabs/Shisya/Slot/" + slotName;
					GameObject slotObj = Instantiate (Resources.Load (slotPath)) as GameObject;
					slotObj.transform.SetParent (content.transform);
					slotObj.transform.localScale = new Vector2 (1,1);
					slotObj.transform.localPosition = new Vector3 (0,0,0);
					string title = shisya.getName(i);
					slotObj.transform.FindChild ("Title").GetComponent<Text> ().text = title;
					ShisyaSelect script = slotObj.GetComponent<ShisyaSelect> ();

					if (slotName == "DaimyoSlot") {
						string daimyoId = shisyaParamList [0];
						string imagePath = "Prefabs/Kamon/" + daimyoId.ToString ();
						slotObj.transform.FindChild ("Back").GetComponent<Image> ().sprite = 
							Resources.Load (imagePath, typeof(Sprite)) as Sprite;

						//Choose Buka
						int daimyoBusyoId = daimyo.getDaimyoBusyoId (int.Parse (daimyoId));
						int busyoId = getRandomBusyo (int.Parse (daimyoId), daimyoBusyoId);

						string busyoImagePath = "Prefabs/Player/Sprite/unit" + busyoId.ToString ();
						slotObj.transform.FindChild ("Image").GetComponent<Image> ().sprite = 
							Resources.Load (busyoImagePath, typeof(Sprite)) as Sprite;

						BusyoInfoGet busyo = new BusyoInfoGet ();
						string busyoName = busyo.getName (busyoId);
						string daimyoName = daimyo.getName (int.Parse (daimyoId));
                        if (Application.systemLanguage != SystemLanguage.Japanese) {                            
                            script.shisyaName = daimyoName + "'s retainer :" + busyoName;
                        }else {
                            script.shisyaName = daimyoName + "配下 " + busyoName;
                        }

					} else if (slotName == "SyogunSlot") {
						int syogunId = PlayerPrefs.GetInt ("syogunDaimyoId");
						string seiryoku = PlayerPrefs.GetString ("seiryoku");
						List<string> seiryokuList = new List<string> ();
						seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
						if (seiryokuList.Contains (syogunId.ToString ())) {
							string imagePath = "Prefabs/Kamon/" + syogunId.ToString ();
							slotObj.transform.FindChild ("Back").GetComponent<Image> ().sprite = 
							Resources.Load (imagePath, typeof(Sprite)) as Sprite;

							string daimyoName = daimyo.getName (syogunId);
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                script.shisyaName = " Syogun " + daimyoName + "'s retainer";
                            } else {
                                script.shisyaName = "将軍" + daimyoName + "配下";
                            }
						} else {
							PlayerPrefs.DeleteKey ("shisya" + i.ToString ());
						}
					} else {
						if (i == 13 || i == 15 || i == 16 || i == 17) {
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                script.shisyaName = "Nobleman";
                            }else {
                                script.shisyaName = "貴族";
                            }
						} else if(i == 18){
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                script.shisyaName = "Merchant";
                            }else {
                                script.shisyaName = "商人";
                            }
						} else if(i == 19){
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                script.shisyaName = "Westerner";
                            }else {
                                script.shisyaName = "南蛮人";
                            }
						}else if(i == 20){
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                script.shisyaName = "Monk";
                            }else {
                                script.shisyaName = "僧";
                            }
						}else if(i == 21){
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                script.shisyaName = "Local Samurai";
                            }else {
                                script.shisyaName = "国人衆";
                            }
						}
					}


					//Random Handling
					string requried1 = shisya.getYesRequried1 (i);
					if (requried1 == "random") {
						randomMyKahouView(slotObj);
					}else if(requried1 == "randomKahou"){
						randomSalesKahouView(slotObj);
					}


					//Set Value
					script.shisyaId = i;
					script.title = title;
					script.Content = content;

					int rdm = UnityEngine.Random.Range(1,4); //1-3
					string serihu = "";
					if (rdm == 1) {
						serihu = shisya.getSerihu1 (i);
					} else if (rdm == 2) {
						serihu = shisya.getSerihu2 (i);
					} else if (rdm == 3) {
						serihu = shisya.getSerihu3 (i);
					}
						
					serihu = serihuChanger(i, serihu, shisyaParamList,slotObj);
					script.serihu = serihu;


					if (!ClickFlg) {
						ClickFlg = true;
						firstSlot = slotObj;
					}
				}

			}
		}

		if (firstSlot != null) {
			firstSlot.GetComponent<ShisyaSelect> ().OnClick ();
		} else {
			Application.LoadLevel ("mainStage");
		}


	}


	public string serihuChanger(int shisyaId, string originalSerihu, List<string> shisyaParamList, GameObject slotObj){
		string finalSerihu = "";
		Daimyo daimyo = new Daimyo ();
		KuniInfo kuni = new KuniInfo ();

		if (shisyaId == 1 || shisyaId == 4 || shisyaId == 5 || shisyaId == 6 || shisyaId == 7 ||  shisyaId == 17) {
			string daimyoName = daimyo.getName (int.Parse (shisyaParamList [0]));
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                finalSerihu = originalSerihu.Replace ("ABC", daimyoName);
            }else {
                finalSerihu = originalSerihu.Replace("A", daimyoName);
            }
			slotObj.GetComponent<ShisyaSelect> ().srcDaimyoId = int.Parse (shisyaParamList [0]);
			slotObj.GetComponent<ShisyaSelect> ().srcDaimyoName = daimyoName;

			if (shisyaId == 5) {
				string itemName = slotObj.GetComponent<ShisyaSelect> ().itemName;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    finalSerihu = finalSerihu.Replace ("DEF", itemName);
                }else {
                    finalSerihu = finalSerihu.Replace("B", itemName);
                }
			}
			if (shisyaId == 7) {
				int randomMoney = UnityEngine.Random.Range(1,6);
				randomMoney = randomMoney * 1000;

				slotObj.GetComponent<ShisyaSelect> ().moneyNo = randomMoney;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    finalSerihu = finalSerihu.Replace ("DEF", randomMoney.ToString());
                }else {
                    finalSerihu = finalSerihu.Replace("B", randomMoney.ToString());
                }
			}

		} else if (shisyaId == 8) {
			string daimyoName = daimyo.getName (int.Parse (shisyaParamList [0]));
			string kuniName = kuni.getKuniName (int.Parse (shisyaParamList [1]));

            if (Application.systemLanguage != SystemLanguage.Japanese) {
                finalSerihu = originalSerihu.Replace ("ABC", daimyoName);
			    finalSerihu = finalSerihu.Replace ("DEF", kuniName);
            }else {
                finalSerihu = originalSerihu.Replace("A", daimyoName);
                finalSerihu = finalSerihu.Replace("B", kuniName);
            }

			slotObj.GetComponent<ShisyaSelect> ().key = shisyaParamList [2];
			slotObj.GetComponent<ShisyaSelect> ().srcDaimyoId = int.Parse (shisyaParamList [0]);
			slotObj.GetComponent<ShisyaSelect> ().srcDaimyoName = daimyoName;
			slotObj.GetComponent<ShisyaSelect> ().targetKuniId = int.Parse (shisyaParamList [1]);
			slotObj.GetComponent<ShisyaSelect> ().targetKuniName = kuniName;


		} else if (shisyaId == 9) {
			string daimyoName = daimyo.getName (int.Parse (shisyaParamList [0]));
			int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
			string myDaimyoName = daimyo.getName (myDaimyo);
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                finalSerihu = originalSerihu.Replace ("ABC", daimyoName);
			    finalSerihu = finalSerihu.Replace ("DEF", myDaimyoName);
            }else {
                finalSerihu = originalSerihu.Replace("A", daimyoName);
                finalSerihu = finalSerihu.Replace("B", myDaimyoName);
            }

			slotObj.GetComponent<ShisyaSelect> ().srcDaimyoId = int.Parse (shisyaParamList [0]);
			slotObj.GetComponent<ShisyaSelect> ().srcDaimyoName = daimyoName;

		} else if (shisyaId == 3) {
            string srcDaimyoName = daimyo.getName(int.Parse(shisyaParamList[0]));
            string daimyoName = daimyo.getName (int.Parse (shisyaParamList [1]));
			string kuniName = kuni.getKuniName (int.Parse (shisyaParamList [2]));

            if (Application.systemLanguage != SystemLanguage.Japanese) {
                finalSerihu = originalSerihu.Replace ("ABC", daimyoName);
			    finalSerihu = finalSerihu.Replace ("DEF", kuniName);
            }else {
                finalSerihu = originalSerihu.Replace("A", daimyoName);
                finalSerihu = finalSerihu.Replace("B", kuniName);
            }

			slotObj.GetComponent<ShisyaSelect> ().key = shisyaParamList [3];
			slotObj.GetComponent<ShisyaSelect> ().dstDaimyoId = int.Parse (shisyaParamList [1]);
			slotObj.GetComponent<ShisyaSelect> ().dstDaimyoName = daimyoName;
            slotObj.GetComponent<ShisyaSelect>().srcDaimyoId = int.Parse(shisyaParamList[0]);
            slotObj.GetComponent<ShisyaSelect>().srcDaimyoName = srcDaimyoName;
            slotObj.GetComponent<ShisyaSelect> ().targetKuniId = int.Parse (shisyaParamList [2]);
			slotObj.GetComponent<ShisyaSelect> ().targetKuniName = kuniName;

		} else if (shisyaId == 10 || shisyaId == 11 || shisyaId == 12 || shisyaId == 14) {
			//Syogun
			int syogunId = PlayerPrefs.GetInt ("syogunDaimyoId");
			string syogunName = daimyo.getName (syogunId);
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                finalSerihu = originalSerihu.Replace ("ABC", syogunName);
            }else {
                finalSerihu = originalSerihu.Replace("A", syogunName);
            }
			slotObj.GetComponent<ShisyaSelect> ().syogunDaimyoId = syogunId;
			slotObj.GetComponent<ShisyaSelect> ().syogunDaimyoName = syogunName;

			if (shisyaId == 10 || shisyaId == 11 || shisyaId == 12) {
				string daimyoName = daimyo.getName (int.Parse (shisyaParamList [0]));

                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    finalSerihu = finalSerihu.Replace ("DEF", daimyoName);
                }else {
                    finalSerihu = finalSerihu.Replace("B", daimyoName);
                }
				slotObj.GetComponent<ShisyaSelect> ().srcDaimyoId = int.Parse (shisyaParamList [0]);
				slotObj.GetComponent<ShisyaSelect> ().srcDaimyoName = daimyoName;

				if (shisyaId == 12) {
					//Bouei
					string dstDaimyoName = daimyo.getName (int.Parse (shisyaParamList [1]));
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        finalSerihu = finalSerihu.Replace ("GHI", dstDaimyoName);
                    }else {
                        finalSerihu = finalSerihu.Replace("C", dstDaimyoName);
                    }

					string dstKuniName = kuni.getKuniName (int.Parse (shisyaParamList [2]));
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        finalSerihu = finalSerihu.Replace ("JKL", dstKuniName);
                    }else {
                        finalSerihu = finalSerihu.Replace("D", dstKuniName);
                    }

					slotObj.GetComponent<ShisyaSelect> ().dstDaimyoId = int.Parse (shisyaParamList [1]);
					slotObj.GetComponent<ShisyaSelect> ().dstDaimyoName = dstDaimyoName;

					slotObj.GetComponent<ShisyaSelect> ().targetKuniId = int.Parse (shisyaParamList [2]);
					slotObj.GetComponent<ShisyaSelect> ().targetKuniName = dstKuniName;

					slotObj.GetComponent<ShisyaSelect> ().key = shisyaParamList [3];
				}
			}

		} else if (shisyaId == 19) {
			string itemName = slotObj.GetComponent<ShisyaSelect> ().itemName;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                finalSerihu = originalSerihu.Replace ("ABC", itemName);
            }else {
                finalSerihu = originalSerihu.Replace("A", itemName);
            }

		} else {
			finalSerihu = originalSerihu;
		}



		return finalSerihu;
	}

	public int getRandomBusyo(int activeDaimyoId, int daimyoBusyoId){
		
		/*Busyo Master Setting Start*/
		//Active Busyo List
		List<string> metsubouDaimyoList = new List<string> ();
		string metsubouTemp = "metsubou" + activeDaimyoId;
		string metsubouDaimyoString = PlayerPrefs.GetString (metsubouTemp);
		char[] delimiterChars2 = {','};
		if (metsubouDaimyoString != null && metsubouDaimyoString != "") {
			if (metsubouDaimyoString.Contains (",")) {
				metsubouDaimyoList = new List<string> (metsubouDaimyoString.Split (delimiterChars2));
			} else {
				metsubouDaimyoList = new List<string> (metsubouDaimyoString.Split (delimiterChars2));
			}
		}
		metsubouDaimyoList.Add (activeDaimyoId.ToString ());

		Entity_busyo_mst busyoMst = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		List<int> busyoList = new List<int> ();

		for (int i=0; i<busyoMst.param.Count; i++) {
			int busyoId = busyoMst.param [i].id;
			int daimyoId = busyoMst.param [i].daimyoId;

			if (metsubouDaimyoList.Contains (daimyoId.ToString ())) {
				if (busyoId != daimyoBusyoId) {

					busyoList.Add (busyoId);
				}
			}
		}
		/*Busyo Master Setting End*/

		/*Random Shuffle*/
		for (int i = 0; i < busyoList.Count; i++) {
			int temp = busyoList [i];
			int randomIndex = UnityEngine.Random.Range (0, busyoList.Count);
			busyoList [i] = busyoList [randomIndex];
			busyoList [randomIndex] = temp;
		}


		int returnValue = 0;
		if (busyoList.Count == 0) {
			returnValue = 35;
		} else {
			returnValue = busyoList[0];
		}
			

		return returnValue;

	}

	public void randomMyKahouView(GameObject slot){

		//Money or Item 0:money, 1:item
		int moneyOrItem = UnityEngine.Random.Range (0, 2);
		if (moneyOrItem == 0) {
			//money
			int requriedMoney = UnityEngine.Random.Range (1000, 5000);
			slot.GetComponent<ShisyaSelect> ().moneyNo = requriedMoney;

		} else {

			//kahou
			Kahou kahou = new Kahou ();
			List<string> kahouCdId = new List<string> ();
			kahouCdId = kahou.getMyRandomKahouCdId ();
			if (kahouCdId.Count !=0) {
				//kahou
				slot.GetComponent<ShisyaSelect>().itemCd = kahouCdId[0];
				slot.GetComponent<ShisyaSelect>().itemId = kahouCdId[1];
				slot.GetComponent<ShisyaSelect>().itemDataCd = kahouCdId[2];

			} else {
				//money
				int requriedMoney = UnityEngine.Random.Range (1000, 5000);
				slot.GetComponent<ShisyaSelect> ().moneyNo = requriedMoney;

			}



		}
	}

	public void randomSalesKahouView(GameObject slot){

		int requriedMoney = 0;

		//kahou
		Kahou kahou = new Kahou ();

		//Type
		List<string> kahouTypeList = new List<string>{"bugu","kabuto","gusoku","meiba","cyadougu","heihousyo","chishikisyo"};
		int rdmTyp = UnityEngine.Random.Range (0, kahouTypeList.Count);
		string kahouType = kahouTypeList [rdmTyp];
			
		//Rank
		float rankPercent = UnityEngine.Random.value;
		rankPercent = rankPercent * 100;
		string kahouRank = "";
		if (rankPercent <= 5) {
			//S
			kahouRank = "S";
		} else if (5 < rankPercent && rankPercent <= 20) {
			//A
			kahouRank = "A";
		}else if (20 < rankPercent && rankPercent <= 50) {
			//B
			kahouRank = "B";
		}else if (50 < rankPercent) {
			//C
			kahouRank = "C";
		}

		int kahouId = kahou.getRamdomKahouId(kahouType, kahouRank);
		KahouStatusGet kahouSts = new KahouStatusGet ();
		List<string> kahouInfoList = new List<string> ();
		kahouInfoList = kahouSts.getKahouInfo(kahouType, kahouId);
		requriedMoney = int.Parse(kahouInfoList [5]);

		//discount
		List<float> discountList = new List<float>{1.5f,1.4f,1.3f,1.2f,1.1f,1.0f,0.9f,0.8f,0.7f,0.6f,0.5f};
		int rdmDisc = UnityEngine.Random.Range (0, discountList.Count);
		float discountPercent = discountList [rdmDisc];
		requriedMoney = Mathf.CeilToInt((float)requriedMoney * discountPercent);


		//kahou
		slot.GetComponent<ShisyaSelect>().itemCd = kahouType;
		slot.GetComponent<ShisyaSelect> ().itemId = kahouId.ToString();
		slot.GetComponent<ShisyaSelect> ().itemName = kahouInfoList [0];
		slot.GetComponent<ShisyaSelect> ().moneyNo = requriedMoney;

	}

	public void viewCurrentValue(){
		int money = PlayerPrefs.GetInt ("money");
		int hyourou = PlayerPrefs.GetInt ("hyourou");
		int busyoDama = PlayerPrefs.GetInt ("busyoDama");

		GameObject.Find ("Panel").transform.FindChild ("Money").transform.FindChild ("Value").GetComponent<Text> ().text = money.ToString();
		GameObject.Find ("Panel").transform.FindChild ("Hyourou").transform.FindChild ("Value").GetComponent<Text> ().text = hyourou.ToString();
		GameObject.Find ("Panel").transform.FindChild ("BusyoDama").transform.FindChild ("Value").GetComponent<Text> ().text = busyoDama.ToString();

		
	}

}
