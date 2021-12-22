using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;

public static class LoginManager  //�Q�[�����s���ɃC���X�^���X�������I��1�������������
{
    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    static LoginManager()
    {
        //TitleID�ݒ�
        PlayFabSettings.staticSettings.TitleId = "PlayFab�ō쐬�����^�C�g����ID�������B�p����5�����̂���";

        Debug.Log("TitleID�ݒ�F" + PlayFabSettings.staticSettings.TitleId);

    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <returns></returns>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static async UniTaskVoid InitializeAsync()
    {
        Debug.Log("�������J�n");

        //PlayFab�ւ̃��O�C�������ƃ��O�C��
        await PrepareLoginPlayFab();

        Debug.Log("����������");
    }

    public static async UniTask PrepareLoginPlayFab()
    {
        Debug.Log("���O�C���@�����@�J�n");

        //���̃��O�C���̏��(���N�G�X�g�j���쐬���Đݒ�
        var request = new LoginWithCustomIDRequest
        {
            CustomId = "GettingStartedGuide",       //���̕��������[�U�[��ID�ɂȂ�܂�
            CreateAccount = true�@�@�@�@�@�@�@�@�@�@//�A�J�E���g���쐬����Ă��Ȃ��ꍇ�Atrue�̏ꍇ�͓������O�C�����ăA�J�E���g���쐬����
        };

        //PlayFab�ւ̃��O�C���B��񂪊m�F�ł���܂őҋ@
        var result = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

        //�G���[�̓��e�����āA���O�C���ɐ������Ă��邩�𔻒�(�G���[�n���h�����O�j
        var message = result.Error is null
            ? $"���O�C�������I My PlayFabID is { result.Result.PlayFabId}"
            : result.Error.GenerateErrorReport();

        Debug.Log(message);
            
    }

}
