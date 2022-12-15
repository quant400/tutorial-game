
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;
using UniRx.Triggers;
using UniRx;
using UniRx.Operators;
using System.IO;
using UnityEngine.Networking;
using System.Text;

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
    public bool usingMeta=false;
    public (string, string) logedPlayer;
    string juiceBal = "0";
    string coinBal = "0";
    GameObject juiceText, CoinText;
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

    public string GetLoggedPlayerString()
    {
        if (usingMeta)
            return PlayerPrefs.GetString("Account");
        else
            return logedPlayer.Item1 + "$$$" + logedPlayer.Item2;
    }

    public void SetJuiceBal(string val)
    {
        juiceBal = val;
    }
    public void SetCoinBal(string val)
    {
        coinBal = val;
    }
    public void UpdateJuiceBalance()
    {
        if (juiceBal == "")
            juiceText.GetComponent<TMPro.TMP_Text>().text = "0";
        else
            juiceText.GetComponent<TMPro.TMP_Text>().text = juiceBal;
    }

    public void UpdateCoinBalance()
    {
        if (coinBal == "")
            CoinText.GetComponent<TMPro.TMP_Text>().text = "0";
        else
            CoinText.GetComponent<TMPro.TMP_Text>().text = coinBal;
    }




    public void getJuiceFromRestApi(string assetId)
    {
        StartCoroutine(getJuiceRestApi(assetId));
    }
    struct reply
    {
        public string id;
        public string balance;
    };
    IEnumerator getJuiceRestApi(string assetId)
    {
        string url = "";
        if (KeyMaker.instance.buildType == BuildType.staging)
            url = "https://staging-api.cryptofightclub.io/game/sdk/juice/balance/";
        else if (KeyMaker.instance.buildType == BuildType.production)
            url = "https://api.cryptofightclub.io/game/sdk/juice/balance/";
        using (UnityWebRequest request = UnityWebRequest.Get(url + assetId))
        {
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.error == null)
            {
                string result = Encoding.UTF8.GetString(request.downloadHandler.data);
                reply r = JsonUtility.FromJson<reply>(request.downloadHandler.text);
                if (KeyMaker.instance.buildType == BuildType.staging)
                    Debug.Log(request.downloadHandler.text);
                gameplayView.instance.SetJuiceBal(r.balance);

            }
            else
            {
                Debug.Log("error in server");
            }


        }
    }
}

