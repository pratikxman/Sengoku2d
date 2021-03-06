﻿using UnityEngine;
using System.Collections;

public class GeneralFuncController : MonoBehaviour {

	private bool scrollStartFlg = false; // スクロールが始まったかのフラグ
	private Vector2 scrollStartPos  =   new Vector2(); // スクロールの起点となるタッチポジション
	private static float SCROLL_END_LEFT = -15f; // 左側への移動制限(これ以上左には進まない)
	private static float SCROLL_END_RIGHT = 15f; // 右側への移動制限(これ以上右には進まない)
	private static float SCROLL_DISTANCE_CORRECTION = 0.1f; // スクロール距離の調整
	
	private Vector2 touchPosition   =   new Vector2(); // タッチポジション初期化
	private Collider2D collide2dObj =   null; // タッチ位置にあるオブジェクトの初期化
	

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			
			touchPosition   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			collide2dObj = Physics2D.OverlapPoint(touchPosition);
			
			if (scrollStartFlg == false && collide2dObj) {
				GameObject obj = collide2dObj.transform.gameObject;				
			}else{
				scrollStartFlg = true;
				if(scrollStartPos.x == 0.0f){
					scrollStartPos   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				}else{
					Vector2 touchMovePos    =   touchPosition;
					if(scrollStartPos.x != touchMovePos.x){
						// 直前のタッチ位置との差を取得する
						float diffPos   =   SCROLL_DISTANCE_CORRECTION * (touchMovePos.x - scrollStartPos.x);
						
						Vector2 pos = this.transform.position;
						pos.x -= diffPos;
						// スクロールが制限を超過する場合、処理を止める
						if(pos.x > SCROLL_END_RIGHT || pos.x < SCROLL_END_LEFT){
							return;
						}
						this.transform.position = pos;
						scrollStartPos = touchMovePos;
					}
				}
			}
		}else{
			// タッチを離したらフラグを落とし、スクロール開始位置も初期化する 
			scrollStartFlg  =   false;
			scrollStartPos  =   new Vector2();
		}
	}
}