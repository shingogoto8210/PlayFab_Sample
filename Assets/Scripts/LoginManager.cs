using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public static class LoginManager  //ゲーム実行時にインスタンスが自動的に1つだけ生成される
{

    public static GetPlayerCombinedInfoRequestParams CombinedInfoRequestParams { get; }
    = new GetPlayerCombinedInfoRequestParams
    {
        GetUserAccountInfo = true,
        GetPlayerProfile = true,
        GetTitleData = true,
        GetUserData = true,
        GetUserInventory = true,
        GetUserVirtualCurrency = true,
        GetPlayerStatistics = true
    };
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

        //プレイヤーデータの作成と更新
        await CreateUserDataAsync();

        //データを自動で取得する設定にしているので,取得したデータをローカルにキャッシュする
        UpdateLocalCacheAsync(loginResult);

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
                InfoRequestParameters = CombinedInfoRequestParams
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
            CreateAccount = false,   //アカウントの上書きは行わないようにする
            InfoRequestParameters = CombinedInfoRequestParams //プロパティ情報を設定
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

    /// <summary>
    /// Emailとパスワードでログイン(アカウント回復用）
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static async UniTask<(bool, string)> LoginEmailAndPasswordAsync(string email,string password)
    {
        //Emailによるログインリクエストの作成
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
            InfoRequestParameters = CombinedInfoRequestParams
        };

        //PlayFabにログイン
        var response = await PlayFabClientAPI.LoginWithEmailAddressAsync(request);

        //エラーハンドリング
        if(response.Error != null)
        {
            switch (response.Error.Error)
            {
                case PlayFabErrorCode.InvalidParams:
                case PlayFabErrorCode.InvalidEmailOrPassword:
                case PlayFabErrorCode.AccountNotFound:
                    Debug.Log("メールアドレスかパスワードが正しくありません");
                    break;
                default:
                    Debug.Log(response.Error.GenerateErrorReport());
                    break;
            }
            return (false, "メールアドレスかパスワードが正しくありません");
        }

        //PlayPrefsを初期化して、ログイン結果のUserIdを登録しなおす
        PlayerPrefs.DeleteAll();

        //新しくPlayFabからUserIdを取得
        //InfoResultPayloadはクライアントプロフィールオプション（InfoRequestParameters）で許可されてないとnullになる
        PlayerPrefsManager.UserId = response.Result.InfoResultPayload.AccountInfo.CustomIdInfo.CustomId;


        //Emailでログインしたことを記録する
        PlayerPrefsManager.IsLoginEmailAdress = true;

        return (true, "Emailによるログインが完了しました");
    }

    /// <summary>
    /// PlayFab から取得したデータ群をローカル(端末)にキャッシュ
    /// </summary>
    /// <param name="loginResult"></param>
    /// <returns></returns>

    public static void UpdateLocalCacheAsync(LoginResult loginResult)  //  <=　後程、async を追加し、戻り値を UniTask に変更します。
    {   

        // TODO カタログ類の初期化。他のインスタンスの初期化にも必要なので最初に行う


        // TODO タイトルデータの取得


        // TODO ユーザーデータの取得

        // ユーザー名などの取得
        PlayerPlofileManager.SyncPlayFabToClient(loginResult.InfoResultPayload.PlayerProfile, loginResult.InfoResultPayload.PlayerStatistics);


        // TODO 他の初期化処理を追加


        Debug.Log("各種データのキャッシュ完了");
    }

    private static async UniTask CreateUserDataAsync()
    {
        var createData = new Dictionary<string, string>
        {
            {"Level","0" }
        };

        await UserDataManager.UpdatePlayerDataAsync(createData);

        Debug.Log("ユーザーデータ　登録完了");
    }
}
