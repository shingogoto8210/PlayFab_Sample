
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class AccountCanvas : MonoBehaviour
{
    [SerializeField]
    private Button btnSubmit, btnCancel;

    [SerializeField]
    private InputField emailInput, passwordInput;

    [SerializeField]
    private Text txtEmail, txtPassword;

    [SerializeField]
    private GameObject responsePopUp;

    [SerializeField]
    private Button btnClosePopUp;

    [SerializeField]
    private Button btnEmailLogin;

    [SerializeField]
    private Text txtResponseInfo;

    private (string email, string password) inputValue;  //Email�ƃp�X���[�h�o�^�p


    private void Start()
    {
        responsePopUp.SetActive(false);

        //�{�^���̓o�^
        btnSubmit?.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => OnClickSubmit());

        btnCancel?.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => OnClickCancel()) ;

        btnClosePopUp?.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => OnClickCloseCompletePopUp());

        btnEmailLogin?.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => OnClickEmailLogin());

        //InputField
        emailInput?.OnEndEditAsObservable()
            .Subscribe(x => UpdateDisplayEmail(x));

        passwordInput?.OnEndEditAsObservable()
            .Subscribe(x => UpdateDisplayPassword(x));
    }

    /// <summary>
    /// Email�̒l�ƕ\���̋@�\
    /// </summary>
    /// <param name="newPassword"></param>
    private void UpdateDisplayEmail(string newEmail)
    {
        txtEmail.text = newEmail;
        inputValue.email = newEmail;
        Debug.Log(inputValue);
    }

    /// <summary>
    /// �p�X���[�h�̒l�ƕ\���̋@�\
    /// </summary>
    /// <param name="newPassword"></param>
    private void UpdateDisplayPassword(string newPassword)
    {
        txtPassword.text = newPassword;
        inputValue.password = newPassword;
        Debug.Log(inputValue);
    }

    private async void OnClickSubmit()
    {
        Debug.Log("OK �A�J�E���g�A�g�̏��F�J�n");

        //Email�ƃp�X���[�h�𗘗p���āA���[�U�[�A�J�E���g�̘A�g�����݂�
        bool isLink = await PlayFabAccountLink.SetEmailAndPasswordAsync(inputValue.email, inputValue.password);

        if (isLink)
        {
            Debug.Log("�A�g����");

            txtResponseInfo.text = "�A�J�E���g�A�g���������܂����B";
            responsePopUp.SetActive(true);

        }
        else
        {
            Debug.Log("�A�J�E���g�A�g�����s���܂����B");

            txtResponseInfo.text = "�A�J�E���g�A�g�����s���܂����B";
            responsePopUp.SetActive(true);
        }
    }

    /// <summary>
    /// NG�{�^�������������ۂ̏���
    /// </summary>
    private void OnClickCancel()
    {
        this.gameObject.SetActive(false);

        Debug.Log("NG");
    }

    /// <summary>
    /// CompletePopUp���^�b�v�����ۂ̏���
    /// </summary>
    private void OnClickCloseCompletePopUp()
    {
        responsePopUp.SetActive(false);

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Email�Ń��O�C���{�^�������������ۂ̏���
    /// </summary>
    private async void OnClickEmailLogin()
    {
        //Email�Ń��O�C�������݂�@isLogin = true �Ȃ烍�O�C������
        //TODO ���̎菇��LoginManager�ɊY�����郁�\�b�h���쐬����̂ŁA����܂ŃR�����g�A�E�g���Ă���
        //(bool isLogin, string log)response = await LoginManager.LoginEmailAndPasswordAsync(inputValue.email,inputValue.password);
        
        //Debug.Log(response.log);

        //txtResponseInfo.text = responsePopUp.log;
        responsePopUp.SetActive(true);
    }

}
