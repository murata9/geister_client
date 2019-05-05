using UnityEngine;
using System.Collections;

public class DebugCanvas : SingletonMonoBehaviour<DebugCanvas>{

	protected override void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}
}
