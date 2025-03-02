using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RNDShop : MonoBehaviour
{

    [SerializeField] GameObject RNDShopPlaceHolderSpawn_Ref;
    [SerializeField] GameObject RNDShopPlaceHolderIn_Ref;
    [SerializeField] GameObject RNDShopPlaceHolderOut_Ref;
    GameObject RNDIn;
    GameObject RNDOut;
    Vector3 RNDInActualPosition;
    Vector3 RNDOutActualPosition;
    [SerializeField, Range(0.1f, 1f)] float RNDSpeed;
    float timer = 0;
    GameObject GM_ref;
    GM GMCompontent_ref;
    [SerializeField] List<GameObject> UnlockableCharacters;
    [SerializeField] List<int> UnlockableCharactersPercentages;

    private void Start()
    {
        GM_ref = GameObject.FindGameObjectWithTag("GM");
        GMCompontent_ref = GM_ref.GetComponent<GM>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(RNDIn != null) 
        { 
            if (RNDIn.transform.position != RNDShopPlaceHolderIn_Ref.transform.position)
            {
                 RNDIn.transform.position = Vector3.Lerp(RNDInActualPosition, RNDShopPlaceHolderIn_Ref.transform.position, timer / RNDSpeed);
                Debug.LogError("RNDInActualPosition : " + RNDInActualPosition);
            }
        }
        if (RNDOut != null)
        {
            if (RNDOut.transform.position != RNDShopPlaceHolderOut_Ref.transform.position)
            {
                RNDOut.transform.position = Vector3.Lerp(RNDOutActualPosition, RNDShopPlaceHolderOut_Ref.transform.position, timer / RNDSpeed);
            }
        }
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
                    PurchaseInAnimation(Unlockable[i]);
                    return;
                }
            }
        }
        else
        {
            Debug.LogError("Non hai abbastanza monete");
        }
    }

    public void PurchaseInAnimation(GameObject UnlockedCharacter)
    {
        timer = 0;
        GM_ref.GetComponent<HUD>().LockRNDPurchase();
        RNDIn = Instantiate(UnlockedCharacter, RNDShopPlaceHolderSpawn_Ref.transform.position, Quaternion.Euler(10, -15, 0));
        RNDInActualPosition = RNDIn.transform.position;
        Debug.LogError("RNDInActualPosition : " + RNDInActualPosition);
        Invoke("PurchaseOutAnimation", 5f);
    }
    public void PurchaseOutAnimation()
    {
        timer = 0;
        RNDOut = RNDIn;
        RNDOutActualPosition = RNDOut.transform.position;
        RNDIn = null;
        GM_ref.GetComponent<HUD>().UnlockRNDPurchase();
        Invoke("PurchaseOutAnimation", 2f);
    }
    public void PurchaseDeleteOut()
    {
        Destroy(RNDOut);
    }
}
