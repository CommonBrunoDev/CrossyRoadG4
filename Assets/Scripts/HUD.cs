using System.Threading;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] GameObject PlayerCoins_ref;
    [SerializeField] GameObject PlayerPoints_ref;
    [SerializeField] GameObject PlayerPauseCoins_ref;
    [SerializeField] GameObject PlayerPausePoints_ref;
    [SerializeField] GameObject PlayerRNDCoins_ref;
    [SerializeField] GameObject PlayerRNDPoints_ref;
    [SerializeField] GameObject InGameRunningPanel_ref;
    [SerializeField] GameObject InGamePausePanel_ref;
    [SerializeField] GameObject PreGameSettingsPanel_ref;
    [SerializeField] GameObject PreGameCharacterSelectionPanel_ref;
    [SerializeField] GameObject PreGameRNDShopPanel_ref;
    [SerializeField] GameObject TimerResumeButton_ref;
    [SerializeField] GameObject ResumeButton_ref;
    [SerializeField] GameObject PreGameStartMenu_ref;
    TMP_Text TimerResumeButtonText_ref;
    TMP_Text PlayerCoinsText_ref;
    TMP_Text PlayerPointsText_ref;
    TMP_Text PlayerPauseCoinsText_ref;
    TMP_Text PlayerPausePointsText_ref;
    TMP_Text PlayerRNDCoinsText_ref;
    GameObject GM_ref;
    GM GMCompontent_ref;

    private void Awake()
    {
        TimerResumeButtonText_ref = TimerResumeButton_ref.GetComponent<TMP_Text>();
        PlayerCoinsText_ref = PlayerCoins_ref.GetComponent<TMP_Text>();
        PlayerPointsText_ref = PlayerPoints_ref.GetComponent<TMP_Text>();
        PlayerPauseCoinsText_ref = PlayerPauseCoins_ref.GetComponent<TMP_Text>();
        PlayerPausePointsText_ref = PlayerPausePoints_ref.GetComponent<TMP_Text>();
        PlayerRNDCoinsText_ref = PlayerRNDCoins_ref.GetComponent<TMP_Text>();
        GM_ref = GameObject.FindGameObjectWithTag("GM");
        GMCompontent_ref = GM_ref.GetComponent<GM>();
    }
    public void StartGame()
    {
        PreGameStartMenu_ref.SetActive(false);
        InGameRunningPanel_ref.SetActive(true);
        GMCompontent_ref.b_IsStarted = true;
    }
    public void PauseGame()
    {
        InGameRunningPanel_ref.SetActive(false);
        InGamePausePanel_ref.SetActive(true);
        TimerResumeButton_ref.SetActive(false);
        ResumeButton_ref.SetActive(true);
        GMCompontent_ref.UpdatePlayerStats();
    }
    public void OpenCharacterSelection(GameObject Panel_ref)
    {
        Panel_ref.SetActive(false);
        PreGameCharacterSelectionPanel_ref.SetActive(true);
    }
    public void OpenGameSettings(GameObject Panel_ref)
    {
        Panel_ref.SetActive(false);
        PreGameSettingsPanel_ref.SetActive(true);
    }
    public void OpenRNDShop(GameObject Panel_ref)
    {
        Panel_ref.SetActive(false);
        PreGameRNDShopPanel_ref.SetActive(true);
    }
    public void BackToStartMenu(GameObject Panel_ref)
    {
        Panel_ref.SetActive(false);
        PreGameStartMenu_ref.SetActive(true);
    }
    public void ResumeGame()
    {
        InGamePausePanel_ref.SetActive(false);
        InGameRunningPanel_ref.SetActive(true);
    }

    public void ResumingGame(float timer)
    {
        ResumeButton_ref.SetActive(false);
        TimerResumeButton_ref.SetActive(true);
        int IntTimer = ((int)timer) + 1;
        TimerResumeButtonText_ref.text = IntTimer.ToString();
    }

    public void UpdatePlayerValues(int PlayerCoins, int PlayerPoints)
    {
        PlayerCoinsText_ref.text = PlayerCoins.ToString();
        PlayerPointsText_ref.text = PlayerPoints.ToString();
        PlayerPauseCoinsText_ref.text = PlayerCoins.ToString();
        PlayerPausePointsText_ref.text = PlayerPoints.ToString();
        PlayerRNDCoinsText_ref.text = PlayerCoins.ToString();
    }
}
