using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_kahou_meiba_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string kahouType;
		public string kahouName;
		public string kahouRank;
		public string kahouExp;
		public string kahouTarget;
		public int kahouEffect;
		public string unit;
		public double kahouBuy;
		public int kahouSell;
		public int kahouRatio;
		public string kahouNameEng;
		public string kahouExpEng;
		public string kahouTargetEng;
	}
}