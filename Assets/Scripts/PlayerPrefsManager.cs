
using UnityEngine;

/// <summary>
/// PlayerPrefs�̃w���p�[�N���X
/// </summary>
public static class PlayerPrefsManager
{

    //�v���p�e�B�Ƃ���UserId���쐬����
    public static string UserId
    {
        set
        {
            PlayerPrefs.SetString("UserId", value);
            PlayerPrefs.Save();
        }
        get => PlayerPrefs.GetString("UserId");
    }
}
