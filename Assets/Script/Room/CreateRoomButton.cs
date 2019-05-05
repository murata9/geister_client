using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTTP;
using Protocol;

public class CreateRoomButton : MonoBehaviour {

    public void onClickCreateRoomButton()
    {
        ApiClient.Instance.ResponseCreateRoom = onResponseCreateRoom;
        var param = new RequestCreateRoom(); // empty param
        ApiClient.Instance.RequestCreateRoom(param);
    }

    public void onResponseCreateRoom(ResponseCreateRoom param)
    {
        Debug.Log("onResponseCreateRoom");
    }
}
