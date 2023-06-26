
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.IO;
using UniRx.Triggers;
using UniRx;
using UniRx.Operators;
public class characterSelectionView : MonoBehaviour
{
    public int currentCharacter;
    [SerializeField]
    Camera cam;
    [SerializeField]
    Transform[] characters;
    [SerializeField]
    Transform characterList;
   
    bool selected;
    [SerializeField]
    float sideCharZdisp;
    [SerializeField]
    Button rightButton, leftButton , select, backButton;
    NFTInfo[] myNFT;
    [SerializeField]
    RuntimeAnimatorController controller;
    [SerializeField]
    GameObject buttonsToEnable, ButtonToDisable;
    [SerializeField]
    NFTInfo[] characterNFTMap;

    //for new screen
    [SerializeField]
    Transform[] charButtons;
    bool[] avaliableColors = new bool[] { true, true, true, true, true };
    int currentStartIndex;
    //for skip
    bool skipping;
    UnityEngine.Object[] info;
    public void Start()
    {
        observeCharacterSelectionBtns();
        DisablePlay();
    }
   
    void observeCharacterSelectionBtns()
    {
        rightButton.OnClickAsObservable()
            .Do(_ => MoveRight())
            .Where(_=>PlaySounds.instance!=null)
            .Do(_=> PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);
        leftButton.OnClickAsObservable()
           .Do(_ => MoveLeft())
           .Where(_ => PlaySounds.instance != null)
           .Do(_ => PlaySounds.instance.Play())
           .Subscribe()
           .AddTo(this);
        select.OnClickAsObservable()
         .Do(_ => FinalSelectSinglePlayer())
         .Where(_ => PlaySounds.instance != null)
         .Do(_ => PlaySounds.instance.Play())
         .Do(_ => tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.OnCharacterSelected)
         .Subscribe()
         .AddTo(this);

        backButton.OnClickAsObservable()
         .Do(_ => BackButton())
         .Where(_ => PlaySounds.instance != null)
         .Do(_ => PlaySounds.instance.Play())
         .Subscribe()
         .AddTo(this);
    }

    private void ResetAvalaibleColors()
    {
        for (int i = 0; i < avaliableColors.Length; i++)
        {
            avaliableColors[i] = true;
        }
    }
    public void MoveRight()
    {
         
        currentStartIndex += 4;
        if (skipping) 
        {
            if (currentStartIndex+4 >info.Length-1)
                rightButton.gameObject.SetActive(false);
            else
                rightButton.gameObject.SetActive(true);
            if(currentStartIndex>0)
                leftButton.gameObject.SetActive(true);
            SkipDisplayChars(currentStartIndex);
        }
        else if (gameplayView.instance.usingFreemint)
        {
            if (currentStartIndex + 4 > info.Length - 1)
                rightButton.gameObject.SetActive(false);
            else
                rightButton.gameObject.SetActive(true);
            if (currentStartIndex > 0)
                leftButton.gameObject.SetActive(true);
            FreeMintDisplayChars(currentStartIndex);
        }
        else
        {
            if (currentStartIndex+4 > myNFT.Length-1)
                rightButton.gameObject.SetActive(false);
            else
                rightButton.gameObject.SetActive(true);
            if (currentStartIndex > 0)
                leftButton.gameObject.SetActive(true);
            DisplayChar(currentStartIndex);
        }
        
      
    }

    public void MoveLeft()
    {

        currentStartIndex -= 4;
        if (skipping)
        {
            if (currentStartIndex - 4 < 0)
                leftButton.gameObject.SetActive(false);
            else
                leftButton.gameObject.SetActive(true);
            if (currentStartIndex < info.Length)
                rightButton.gameObject.SetActive(true);
            SkipDisplayChars(currentStartIndex);
        }
        else if (gameplayView.instance.usingFreemint)
        {
            if (currentStartIndex - 4 < 0)
                leftButton.gameObject.SetActive(false);
            else
                leftButton.gameObject.SetActive(true);
            if (currentStartIndex < info.Length)
                rightButton.gameObject.SetActive(true);
            FreeMintDisplayChars(currentStartIndex);
        }
        else
        {
            if (currentStartIndex - 4 < 0)
                leftButton.gameObject.SetActive(false);
            else
                leftButton.gameObject.SetActive(true);
            if (currentStartIndex < myNFT.Length)
                rightButton.gameObject.SetActive(true);
            DisplayChar(currentStartIndex);
        }
    }

