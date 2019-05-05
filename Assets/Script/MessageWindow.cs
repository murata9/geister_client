using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageWindow : MonoBehaviour {

	[SerializeField] Text message;

	public void SetMessage(string str){
		message.text = str;
	}

	public void ClickConfirmButton(){
		Destroy (this.gameObject);
	}
}
