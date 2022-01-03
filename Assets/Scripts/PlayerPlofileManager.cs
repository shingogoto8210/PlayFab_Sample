using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
public static class PlayerPlofileManager
{
    /// <summary>
    /// PlayerProfileModel�N���X�̃v���p�e�B
    /// </summary>
    public static PlayerProfileModel Profile { get; set; }

    public static string PlayFab => Profile.PlayerId;   �@�@�@�@ //PlayerProfileModel�N���X���ɗp�ӂ���Ă���PlayerId�ϐ��𗘗p����

    public static string UserDisplayName => Profile.DisplayName; //PlayerProfileModel�N���X���ɗp�ӂ���Ă���DisplayName�ϐ��𗘗p����

    /// <summary>
    /// ���[�U�[���̍X�V
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static async UniTask<(bool isSuccess, string errorMessage)> UpdateUserDisplayNameAsync(string name)
    {
        //�V�������[�U�[���̍쐬
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };

        //PlayFab�֍X�V����
        var response = await PlayFabClientAPI.UpdateUserTitleDisplayNameAsync(request);

        //�G���[�n���h�����O(�h�L�������g���Q�l�ɂ��Ă���j
        //https://docs.microsoft.com/en-us/rest/api/playfab/client/account-management/updateusertitledisplayname?view=playfab-rest
        if(response.Error != null)
        {
            switch (response.Error.Error)
            {
                case PlayFabErrorCode.InvalidParams:
                    return (false, "���[�U�[�̖��O��3�`25�����ȓ��œ��͂��Ă��������B");

                case PlayFabErrorCode.ProfaneDisplayName:
                case PlayFabErrorCode.NameNotAvailable:
                    return (false, "���̖��O�͎g�p�ł��܂���B");

                default:
                    return (false, "�z��O�̃G���[");
            }
        }

        //���[�J���̃f�[�^���X�V
        Profile.DisplayName = name;

        //�G���[�Ȃ��ōX�V����
        return (true, "���[�U�[�����X�V���܂����B�F�V�������[�U�[��" + response.Result.DisplayName);
    }

    /// <summary>
    /// PlayFab����Client�փf�[�^�𓯊�
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="statisticValues"></param>
    public static void SyncPlayFabToClient(PlayerProfileModel profile, List<StatisticValue> statisticValues)
    {

        //���񃍃O�C������null�̂���new���Ă���
        Profile = profile ?? new PlayerProfileModel();

        //TODO statistic ��ǉ�����
    }

    //TODO ���v���A���x���A�L�����̍X�V�����̒ǉ�
}
