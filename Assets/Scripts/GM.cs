using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GM : MonoBehaviour
{
    int PlayerPoints = 0;
    public int PlayerCoins = 0;
    public int PlayerHighScore = 0;
    [SerializeField] public float Resumetimer;
    public List<GameObject> UnlockedCharacters;
    public bool b_IsStarted = false;
    public bool b_IsResuming;
    public bool b_IsInPause;
    HUD HUDComponent_ref;
    private void Start()
    {
        gameObject.GetComponent<PlayerDataManager>().LoadData();
        HUDComponent_ref = gameObject.GetComponent<HUD>();
        HUDComponent_ref.UpdatePlayerValues(PlayerCoins, PlayerPoints);
    }
    
    private void Update()
    {
        if (b_IsResuming)
        {
            WaitResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        HUDComponent_ref.PauseGame();
        b_IsInPause = true;
    }
    public void WaitResumeGame()
    {
        b_IsResuming = true;
        Resumetimer -= (Time.unscaledDeltaTime);
        HUDComponent_ref.ResumingGame(Resumetimer);
        if (Resumetimer <= 0)
        {
            ResumeGame();
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameObject.GetComponent<HUD>().ResumeGame();
        b_IsInPause = false;
        b_IsResuming = false;
        Resumetimer = 3;
    }
    public void IncreasePlayerCoins()
    {
        PlayerCoins += 1;
        HUDComponent_ref.UpdatePlayerValues(PlayerCoins, PlayerPoints);
        gameObject.GetComponent<PlayerDataManager>().SaveData();
    }

    public void IncreasePlayerPoints()
    {
        PlayerPoints += 1;
        HUDComponent_ref.UpdatePlayerValues(PlayerCoins, PlayerPoints);
        if (PlayerPoints > PlayerHighScore)
        {
            PlayerHighScore = PlayerPoints;
            gameObject.GetComponent<PlayerDataManager>().SaveData();
        }
    }
    public void UpdatePlayerStats()
    {
        HUDComponent_ref.UpdatePlayerValues(PlayerCoins, PlayerPoints);
    }
}
