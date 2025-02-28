using System.Threading;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] Image FadeImage;
    [SerializeField] private float fadeSpeed = 0;
    private bool fadeActive = false;

    private void Awake()
    {
        TimerResumeButtonText_ref = TimerResumeButton_ref.GetComponent<TMP_Text>();
        PlayerCoinsText_ref = PlayerCoins_ref.GetComponent<TMP_Text>();
        PlayerPointsText_ref = PlayerPoints_ref.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (FadeImage.color.a > 0 || fadeActive)
        {
            var c = fadeActive ? fadeSpeed : -fadeSpeed;
            FadeImage.color += new Color(0, 0, 0, c);
            if (FadeImage.color.a >= 1) 
            {
                fadeActive = false;
                GM.Instance.ChangeNightmare();
            }
            if (FadeImage.color.a <= 0) { FadeImage.color = new Color(1,1,1,0); }
        }
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

    public void FadeNightmare()
    { fadeActive = true; }
}
