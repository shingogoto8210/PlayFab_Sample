using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;

public class UserDataManager : MonoBehaviour
{
    //TODO Level�Ȃǂ̏�����������

    /// <summary>
    /// �v���C���[�f�[�^���̍쐬�ƍX�V�i�v���C���[�f�[�^(�^�C�g���j��Key��1�����l��o�^������@
    /// </summary>
    /// <param name="updateUserData"></param>
    /// <param name="userDataPermission"></param>
    /// <returns></returns>
    public static async UniTask UpdatePlayerDataAsync(Dictionary<string,string> updateUserData, UserDataPermission userDataPermission = UserDataPermission.Private)
    {
        var request = new UpdateUserDataRequest
        {
            Data = updateUserData,

            //�A�N�Z�X���̍X�V
            Permission = userDataPermission
        };

        var response = await PlayFabClientAPI.UpdateUserDataAsync(request);

        if(response.Error != null)
        {
            Debug.Log("�G���[");
            return;
        }

        Debug.Log("�v���C���[�f�[�^�@�X�V");
    }

    /// <summary>
    /// �v���C���[�f�[�^����w�肵��Key�̏��̍폜
    /// </summary>
    /// <param name="deleteKey"></param>
    public static async void DeletePlayerDataAsync(string deleteKey)
    {

        var request = new UpdateUserDataRequest
        {
            KeysToRemove = new List<string> { deleteKey }
        };

        var response = await PlayFabClientAPI.UpdateUserDataAsync(request);

        if(response.Error != null)
        {
            Debug.Log("�G���[");
            return;
        }

        Debug.Log("�v���C���[�f�[�^�@�폜");
    }
}
