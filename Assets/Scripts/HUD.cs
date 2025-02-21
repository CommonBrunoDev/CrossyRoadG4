using System.Threading;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] GameObject PlayerCoins_ref;
    [SerializeField] GameObject PlayerPoints_ref;
    [SerializeField] GameObject InGameRunningPanel_ref;
    [SerializeField] GameObject InGamePausePanel_ref;
    [SerializeField] GameObject TimerResumeButton_ref;
    [SerializeField] GameObject ResumeButton_ref;
    TMP_Text TimerResumeButtonText_ref;
    TMP_Text PlayerCoinsText_ref;
    TMP_Text PlayerPointsText_ref;

    private void Awake()
    {
        TimerResumeButtonText_ref = TimerResumeButton_ref.GetComponent<TMP_Text>();
        PlayerCoinsText_ref = PlayerCoins_ref.GetComponent<TMP_Text>();
        PlayerPointsText_ref = PlayerPoints_ref.GetComponent<TMP_Text>();
    }

    public void PauseGame()
    {
        InGameRunningPanel_ref.SetActive(false);
        InGamePausePanel_ref.SetActive(true);
        TimerResumeButton_ref.SetActive(false);
        ResumeButton_ref.SetActive(true);
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
    }
}
