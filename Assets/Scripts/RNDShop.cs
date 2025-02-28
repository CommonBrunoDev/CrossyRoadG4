using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RNDShop : MonoBehaviour
{

    [SerializeField] GameObject RNDShopPlaceHolderSpawn_Ref;
    [SerializeField] GameObject RNDShopPlaceHolderIn_Ref;
    [SerializeField] GameObject RNDShopPlaceHolderOut_Ref;
    GameObject GM_ref;
    GM GMCompontent_ref;
    [SerializeField] List<GameObject> UnlockableCharacters;
    [SerializeField] List<int> UnlockableCharactersPercentages;

    private void Start()
    {
        GM_ref = GameObject.FindGameObjectWithTag("GM");
        GMCompontent_ref = GM_ref.GetComponent<GM>();
        GMCompontent_ref.PlayerCoins = 500;
    }

    public void PurchaseRandomCharacter()
    {
        List<GameObject> Unlockable = new List<GameObject> { };
        List<int> Percentages = new List<int> { };
        int TotalPercentages = 0;
        if (GMCompontent_ref.PlayerCoins >= 100)
        {
            GMCompontent_ref.PlayerCoins -= 100;
            GMCompontent_ref.UpdatePlayerStats();
            GMCompontent_ref.GetComponent<PlayerDataManager>().SaveData();
            foreach (GameObject UnlockableCharacter in UnlockableCharacters)
            {
                if (!GMCompontent_ref.UnlockedCharacters.Contains(UnlockableCharacter))
                {
                    Unlockable.Add(UnlockableCharacter);
                    Percentages.Add(UnlockableCharactersPercentages[UnlockableCharacters.IndexOf(UnlockableCharacter)]);
                    TotalPercentages += UnlockableCharactersPercentages[UnlockableCharacters.IndexOf(UnlockableCharacter)];
                }
            }
            int RND = Random.Range(1, TotalPercentages);
            int TotRND = 0;
            for (int i = 0; i < Unlockable.Count; i++)
            {
                TotRND += Percentages[i];
                if (RND <= TotRND)
                {
                    GMCompontent_ref.UnlockedCharacters.Add(Unlockable[i]);
                    GMCompontent_ref.GetComponent<PlayerDataManager>().SaveData();
                    return;
                }
            }
        }
        else
        {
            Debug.LogError("Non hai abbastanza monete");
        }
    }
}