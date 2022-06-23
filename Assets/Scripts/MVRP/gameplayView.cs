
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;
using UniRx.Triggers;
using UniRx;
using UniRx.Operators;
public class gameplayView : MonoBehaviour
{
    public static gameplayView instance;


    [SerializeField]
    GameObject player;


    public NFTInfo chosenNFT;


    public static NFTInfo[] nftDataArray;
    public static bool playerLogged;
    public GameObject gameOverObject;

    bool sfxMuted = false;

    public bool isTryout = false;
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




    public void StartGame()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(chosenNFT.id);
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


}

