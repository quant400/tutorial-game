
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;
using UniRx.Triggers;
using UniRx;
using UniRx.Operators;
using System.IO;

public class gameplayView : MonoBehaviour
{
    public static gameplayView instance;


    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject playerPrefab;

    public NFTInfo chosenNFT;


    public static NFTInfo[] nftDataArray;
    public static bool playerLogged;
    public GameObject gameOverObject;

    bool sfxMuted = false;

    public bool isTryout = false;

    public bool isPaused=false;
    public bool hasOtherChainNft=false;
    public bool usingFreemint = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;

        }
        if (PlayerPrefs.HasKey("SFX"))
        {
            if (PlayerPrefs.GetString("SFX") == "off")
                sfxMuted = true;
        }
    }



    public void SpawnPlayer()
    {
        // changed for special events that need character replacement 
        //chosenNFT.name = "pumpkin";
        //end
        string n = NameToSlugConvert(chosenNFT.name);
        player = Instantiate(playerPrefab, new Vector3(7.7f, 0, 30), Quaternion.identity);
        player.GetComponent<SetUpSkin>().SetUpChar(n);
    }
    public void StartGame()
    {
      
        tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.OnGameRunning;

    }
    public void EndGame()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }




    public bool GetSFXMuted()
    {
        return sfxMuted;
    }

    public void SetSFXMuted(bool b)
    {
        sfxMuted = b;
    }


    public void PlayClick()
    {
        GetComponent<AudioSource>().Play();
    }

    string NameToSlugConvert(string name)
    {
        string slug;
        slug = name.ToLower().Replace(".", "").Replace("'", "").Replace(" ", "-");
        return slug;

    }
}

