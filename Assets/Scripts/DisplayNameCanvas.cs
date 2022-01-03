
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

public class DisplayNameCanvas : MonoBehaviour
{
    [SerializeField]
    private Button btnSubmit, btnCancel;

    [SerializeField]
    private InputField displayNameInput;

    [SerializeField]
    private Text txtDisplayName;

    [SerializeField]
    private GameObject responsePopUp;

    [SerializeField]
    private Button btnClosePopUp;

    [SerializeField]
    private Text txtResponseInfo;

    private string displayName; //���[�U�[���o�^�p

    private void Start()
    {
        responsePopUp.SetActive(false);

        //�{�^���̓o�^�p
        btnSubmit?.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => OnClickSubmit());

        btnCancel?.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => OnClickCancel());

        btnClosePopUp?.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => OnClickCloseCompletePopUp());

        //InputField(�������͂��Ď����A��ʂ̕\���ؑւ��s���j
        displayNameInput?.OnEndEditAsObservable()
            .Subscribe(x => UpdateDisplayName(x));
    }

    /// <summary>
    /// ���[�U�[���̒l�ƕ\���̍X�V
    /// </summary>
    /// <param name="newName"></param>
    private void UpdateDisplayName(string newName)
    {
        txtDisplayName.text = newName;
        displayName = newName;
        Debug.Log(displayName);
    }

    /// <summary>
    /// OK�{�^�����������ۂ̏���
    /// </summary>
    private async void OnClickSubmit()
    {
        Debug.Log("OK�@�A�J�E���g�A�g�̏��F�J�n");

        //Email�ƃp�X���[�h�𗘗p���āA���[�U�[�A�J�E���g�̘A�g�����݂�
        (bool isSuccess, string message) response = await PlayerPlofileManager.UpdateUserDisplayNameAsync(displayName);

        //Debug�p
        if (response.isSuccess)
        {
            Debug.Log("���[�U�[���@�X�V����");
        }
        else
        {
            Debug.Log("���[�U�[���@�X�V���s");
        }

        txtResponseInfo.text = response.message;
        responsePopUp.SetActive(true);
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
}
