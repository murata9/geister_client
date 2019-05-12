using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HTTP;
using Protocol;

public class GameProcessor : MonoBehaviour {
    [SerializeField]
    PrepareButton prepareButton;

    [SerializeField]
    Board board;

    private bool m_stopFlag;
    public e_State m_state { get; private set; }

    public enum e_State
    {
        room_waiting, // 対戦相手待ち
        preparing, // 初期配置中
        prepare_waiting, // 相手の初期配置待ち
        playing, // 対戦中
    }

    void Start()
    {
        if (prepareButton == null) Debug.LogError("prepareButton is null");
        if (board == null) Debug.LogError("board is null");
        ApiClient.Instance.ResponseShowRoom = onResponseShowRoom;
        ApiClient.Instance.ResponseShowGame = onResponseShowGame;
        ApiClient.Instance.ResponseListPieces = onResponseListPieces;
        StartCoroutine(Process());
    }

    public void Stop()
    {
        m_stopFlag = true;
        StopAllCoroutines();
    }

    void RequestShowRoom()
    {
        var param = new RequestShowRoom();
        param.room_id = DataPool.Instance.roomid;
        ApiClient.Instance.RequestShowRoom(param);
    }

    void onResponseShowRoom(ResponseShowRoom param)
    {
        if (m_stopFlag) return;
        // 対戦相手が来たら初期配置へ
        if (param.status != "waiting" && m_state == e_State.room_waiting)
        {
            DataPool.Instance.gameid = param.game_id;
            m_state = e_State.preparing;
            // 初期配置には先手後手情報が必要なので、ゲーム情報を一度リクエストする
            RequestShowGame();
        }
    }

    void RequestShowGame()
    {
        var param = new RequestShowGame();
        param.game_id = DataPool.Instance.gameid;
        ApiClient.Instance.RequestShowGame(param);
    }

    void onResponseShowGame(ResponseShowGame param)
    {
        if (m_stopFlag) return;
        // TODO
        Debug.Log("onResponseShowGame: " + param.status);
   
        switch(param.status)
        {
            case "preparing":
                if (m_state == e_State.preparing)
                {
                    // 初回受信
                    DataPool.Instance.first_mover_user_id = param.first_mover_user_id;
                    // 確定ボタンを表示
                    prepareButton.gameObject.SetActive(true);
                }
                break;
            case "playing":
                if (m_state == e_State.prepare_waiting)
                {
                    m_state = e_State.playing;
                }
                if (param.turn_count != DataPool.Instance.last_turn_count)
                {
                    DataPool.Instance.turn_mover_user_id = param.turn_mover_user_id;
                    RequestListPieces();
                }
                break;
            case "finished":
                break;
            case "exited":
                break;
            default:
                Debug.LogError("unknown status:" + param.status);
                break;
        }
    }

    void RequestListPieces()
    {
        var param = new RequestListPieces();
        param.game_id = DataPool.Instance.gameid;
        ApiClient.Instance.RequestListPieces(param);
    }

    void onResponseListPieces(ResponseListPieces param)
    {
        board.recvPieceLists(param);
    }

    public void onEndPreparing()
    {
        if (m_state != e_State.preparing)
        {
            Debug.LogError("logic error invalid status:" + m_state.ToString());
        }
        m_state = e_State.prepare_waiting;
    }

    public void onPieceMoved()
    {
        RequestShowGame();
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
                case e_State.playing:
                    if (!DataPool.Instance.isMyTurn())
                    {
                        RequestShowGame();
                    }
                    break;
                default:
                    Debug.LogError("status:" + m_state.ToString()+ "に対する操作がない");
                    break;
            }
            yield return new WaitForSeconds(3);
        }
    }
}
