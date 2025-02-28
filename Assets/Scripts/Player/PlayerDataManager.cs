using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    [SerializeField] GM GMComponent_ref;
    [SerializeField] CharacterSelection CSComponent_ref;

    private void Start()
    {
       
    }
    public void SaveData()
    {
        PlayerData playerData = new PlayerData();
        playerData.PlayerCoins = GMComponent_ref.PlayerCoins;
        playerData.PlayerHighScore = GMComponent_ref.PlayerHighScore;
        playerData.PlayerCharacter = CSComponent_ref.PlayerActualCharacter;
        playerData.UnlockedCharacters = GMComponent_ref.UnlockedCharacters;

        string json = JsonUtility.ToJson(playerData);
        string path = Application.persistentDataPath + "/PlayerData.json";

        System.IO.File.WriteAllText(path, json);
    }
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/PlayerData.json";
        if (File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            PlayerData LoadedData = JsonUtility.FromJson<PlayerData>(json);

            GMComponent_ref.PlayerCoins = LoadedData.PlayerCoins;
            GMComponent_ref.PlayerHighScore = LoadedData.PlayerHighScore;
            CSComponent_ref.PlayerActualCharacter= LoadedData.PlayerCharacter;
            GMComponent_ref.UnlockedCharacters = LoadedData.UnlockedCharacters;

        }
        else
        {
            Debug.LogError("Non è stato trovato il file di salvataggio");
            PlayerData playerData = new PlayerData();
            playerData.PlayerCoins = 0;
            playerData.PlayerHighScore = 0;

            string json = JsonUtility.ToJson(playerData);
            System.IO.File.WriteAllText(path, json);
        }
    }
}
