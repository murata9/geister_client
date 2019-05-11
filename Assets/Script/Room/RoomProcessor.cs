using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HTTP;
using Protocol;

public class RoomProcessor : MonoBehaviour {
    [SerializeField]
    RectTransform m_roomPrefab;
    [SerializeField]
    GameObject m_roomParentObj;

    bool m_stopFlag = false;

    void Start()
    {
        if (m_roomPrefab == null)
        {
            Debug.LogError("m_roomPrefab is null");
        }
        if (m_roomParentObj == null)
        {
            Debug.LogError("m_roomParentObj is null");
        }
        ApiClient.Instance.ResponseListRooms = onResponseRoomList;
        StartCoroutine(Process());
    }

    public void Stop()
    {
        m_stopFlag = true;
        StopAllCoroutines();
    }

    void onResponseRoomList(ResponseListRooms param)
    {
        Debug.Log("Rooms");
        if (m_stopFlag)
        {
            return;
        }
        // 最初にルーム一覧を空にする
        foreach (Transform r in m_roomParentObj.transform)
        {
            Destroy(r.gameObject);
        }

        foreach (var room in param.rooms)
        {
            Debug.Log("    " + room.owner_name);
            var r = GameObject.Instantiate(m_roomPrefab) as RectTransform;
            r.SetParent(m_roomParentObj.transform);
            r.localScale = Vector3.one;
            r.GetComponent<Room>().setParam(room);
        }
    }

    void RequestRoomList()
    {
        var param = new RequestListRooms(); // empty param
        ApiClient.Instance.RequestListRooms(param);
    }

    IEnumerator Process()
    {
        while (true)
        {
            RequestRoomList();
            yield return new WaitForSeconds(3);
        }
    }
}
