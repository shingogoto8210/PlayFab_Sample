using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;

public class UserDataManager : MonoBehaviour
{
    //TODO Levelなどの情報をもたせる

    /// <summary>
    /// プレイヤーデータ内の作成と更新（プレイヤーデータ(タイトル）のKeyに1つだけ値を登録する方法
    /// </summary>
    /// <param name="updateUserData"></param>
    /// <param name="userDataPermission"></param>
    /// <returns></returns>
    public static async UniTask UpdatePlayerDataAsync(Dictionary<string,string> updateUserData, UserDataPermission userDataPermission = UserDataPermission.Private)
    {
        var request = new UpdateUserDataRequest
        {
            Data = updateUserData,

            //アクセス許可の更新
            Permission = userDataPermission
        };

        var response = await PlayFabClientAPI.UpdateUserDataAsync(request);

        if(response.Error != null)
        {
            Debug.Log("エラー");
            return;
        }

        Debug.Log("プレイヤーデータ　更新");
    }

    /// <summary>
    /// プレイヤーデータから指定したKeyの情報の削除
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
            Debug.Log("エラー");
            return;
        }

        Debug.Log("プレイヤーデータ　削除");
    }
}
