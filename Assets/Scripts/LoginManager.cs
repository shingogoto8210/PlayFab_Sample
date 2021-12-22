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
        PlayFabSettings.staticSettings.TitleId = "PlayFabで作成したタイトルのIDを書く。英数字5文字のもの";

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

    public static async UniTask PrepareLoginPlayFab()
    {
        Debug.Log("ログイン　準備　開始");

        //仮のログインの情報(リクエスト）を作成して設定
        var request = new LoginWithCustomIDRequest
        {
            CustomId = "GettingStartedGuide",       //この部分がユーザーのIDになります
            CreateAccount = true　　　　　　　　　　//アカウントが作成されていない場合、trueの場合は匿名ログインしてアカウントを作成する
        };

        //PlayFabへのログイン。情報が確認できるまで待機
        var result = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

        //エラーの内容を見て、ログインに成功しているかを判定(エラーハンドリング）
        var message = result.Error is null
            ? $"ログイン成功！ My PlayFabID is { result.Result.PlayFabId}"
            : result.Error.GenerateErrorReport();

        Debug.Log(message);
            
    }

}
