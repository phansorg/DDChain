using UnityEngine;
using System.Collections;
using NCMB;
using System.Collections.Generic;

public class UserAuth : MonoBehaviour
{

    private string currentPlayerName = string.Empty;
    private UserAuth instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            string name = gameObject.name;
            gameObject.name = name + "(Singleton)";

            GameObject duplicater = GameObject.Find(name);
            if (duplicater != null)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.name = name;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // mobile backendに接続してログイン ------------------------

    public void logIn(string id, string pw)
    {

        NCMBUser.LogInAsync(id, pw, (NCMBException e) =>
        {
            // 接続成功したら
            if (e == null)
            {
                currentPlayerName = id;
            }
            // 失敗した場合はサインアップを試みる
            else
            {
                signUp(id, pw);
            }
        });
    }

    // mobile backendに接続して新規会員登録 ------------------------

    public void signUp(string id, string pw)
    {

        NCMBUser user = new NCMBUser();
        user.UserName = id;
        user.Password = pw;
        user.SignUpAsync((NCMBException e) =>
        {

            if (e == null)
            {
                currentPlayerName = id;
            }
        });
    }

    // mobile backendに接続してログアウト ------------------------

    public void logOut()
    {

        NCMBUser.LogOutAsync((NCMBException e) =>
        {
            if (e == null)
            {
                currentPlayerName = string.Empty;
            }
        });
    }

    // 現在のプレイヤー名を返す --------------------
    public string currentPlayer()
    {
        return currentPlayerName;
    }

}