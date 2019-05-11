using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        DataPool.Instance.player_entry_id = param.player_entry_id;
        DataPool.Instance.roomid = param.room_id;
        SceneManager.LoadScene("Game");
    }
}
