
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



    public void SpawnPlayer()
    {
        Debug.Log(1);
        string n = gameplayView.instance.chosenNFT.name;
        Debug.Log(chosenNFT.name);
        GameObject resource = Resources.Load(Path.Combine("SinglePlayerPrefabs/Characters", NameToSlugConvert(n))) as GameObject;
        player = Instantiate(resource, new Vector3(7.7f, 0, 30), Quaternion.identity);
        Debug.Log(chosenNFT.id);
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

