using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    int PlayerPoints = 0;
    public int PlayerCoins = 450;
    public int PlayerHighScore = 0;
    [SerializeField] public float Resumetimer;
    public List<GameObject> UnlockedCharacters;
    [SerializeField] GameObject DefaultCharacter;
    public bool b_IsStarted = false;
    public bool b_IsResuming;
    public bool b_IsInPause;
    HUD HUDComponent_ref;
    [SerializeField] CharacterSelection CharacterSelection_ref;

    public bool nightmareMode = false;
    public float nightmareTime = 8;
    private float nightmareTimer = 0;

    private static GM instance;
    public static GM Instance { get { return instance; } }
    private void Awake() { instance = this; }

 

    private void Start()
    {
        nightmareTimer = nightmareTime;
        gameObject.GetComponent<PlayerDataManager>().LoadData();
        if (UnlockedCharacters.Count == 0)
        {
            UnlockedCharacters.Add(DefaultCharacter);
        }
        HUDComponent_ref = gameObject.GetComponent<HUD>();
        HUDComponent_ref.UpdatePlayerValues(PlayerCoins, PlayerPoints);
        CharacterSelection_ref.LoadSelectedCharacter();
    }
    
    private void Update()
    {
        if (b_IsResuming)
        {
            WaitResumeGame();
        }

        if (!b_IsInPause) { nightmareTimer -= Time.deltaTime; }
        if (nightmareTimer <= 0)
        {
            nightmareTimer = nightmareTime;
            HUD.Instance.FadeNightmare();
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
    public void ChangeNightmare()
    {
        nightmareMode = !nightmareMode;
        NightmareMesher[] NMs =  UnityEngine.Object.FindObjectsByType<NightmareMesher>(FindObjectsSortMode.InstanceID);
        for (int i = 0; i < NMs.Length; i++)
        {NMs[i].SetMesh(nightmareMode); }
    }
    public void UpdatePlayerStats()
    {
        HUDComponent_ref.UpdatePlayerValues(PlayerCoins, PlayerPoints);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
