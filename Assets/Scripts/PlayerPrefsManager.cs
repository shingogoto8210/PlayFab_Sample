
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

    //���[���A�h���X�𗘗p���ă��O�C���ς݂̏ꍇ��true
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
