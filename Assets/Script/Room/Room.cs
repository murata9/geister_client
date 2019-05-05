using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Protocol;

public class Room : MonoBehaviour {
    [SerializeField]
    Text m_ownerName;

    void Start()
    {
        if (m_ownerName == null)
        {
            Debug.LogError("m_ownerName is null");
        }
    }

    public int RoomID { private set; get; }

    public void setParam(RoomInfo param)
    {
        m_ownerName.text = param.owner_name;
        RoomID = param.room_id;
        // TODO:status
    }
}
