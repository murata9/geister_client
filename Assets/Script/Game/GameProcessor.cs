using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HTTP;
using Protocol;

public class GameProcessor : MonoBehaviour {
    [SerializeField]
    PrepareButton prepareButton;

    private bool m_stopFlag;
    private e_State m_state;

    enum e_State
    {
        room_waiting, // 対戦相手待ち
        preparing, // 初期配置中
        prepare_waiting, // 相手の初期配置待ち
    }

    void Start()
    {
        if (prepareButton == null) Debug.LogError("prepareButton is null");
        ApiClient.Instance.ResponseShowRoom = onResponseShowRoom;
        ApiClient.Instance.ResponseShowGame = onResponseShowGame;
        StartCoroutine(Process());
    }

    public void Stop()
    {
        m_stopFlag = true;
        StopAllCoroutines();
    }

    void onResponseShowRoom(ResponseShowRoom param)
    {
        if (m_stopFlag) return;
        // TODO ログを消す
        Debug.Log("onResponseShowRoom");
        Debug.Log("" + param.status);
        // 対戦相手が来たら初期配置へ
        if (param.status != "waiting" && m_state == e_State.room_waiting)
        {
            DataPool.Instance.gameid = param.game_id;
            m_state = e_State.preparing;
            // 初期配置には先手後手情報が必要なので、ゲーム情報を一度リクエストする
            RequestShowGame();
        }
    }

    void onResponseShowGame(ResponseShowGame param)
    {
        if (m_stopFlag) return;
        // TODO
        Debug.Log("onResponseShowGame");
        Debug.Log("" + param.status);
        if (m_state == e_State.preparing)
        {
            // 初回受信
            DataPool.Instance.first_mover_user_id = param.first_mover_user_id;
            // 確定ボタンを表示
            prepareButton.gameObject.SetActive(true);
        }
    }

    void RequestShowRoom()
    {
        var param = new RequestShowRoom();
        param.room_id = DataPool.Instance.roomid;
        ApiClient.Instance.RequestShowRoom(param);
    }

    void RequestShowGame()
    {
        var param = new RequestShowGame();
        param.game_id = DataPool.Instance.gameid;
        ApiClient.Instance.RequestShowGame(param);
    }

    IEnumerator Process()
    {
        while (true)
        {
            switch (m_state)
            {
                case e_State.room_waiting:
                    RequestShowRoom();
                    break;
                case e_State.preparing:
                    // 何もしない
                    break;
                case e_State.prepare_waiting:
                    RequestShowGame();
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(3);
        }
    }
}