    public int GetavaliableColor()
    {
        int c = UnityEngine.Random.Range(0, avaliableColors.Length);
        if (avaliableColors[c] == true)
        {
            avaliableColors[c] = false;
            return c;
        }
        else
            return GetavaliableColor();
    }
    public void EnablePlay()
    {
        select.interactable = true;
    }
    public void DisablePlay()
    {
        select.interactable = false;
    }

    public void Selected()
    {
       
        selected = true;
        characters[currentCharacter].GetComponent<Animator>().SetBool("Selected", true);

    }


    public void UndoFinalSelect()
    {
        rightButton.interactable = true;
        leftButton.interactable = true;
    }

    //added for single player
    public void FinalSelectSinglePlayer()
    {
        gameplayView.instance.chosenNFT = characterNFTMap[currentCharacter];
        selected = true;
       
   

    }

    // foire new character selection
    public void UpdateSelected(int selected)
    {
        currentCharacter = currentStartIndex+selected;
        gameplayView.instance.chosenNFT = characterNFTMap[currentCharacter];
        gameplayView.instance.getJuiceFromRestApi(gameplayView.instance.chosenNFT.id);
    }


    internal void SetData(NFTInfo[] nFTData)
    {
        //will be respossible for setting up characters according to nft
        myNFT = nFTData;
        SetUpCharacters();

    }


    private void SetUpCharacters()
    {
        if (tutorialGameModel.charactersSetted == false)
        {

            

           skipping = false;
           characters = new Transform[myNFT.Length];
           characterNFTMap = new NFTInfo[myNFT.Length];
            
           
        }

        Done();


    }

   
    //skip for new screen
    public void Skip()
    {
        if (!gameplayView.instance.isTryout)
        {
            skipping = true;
            info = Resources.LoadAll("SinglePlayerPrefabs/DisplaySprites/HeadShots", typeof(Sprite));
            characterNFTMap = new NFTInfo[info.Length];
            SkipDisplayChars(0);
            Done();
        }
        else
        {
            gameplayView.instance.chosenNFT = new NFTInfo { name = "grane", id = 175.ToString() };
            selected = true;
            tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.OnCharacterSelected;
        }
    }
   
    void SkipDisplayChars(int startingindex)
    {
        for (int i = 0; i < 4; i++)
        { 
            
            if (i+startingindex>=info.Length)
                charButtons[i].GetComponent<ButtonInfoHolder>().SetChar("null");
            else
            {
                string name = info[i + startingindex].name;
                charButtons[i].GetComponent<ButtonInfoHolder>().SetChar(name);
                characterNFTMap[i + startingindex] = new NFTInfo { id = 175.ToString(), name = name };
            }
        }
        ResetAvalaibleColors();
    }
    public void FreeMint()
    {
        info = Resources.LoadAll("SinglePlayerPrefabs/DisplaySprites/FreeMint/HeadShots", typeof(Sprite));
        characterNFTMap = new NFTInfo[info.Length];
        FreeMintDisplayChars(0);
        Done();
    }

    void FreeMintDisplayChars(int startingindex)
    {
        for (int i = 0; i < 4; i++)
        {

            if (i + startingindex >= info.Length)
                charButtons[i].GetComponent<ButtonInfoHolder>().SetChar("null");
            else
            {
                string name = info[i + startingindex].name;
                charButtons[i].GetComponent<ButtonInfoHolder>().SetChar(name);
                characterNFTMap[i + startingindex] = new NFTInfo { id = 00000.ToString(), name = name };
            }
        }
        ResetAvalaibleColors();
    }
    void DisplayChar(int startingindex)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i + startingindex >= myNFT.Length)
            {
                charButtons[i].GetComponent<ButtonInfoHolder>().SetChar("null");
            }

            else
            {
                string charName = NameToSlugConvert(myNFT[i+startingindex].name);
                charButtons[i].GetComponent<ButtonInfoHolder>().SetChar(charName);
                characterNFTMap[i+startingindex] = myNFT[i+startingindex];
            }
        }
        ResetAvalaibleColors();
        tutorialGameModel.charactersSetted = true;
    }
    private void Done()
    {
        buttonsToEnable.SetActive(true);
        ButtonToDisable.SetActive(false);
    }

    void BackButton()
    {
        Debug.Log("reached");
        tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.Onlogged;
    }

    string NameToSlugConvert(string name)
    {
        string slug;
        slug = name.ToLower().Replace(".", "").Replace("'", "").Replace(" ", "-");
        if (name == "Red Velvet")
            slug = "neurotica";
        if (name == "Ma�ana")
            slug = "manana";
        if (name == "Horatio�d")
            slug = "horatiod";
        return slug;

    }
}

