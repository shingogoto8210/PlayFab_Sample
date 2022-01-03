using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
public static class PlayerPlofileManager
{
    /// <summary>
    /// PlayerProfileModelクラスのプロパティ
    /// </summary>
    public static PlayerProfileModel Profile { get; set; }

    public static string PlayFab => Profile.PlayerId;   　　　　 //PlayerProfileModelクラス内に用意されているPlayerId変数を利用する

    public static string UserDisplayName => Profile.DisplayName; //PlayerProfileModelクラス内に用意されているDisplayName変数を利用する

    /// <summary>
    /// ユーザー名の更新
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static async UniTask<(bool isSuccess, string errorMessage)> UpdateUserDisplayNameAsync(string name)
    {
        //新しいユーザー名の作成
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };

        //PlayFabへ更新命令
        var response = await PlayFabClientAPI.UpdateUserTitleDisplayNameAsync(request);

        //エラーハンドリング(ドキュメントを参考にしてつくる）
        //https://docs.microsoft.com/en-us/rest/api/playfab/client/account-management/updateusertitledisplayname?view=playfab-rest
        if(response.Error != null)
        {
            switch (response.Error.Error)
            {
                case PlayFabErrorCode.InvalidParams:
                    return (false, "ユーザーの名前は3〜25文字以内で入力してください。");

                case PlayFabErrorCode.ProfaneDisplayName:
                case PlayFabErrorCode.NameNotAvailable:
                    return (false, "この名前は使用できません。");

                default:
                    return (false, "想定外のエラー");
            }
        }

        //ローカルのデータを更新
        Profile.DisplayName = name;

        //エラーなしで更新完了
        return (true, "ユーザー名を更新しました。：新しいユーザー名" + response.Result.DisplayName);
    }

    /// <summary>
    /// PlayFabからClientへデータを同期
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="statisticValues"></param>
    public static void SyncPlayFabToClient(PlayerProfileModel profile, List<StatisticValue> statisticValues)
    {

        //初回ログイン時はnullのためnewしておく
        Profile = profile ?? new PlayerProfileModel();

        //TODO statistic を追加する
    }

    //TODO 統計情報、レベル、キャラの更新処理の追加
}
