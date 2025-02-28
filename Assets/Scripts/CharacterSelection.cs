using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] GameObject CharacterSelectionPlaceHolderMid_Ref;
    [SerializeField] GameObject CharacterSelectionPlaceHolderDX_Ref;
    [SerializeField] GameObject CharacterSelectionPlaceHolderSX_Ref;
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
        //Invoke("LockLockedCharacters", 1f);
        CharacterSX = Instantiate(Characters[CharactersIndex], CharacterSelectionPlaceHolderSX_Ref.transform.position, Quaternion.Euler(-20, 45, -20));
        CharacterMid = Instantiate(Characters[CharactersIndex + 1], CharacterSelectionPlaceHolderMid_Ref.transform.position, Quaternion.Euler(-20, 45, -20));
        CharacterDX = Instantiate(Characters[CharactersIndex + 2], CharacterSelectionPlaceHolderDX_Ref.transform.position, Quaternion.Euler(-20, 45, -20));
        CharactersIndex = 1;
        PlayerMesh_ref = Player_ref.GetComponent<MeshFilter>();
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
        else
        {
            //timer = 0;
        }
    }
    private void LockLockedCharacters()
    {
        foreach (GameObject Unlocked in Characters)
        {
            if (!GMCompontent_ref.UnlockedCharacters.Contains(Unlocked))
            {
                Characters[Characters.IndexOf(Unlocked)].GetComponent<Renderer>().material.color = Color.black;
            }
        }
    }
    public void NextCharacter()
    {
        timer = 0;
        Destroy(CharacterSX);
        CharacterSX = CharacterMid;
        CharacterMid = CharacterDX;
        int CharactersLenght = Characters.Count-1;
        if (CharactersIndex == CharactersLenght)
        {
            CharacterDX = Instantiate(Characters[1], CharacterSelectionPlaceHolderDX_Ref.transform.position, Quaternion.Euler(-20, 45, -20));
            CharactersIndex = 0;
        }
        else if (CharactersIndex == CharactersLenght - 1)
        {
            CharacterDX = Instantiate(Characters[0], CharacterSelectionPlaceHolderDX_Ref.transform.position, Quaternion.Euler(-20, 45, -20));
            CharactersIndex += 1;
        }
        else
        {
            CharacterDX = Instantiate(Characters[CharactersIndex + 2], CharacterSelectionPlaceHolderDX_Ref.transform.position, Quaternion.Euler(-20, 45, -20));
            CharactersIndex += 1;
        }
        CharacterSXActualPosition = CharacterSX.transform.position;
        CharacterDXActualPosition = CharacterDX.transform.position;
        CharacterMidActualPosition = CharacterMid.transform.position;
    }
    public void PreviousCharacter()
    {
        timer = 0;
        Destroy(CharacterDX);
        CharacterDX = CharacterMid;
        CharacterMid = CharacterSX;
        int CharactersLenght = Characters.Count - 1;
        if (CharactersIndex == 0)
        {
            CharacterSX = Instantiate(Characters[CharactersLenght-1], CharacterSelectionPlaceHolderSX_Ref.transform.position, Quaternion.Euler(-20, 45, -20));
            CharactersIndex = CharactersLenght;
        }
        else if (CharactersIndex == 1)
        {

            CharacterSX = Instantiate(Characters[CharactersLenght], CharacterSelectionPlaceHolderSX_Ref.transform.position, Quaternion.Euler(-20, 45, -20));
            CharactersIndex -= 1;
        }
        else
        {
            CharacterSX = Instantiate(Characters[CharactersIndex - 2], CharacterSelectionPlaceHolderSX_Ref.transform.position, Quaternion.Euler(-20, 45, -20));
            CharactersIndex -= 1;
        }
        CharacterSXActualPosition = CharacterSX.transform.position;
        CharacterDXActualPosition = CharacterDX.transform.position;
        CharacterMidActualPosition = CharacterMid.transform.position;
    }
    public void LoadSelectedCharacter()
    {
        PlayerMesh_ref.sharedMesh = PlayerActualCharacter.GetComponent<MeshFilter>().sharedMesh;
    }
    public void SelectCharacter()
    {
        PlayerMesh_ref.sharedMesh = Characters[CharactersIndex].GetComponent<MeshFilter>().sharedMesh;
        PlayerActualCharacter = Characters[CharactersIndex];
        GMCompontent_ref.GetComponent<PlayerDataManager>().SaveData();
    }
}
