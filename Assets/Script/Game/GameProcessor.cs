using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HTTP;
using Protocol;

public class GameProcessor : MonoBehaviour {

    private bool m_stopFlag;

    enum e_State
    {
        room_waiting, // 対戦相手待ち
        prepare, // 初期配置
    }

    void Start()
    {
        ApiClient.Instance.ResponseShowRoom = onResponseShowRoom;
        StartCoroutine(Process());
    }

    public void Stop()
    {
        m_stopFlag = true;
        StopAllCoroutines();
    }

    void onResponseShowRoom(ResponseShowRoom param)
    {
        if (m_stopFlag)
        {
            return;
        }
        // TODO
        Debug.Log("onResponseShowRoom");
        // 対戦相手が来たら初期配置へ
        Debug.Log("" + param.status);
        if (param.status != "waiting")
        {

        }
    }

    void RequestShowRoom()
    {
        var param = new RequestShowRoom();
        param.room_id = DataPool.Instance.roomid;
        ApiClient.Instance.RequestShowRoom(param);
    }

    IEnumerator Process()
    {
        while (true)
        {
            RequestShowRoom();
            yield return new WaitForSeconds(3);
        }
    }
}
