using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class ExitButton : MonoBehaviour {

    [SerializeField]
    RoomProcessor m_roomProcessor;

    private void Start()
    {
        if (m_roomProcessor == null)
        {
            Debug.LogError("m_roomProcessor is null");
        }
    }

    public void onClickExitButton()
    {
        DeleteUserSessionRequest();
        SceneManager.LoadScene("Title");
    }

    private void DeleteUserSessionRequest()
    {
        ApiClient.Instance.ResponseDeleteUserSession = (p) => { }; // 何もしないコールバックを登録
        var param = new RequestDeleteUserSession();
        param.user_session_id = DataPool.Instance.sessionid;
        ApiClient.Instance.RequestDeleteUserSession(param);
        // セッションの期限切れで進行不能になることを防ぐため
        // 成否にかかわらずタイトルに戻る
        // 本来ならレスポンスは待ちたいが、成功時しかレスポンスを受け取れないため
        m_roomProcessor.Stop();

        DataPool.Instance.Reset();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        DeleteUserSessionRequest();
    }
}
