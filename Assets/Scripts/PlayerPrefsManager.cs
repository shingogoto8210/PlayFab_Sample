
using UnityEngine;

/// <summary>
/// PlayerPrefsのヘルパークラス
/// </summary>
public static class PlayerPrefsManager
{

    //プロパティとしてUserIdを作成する
    public static string UserId
    {
        set
        {
            PlayerPrefs.SetString("UserId", value);
            PlayerPrefs.Save();
        }
        get => PlayerPrefs.GetString("UserId");
    }

    //メールアドレスを利用してログイン済みの場合はtrue
    public static bool IsLoginEmailAdress
    {
        set
        {
            PlayerPrefs.SetString("IsLoginEmailAdress", value.ToString());
            PlayerPrefs.Save();
        }

        get => bool.TryParse(PlayerPrefs.GetString("IsLoginEmailAdress"), out bool result) && result;
    }
}
