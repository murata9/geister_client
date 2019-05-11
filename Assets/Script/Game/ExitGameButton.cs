using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class ExitGameButton : MonoBehaviour
{

    [SerializeField]
    GameProcessor m_gameProcessor;

    private void Start()
    {
        if (m_gameProcessor == null)
        {
            Debug.LogError("m_roomProcessor is null");
        }
    }

    public void onClickExitButton()
    {
        DeletePlayerEntryRequest();
        SceneManager.LoadScene("Room");
    }

    private void DeletePlayerEntryRequest()
    {
        ApiClient.Instance.ResponseDeletePlayerEntry = (p) => { }; // 何もしないコールバックを登録
        var param = new RequestDeletePlayerEntry();
        param.player_entry_id = DataPool.Instance.player_entry_id;
        ApiClient.Instance.RequestDeletePlayerEntry(param);
        m_gameProcessor.Stop();

        DataPool.Instance.onExitGame();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        DeletePlayerEntryRequest();
    }
}
