using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class Login : MonoBehaviour {
    [SerializeField]
    InputField m_username;

    [SerializeField]
    InputField m_password;

    public void Start()
    {
        if (m_username == null)
        {
            Debug.LogError("m_username is null");
        }
        if (m_password == null)
        {
            Debug.LogError("m_password is null");
        }
    }

    void onResponseLogin(ResponseCreateUserSession param)
    {
        Debug.Log("onResponseLogin");
        DataPool.Instance.userid = param.user_id;
        DataPool.Instance.sessionid = param.user_session_id;
        ApiClient.Instance.SetAccessToken(param.access_token);

        // ルームへ
        SceneManager.LoadScene("Room");
    }

    public void onClickLoginBotton()
    {
        ApiClient.Instance.ResponseCreateUserSession = onResponseLogin;
        var param = new RequestCreateUserSession();
        param.name = m_username.text;
        param.password = m_password.text;
        ApiClient.Instance.RequestCreateUserSession(param);
    }

    void onResponseSignup(ResponseCreateUser param)
    {
        Debug.Log("onResponseSignup");
        // 新規登録成功後、自動でログインする
        onClickLoginBotton();
    }

    public void onClickSignUp()
    {
        ApiClient.Instance.ResponseCreateUser = onResponseSignup;
        var param = new RequestCreateUser();
        param.name = m_username.text;
        param.password = m_password.text;
        ApiClient.Instance.RequestCreateUser(param);
    }
}
