using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTTP;
using Protocol;

public class EntryRoomButton : MonoBehaviour {
    [SerializeField]
    Room m_ownerRoom;

    void Start()
    {
        if (m_ownerRoom == null)
        {
            Debug.LogError("m_room is null");
        }
    }

    public void onClickEntryRoomButton()
    {
        ApiClient.Instance.ResponseCreatePlayerEntry  = onResponseEntryRoom;
        var param = new RequestCreatePlayerEntry();
        param.room_id = m_ownerRoom.RoomID;
        ApiClient.Instance.RequestCreatePlayerEntry(param);
    }

    public void onResponseEntryRoom(ResponseCreatePlayerEntry param)
    {
        Debug.Log("onResponseEntryRoom");
    }
}
