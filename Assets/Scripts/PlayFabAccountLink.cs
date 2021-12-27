
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Cysharp.Threading.Tasks;


public class PlayFabAccountLink
{
    /// <summary>
    /// ユーザーIDとEmailとパスワードを使ってアカウントの連携を行う
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static async UniTask<bool> SetEmailAndPasswordAsync(string email, string password)
    {
        var request = new AddUsernamePasswordRequest
        {
            Username = PlayerPrefsManager.UserId,
            Email = email,
            Password = password
        };

        //PlayFabにリクエストを送信して、アカウントの連携を判定してもらう
        //処理が終わるまで次の処理には行かず、判定結果が取得出来たらresponseに情報が入るので、それから次の処理へ行く
        var response = await PlayFabClientAPI.AddUsernamePasswordAsync(request);

        //エラーの内容に応じた例外処理を記述する
        if (response.Error != null)
        {
            switch (response.Error.Error)
            {
                case PlayFabErrorCode.InvalidParams:
                    Debug.Log("有効なメールアドレスと、６～100文字以内のパスワードを入力し直して下さい");
                    break;
                case PlayFabErrorCode.EmailAddressNotAvailable:
                    Debug.Log("このメールアドレスはすでに使用されています。");
                    break;
                case PlayFabErrorCode.InvalidEmailAddress:
                    Debug.Log("このメールアドレスは使用できません。");
                    break;
                case PlayFabErrorCode.InvalidPassword:
                    Debug.Log("このパスワードは無効です。");
                    break;
                default:
                    Debug.Log(response.Error.GenerateErrorReport());
                    break;
            }

            return false;

        }
        else
        {
            Debug.Log("Emailとパスワードの登録完了");

            return true;

        }
    }
}
