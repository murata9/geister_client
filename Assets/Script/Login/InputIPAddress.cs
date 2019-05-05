using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTTP;
using UnityEngine.UI;


public class InputIPAddress : MonoBehaviour {

    [SerializeField]
    InputField m_inputfield;

	// Use this for initialization
	void Start () {
        if (m_inputfield == null)
        {
            Debug.LogError("m_inputfield is null");
            return;
        }
        string address = m_inputfield.text;
        if (address != "")
        {
            onInputedIPAddress(address);
        }
	}

    public void onInputedIPAddress(string address)
    {
        address = "http://" + address;
        ApiClient.Instance.SetIpAddress(address);
    }
}
