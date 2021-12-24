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
        PlayFabSettings.staticSettings.TitleId = "DC018";

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

    /// <summary>
    /// PlayerFab�ւ̃��O�C�������ƃ��O�C��
    /// </summary>
    /// <returns></returns>
    public static async UniTask PrepareLoginPlayFab()
    {
        Debug.Log("���O�C���@�����@�J�n");

        await LoginAndUpdateLocalCacheAsync();

        //���̃��O�C���̏��(���N�G�X�g�j���쐬���Đݒ�
        //var request = new LoginWithCustomIDRequest
        //{
        //    CustomId = "GettingStartedGuide",       //���̕��������[�U�[��ID�ɂȂ�܂�
        //    CreateAccount = true�@�@�@�@�@�@�@�@�@�@//�A�J�E���g���쐬����Ă��Ȃ��ꍇ�Atrue�̏ꍇ�͓������O�C�����ăA�J�E���g���쐬����
        //};

        ////PlayFab�ւ̃��O�C���B��񂪊m�F�ł���܂őҋ@
        //var result = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

        ////�G���[�̓��e�����āA���O�C���ɐ������Ă��邩�𔻒�(�G���[�n���h�����O�j
        //var message = result.Error is null
        //    ? $"���O�C�������I My PlayFabID is { result.Result.PlayFabId}"
        //    : result.Error.GenerateErrorReport();

        //Debug.Log(message);
            
    }

    /// <summary>
    /// ���[�U�[�f�[�^�ƃ^�C�g���f�[�^��������
    /// </summary>
    /// <returns></returns>
    public static async UniTask LoginAndUpdateLocalCacheAsync()
    {
        Debug.Log("�������J�n");

        //���[�U�[ID�̎擾�����݂�
        var userId = PlayerPrefsManager.UserId; //var�̌^��string�^

        //���[�U�[ID���擾�ł��Ȃ��ꍇ�ɂ͐V�K�쐬���ē������O�C������
        //�擾�o�����ꍇ�ɂ́C���[�U�[ID���g���ă��O�C������i����̎菇�Ŏ����j
        //var�̌^��LoginResult�^�iPlayFab�@SDK�ŗp�ӂ���Ă���N���X�j
        var loginResult = string.IsNullOrEmpty(userId)
            ? await CreateNewUserAsync()
            : await LoadUserAsync(userId);

        //TODO�@�f�[�^�������Ŏ擾����ݒ�ɂ��Ă���̂�,�擾�����f�[�^�����[�J���ɃL���b�V������

    }

    /// <summary>
    /// �V�K���[�U�[���쐬����UserId��PlayerPrefs�ɕۑ�
    /// </summary>
    /// <returns></returns>
    private static async UniTask<LoginResult> CreateNewUserAsync()
    {
        Debug.Log("���[�U�[�f�[�^�Ȃ��B�V�K���[�U�[�쐬");

        while (true)
        {
            var newUserId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

            //UserId�̍̔�
            var request = new LoginWithCustomIDRequest
            {
                CustomId = newUserId,
                CreateAccount = true,
            };

            //PlayFab�Ƀ��O�C��
            var response = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

            //�G���[�n���h�����O
            if(response.Error != null)
            {
                Debug.Log("Error");
            }

            //������LastLoginTime�ɒl�������Ă���ꍇ�ɂ́C�̔Ԃ���ID���������[�U�[�Əd�����Ă���̂Ń��g���C����
            if (response.Result.LastLoginTime.HasValue)
            {
                continue;
            }

            //PlayerPrefs��UserId���L�^����
            PlayerPrefsManager.UserId = newUserId;

            return response.Result;
        }

        
    }

    /// <summary>
    /// ���O�C�����Ă��郆�[�U�[�f�[�^�����[�h
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private static async UniTask<LoginResult> LoadUserAsync(string userId)
    {
        Debug.Log("���[�U�[�f�[�^����C���O�C���J�n");

        //���O�C�����N�G�X�g�̍쐬
        var request = new LoginWithCustomIDRequest
        {
            CustomId = userId,
            CreateAccount = false   //�A�J�E���g�̏㏑���͍s��Ȃ��悤�ɂ���
        };

        //PlayFab�Ƀ��O�C��
        var response = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

        //�G���[�n���h�����O
        if(response.Error != null)
        {
            Debug.Log("Error");

            //TODO response.Error�ɂ̓G���[�̎�ނ��l�Ƃ��ē����Ă���
            //���̃G���[�ɑΉ�����������switch���ȂǂŋL�q���ĕ����̃G���[�ɑΉ��ł���悤�ɂ���

        }

        //�G���[�̓��e�����ăn���h�����O���s���C���O�C���ɐ������Ă��邩�𔻒�
        var message = response.Error is null ? $"Login success! My PlayFabID is {response.Result.PlayFabId}" : response.Error.GenerateErrorReport();

        Debug.Log(message);

        return response.Result;
    }

}
