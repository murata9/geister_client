using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PrintDebugString : SingletonMonoBehaviour<PrintDebugString> {

	public class DebugString{
		public string str;
		public float lifeTime;

		public DebugString(string _str,float _lifeTime){
			str = _str;
			lifeTime = _lifeTime;
		}
	}

	// LifeTime,DebugString
	List<DebugString> debugStringList = new List<DebugString>();

	Text debugStringComponent;

	override protected void Awake(){
		debugStringComponent = GetComponent<Text> ();
	}

	static readonly float lifeTime = 5.0f;

	public void Add(string str){
		debugStringList.Add (new DebugString(str+"\n",lifeTime));
		Debug.LogError (str);
	}

	// Update is called once per frame
	void Update () {

		debugStringComponent.text = string.Empty;

		if (debugStringList.Count == 0)
			return;

		debugStringList.OrderBy (x => x.lifeTime);

		List<DebugString> eraseList = new List<DebugString> ();
		foreach (var e in debugStringList) {
			e.lifeTime -= Time.deltaTime;

			if (e.lifeTime < 0) {
				eraseList.Add (e);
			} else {
				debugStringComponent.text += e.str;
			}
		}

		foreach (var e in eraseList) {
			debugStringList.Remove (e);
		}
	}
}
