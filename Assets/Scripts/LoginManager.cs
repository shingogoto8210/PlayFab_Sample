using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;

public static class LoginManager  //ゲーム実行時にインスタンスが自動的に1つだけ生成される
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    static LoginManager()
    {
        //TitleID設定
        PlayFabSettings.staticSettings.TitleId = "DC018";

        Debug.Log("TitleID設定：" + PlayFabSettings.staticSettings.TitleId);

    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static async UniTaskVoid InitializeAsync()
    {
        Debug.Log("初期化開始");

        //PlayFabへのログイン準備とログイン
        await PrepareLoginPlayFab();

        Debug.Log("初期化完了");
    }

    /// <summary>
    /// PlayerFabへのログイン準備とログイン
    /// </summary>
    /// <returns></returns>
    public static async UniTask PrepareLoginPlayFab()
    {
        Debug.Log("ログイン　準備　開始");

        await LoginAndUpdateLocalCacheAsync();

        //仮のログインの情報(リクエスト）を作成して設定
        //var request = new LoginWithCustomIDRequest
        //{
        //    CustomId = "GettingStartedGuide",       //この部分がユーザーのIDになります
        //    CreateAccount = true　　　　　　　　　　//アカウントが作成されていない場合、trueの場合は匿名ログインしてアカウントを作成する
        //};

        ////PlayFabへのログイン。情報が確認できるまで待機
        //var result = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

        ////エラーの内容を見て、ログインに成功しているかを判定(エラーハンドリング）
        //var message = result.Error is null
        //    ? $"ログイン成功！ My PlayFabID is { result.Result.PlayFabId}"
        //    : result.Error.GenerateErrorReport();

        //Debug.Log(message);
            
    }

    /// <summary>
    /// ユーザーデータとタイトルデータを初期化
    /// </summary>
    /// <returns></returns>
    public static async UniTask LoginAndUpdateLocalCacheAsync()
    {
        Debug.Log("初期化開始");

        //ユーザーIDの取得を試みる
        var userId = PlayerPrefsManager.UserId; //varの型はstring型

        //ユーザーIDが取得できない場合には新規作成して匿名ログインする
        //取得出来た場合には，ユーザーIDを使ってログインする（次回の手順で実装）
        //varの型はLoginResult型（PlayFab　SDKで用意されているクラス）
        var loginResult = string.IsNullOrEmpty(userId)
            ? await CreateNewUserAsync()
            : await LoadUserAsync(userId);

        //TODO　データを自動で取得する設定にしているので,取得したデータをローカルにキャッシュする

    }

    /// <summary>
    /// 新規ユーザーを作成してUserIdをPlayerPrefsに保存
    /// </summary>
    /// <returns></returns>
    private static async UniTask<LoginResult> CreateNewUserAsync()
    {
        Debug.Log("ユーザーデータなし。新規ユーザー作成");

        while (true)
        {
            var newUserId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

            //UserIdの採番
            var request = new LoginWithCustomIDRequest
            {
                CustomId = newUserId,
                CreateAccount = true,
            };

            //PlayFabにログイン
            var response = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

            //エラーハンドリング
            if(response.Error != null)
            {
                Debug.Log("Error");
            }

            //もしもLastLoginTimeに値が入っている場合には，採番したIDが既存ユーザーと重複しているのでリトライする
            if (response.Result.LastLoginTime.HasValue)
            {
                continue;
            }

            //PlayerPrefsにUserIdを記録する
            PlayerPrefsManager.UserId = newUserId;

            return response.Result;
        }

        
    }

    /// <summary>
    /// ログインしているユーザーデータをロード
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private static async UniTask<LoginResult> LoadUserAsync(string userId)
    {
        Debug.Log("ユーザーデータあり，ログイン開始");

        //ログインリクエストの作成
        var request = new LoginWithCustomIDRequest
        {
            CustomId = userId,
            CreateAccount = false   //アカウントの上書きは行わないようにする
        };

        //PlayFabにログイン
        var response = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

        //エラーハンドリング
        if(response.Error != null)
        {
            Debug.Log("Error");

            //TODO response.Errorにはエラーの種類が値として入っている
            //そのエラーに対応した処理をswitch文などで記述して複数のエラーに対応できるようにする

        }

        //エラーの内容を見てハンドリングを行い，ログインに成功しているかを判定
        var message = response.Error is null ? $"Login success! My PlayFabID is {response.Result.PlayFabId}" : response.Error.GenerateErrorReport();

        Debug.Log(message);

        return response.Result;
    }

}
