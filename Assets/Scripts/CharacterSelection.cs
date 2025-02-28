using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] GameObject CharacterSelectionPlaceHolderMid_Ref;
    [SerializeField] GameObject CharacterSelectionPlaceHolderDX_Ref;
    [SerializeField] GameObject CharacterSelectionPlaceHolderSX_Ref;
    [SerializeField] GameObject PlayerChild_ref;
    [SerializeField] GameObject Player_ref;
    MeshFilter PlayerMesh_ref;
    GameObject CharacterMid;
    GameObject CharacterDX;
    GameObject CharacterSX;
    Vector3 CharacterMidActualPosition;
    Vector3 CharacterDXActualPosition;
    Vector3 CharacterSXActualPosition;
    float timer = 0;
    [SerializeField, Range(0.1f, 1f)] float CharacterSwitchSpeed;
    [SerializeField] List<GameObject> Characters;
    [SerializeField] Material LockedMaterial;
    public GameObject PlayerActualCharacter;
    int CharactersIndex = 0;
    GameObject GM_ref;
    GM GMCompontent_ref;

    private void Start()
    {
        int LastElement = Characters.Count-1;
        CharacterSX = Instantiate(Characters[CharactersIndex + 1], CharacterSelectionPlaceHolderSX_Ref.transform.position, Quaternion.Euler(10, -15, 0));
        CharacterMid = Instantiate(Characters[CharactersIndex], CharacterSelectionPlaceHolderMid_Ref.transform.position, Quaternion.Euler(10, -15, 0));
        CharacterDX = Instantiate(Characters[LastElement], CharacterSelectionPlaceHolderDX_Ref.transform.position, Quaternion.Euler(10, -15, 0));
        CharactersIndex = 0;
        PlayerMesh_ref = PlayerChild_ref.GetComponent<MeshFilter>();
        GM_ref = GameObject.FindGameObjectWithTag("GM");
        GMCompontent_ref = GM_ref.GetComponent<GM>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (CharacterDX.transform.position != CharacterSelectionPlaceHolderDX_Ref.transform.position || CharacterSX.transform.position != CharacterSelectionPlaceHolderSX_Ref.transform.position || CharacterMid.transform.position != CharacterSelectionPlaceHolderMid_Ref.transform.position)
        {
            CharacterSX.transform.position = Vector3.Lerp(CharacterSXActualPosition, CharacterSelectionPlaceHolderSX_Ref.transform.position, timer / CharacterSwitchSpeed);
            CharacterDX.transform.position = Vector3.Lerp(CharacterDXActualPosition, CharacterSelectionPlaceHolderDX_Ref.transform.position, timer / CharacterSwitchSpeed);
            CharacterMid.transform.position = Vector3.Lerp(CharacterMidActualPosition, CharacterSelectionPlaceHolderMid_Ref.transform.position, timer / CharacterSwitchSpeed);
        }
    }
    public void NextCharacter()
    {
        timer = 0;
        Destroy(CharacterDX);
        CharacterDX = CharacterMid;
        CharacterMid = CharacterSX;
        int CharactersLenght = Characters.Count-1;
        if (CharactersIndex == CharactersLenght)
        {
            CharacterSX = Instantiate(Characters[1], CharacterSelectionPlaceHolderSX_Ref.transform.position, Quaternion.Euler(10, -15, 0));
            CharactersIndex = 0;
        }
        else if (CharactersIndex == CharactersLenght - 1)
        {
            CharacterSX = Instantiate(Characters[0], CharacterSelectionPlaceHolderSX_Ref.transform.position, Quaternion.Euler(10, -15, 0));
            CharactersIndex += 1;
        }
        else
        {
            CharacterSX = Instantiate(Characters[CharactersIndex + 2], CharacterSelectionPlaceHolderSX_Ref.transform.position, Quaternion.Euler(10, -15, 0));
            CharactersIndex += 1;
        }
        CharacterSXActualPosition = CharacterSX.transform.position;
        CharacterDXActualPosition = CharacterDX.transform.position;
        CharacterMidActualPosition = CharacterMid.transform.position;
        CheckInUnlockedCharacters();
    }
    public void PreviousCharacter()
    {
        timer = 0;
        Destroy(CharacterSX);
        CharacterSX = CharacterMid;
        CharacterMid = CharacterDX;
        int CharactersLenght = Characters.Count - 1;
        if (CharactersIndex == 0)
        {
            CharacterDX = Instantiate(Characters[CharactersLenght-1], CharacterSelectionPlaceHolderDX_Ref.transform.position, Quaternion.Euler(10, -15, 0));
            CharactersIndex = CharactersLenght;
        }
        else if (CharactersIndex == 1)
        {

            CharacterDX = Instantiate(Characters[CharactersLenght], CharacterSelectionPlaceHolderDX_Ref.transform.position, Quaternion.Euler(10, -15, 0));
            CharactersIndex -= 1;
        }
        else
        {
            CharacterDX = Instantiate(Characters[CharactersIndex - 2], CharacterSelectionPlaceHolderDX_Ref.transform.position, Quaternion.Euler(10, -15, 0));
            CharactersIndex -= 1;
        }
        CharacterSXActualPosition = CharacterSX.transform.position;
        CharacterDXActualPosition = CharacterDX.transform.position;
        CharacterMidActualPosition = CharacterMid.transform.position;
        CheckInUnlockedCharacters();
    }
    public void CheckInUnlockedCharacters()
    {
            if (!GMCompontent_ref.UnlockedCharacters.Contains(Characters[CharactersIndex]))
            {
                GM_ref.GetComponent<HUD>().LockCharacterSelection();
                return;
            }
            else
            {
                GM_ref.GetComponent<HUD>().UnlockCharacterSelection();
            }
    }
    public void LoadSelectedCharacter()
    {
        Destroy(PlayerChild_ref);
        if (PlayerActualCharacter == null)
        {
            PlayerChild_ref = Instantiate(Characters[0], Player_ref.transform.position, Player_ref.transform.rotation);
        }
        else
        {
            PlayerChild_ref = Instantiate(PlayerActualCharacter, Player_ref.transform.position, Player_ref.transform.rotation);
        }
        PlayerChild_ref.transform.parent = Player_ref.transform;
    }
    public void SelectCharacter()
    {
        Destroy(PlayerChild_ref);
        PlayerChild_ref = Instantiate(Characters[CharactersIndex], Player_ref.transform.position, Player_ref.transform.rotation);
        PlayerChild_ref.transform.parent = Player_ref.transform;
        PlayerActualCharacter = Characters[CharactersIndex];
        GMCompontent_ref.GetComponent<PlayerDataManager>().SaveData();
    }
}
