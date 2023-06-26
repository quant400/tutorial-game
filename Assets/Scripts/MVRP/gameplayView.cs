
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
    public JuiceDisplayScript juiceDisplay;
    bool sfxMuted = false;

    public bool isTryout = false;

    public bool isPaused=false;
    public bool hasOtherChainNft=false;
    public bool usingFreemint = false;
    public bool usingMeta=false;
    public (string, string) logedPlayer;
    
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
        string n = NameToSlugConvert(chosenNFT.name);
        player = Instantiate(playerPrefab, new Vector3(7.7f, 0, 30), Quaternion.identity);
        player.GetComponent<SetUpSkin>().SetUpChar(n);//(n) changed for special events that need character replacement 
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
        if (name == "Red Velvet")
            slug = "neurotica";
        if (name == "Mañana")
            slug = "manana";
        if (name == "Horatio’d")
            slug = "horatiod";
        return slug;

    }

    public string GetLoggedPlayerString()
    {
        if (usingMeta)
            return PlayerPrefs.GetString("Account");
        else
            return logedPlayer.Item1 + "$$$" + logedPlayer.Item2;
    }



    public void getJuiceFromRestApi(string assetId)
    {
        StartCoroutine(getJuiceRestApi(assetId));
    }
    public void getFightFromRestApi(string assetId)
    {
        StartCoroutine(getFightRestApi(assetId));
    }
    struct reply
    {
        public string id;
        public string available;
        public string freeze;
        public string total;
        public string status;
    }
    struct fightReply
    {
        public string balance;
    }
    struct JuiceID
    {
        public string id;
    }
    struct FightID
    {
        public string address;
    }
    IEnumerator getJuiceRestApi(string assetId)
    {
        string url = "";
        JuiceID jId = new JuiceID();
        jId.id = assetId;
        if (KeyMaker.instance.buildType == BuildType.staging)
            url = "https://staging-api.cryptofightclub.io/game/sdk/juice/balance";
        else if (KeyMaker.instance.buildType == BuildType.production)
            url = "https://api.cryptofightclub.io/game/sdk/juice/balance";
        string idJsonData = JsonUtility.ToJson(jId);
        using (UnityWebRequest request = UnityWebRequest.Put(url, idJsonData))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(idJsonData);
            request.method = "POST";
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.error == null)
            {
                string result = Encoding.UTF8.GetString(request.downloadHandler.data);
                reply r = JsonUtility.FromJson<reply>(request.downloadHandler.text);
                if (KeyMaker.instance.buildType == BuildType.staging)
                    Debug.Log(request.downloadHandler.text);
                if (r.status == "false")
                    gameplayView.instance.juiceDisplay.SetJuiceBal("0");
                else
                    gameplayView.instance.juiceDisplay.SetJuiceBal(r.total);

            }
            else
            {
                Debug.Log(request.error);
            }


        }
    }
    IEnumerator getFightRestApi(string aaddress)
    {
        string url = "";
        FightID fId = new FightID();
        fId.address = aaddress;
        if (KeyMaker.instance.buildType == BuildType.staging)
            url = "https://staging-api.cryptofightclub.io/game/sdk/fight-balance";
        else if (KeyMaker.instance.buildType == BuildType.production)
            url = "https://api.cryptofightclub.io/game/sdk/fight-balance";
        string idJsonData = JsonUtility.ToJson(fId);
        using (UnityWebRequest request = UnityWebRequest.Put(url, idJsonData))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(idJsonData);
            request.method = "POST";
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.error == null)
            {
                string result = Encoding.UTF8.GetString(request.downloadHandler.data);
                fightReply r = JsonUtility.FromJson<fightReply>(request.downloadHandler.text);
                if (KeyMaker.instance.buildType == BuildType.staging)
                    Debug.Log(request.downloadHandler.text);

                gameplayView.instance.juiceDisplay.SetCoinBal(r.balance);

            }
            else
            {
                Debug.Log(request.error);
            }


        }
    }
}

