
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class NFTInfo
{
    public int id;
    public string name;
}
public class NFTGetView : MonoBehaviour
{
    [SerializeField] characterSelectionView characterSelectView;
    public static UnityWebRequest temp;
    [SerializeField]
    GameObject noNFTCanvas;


    private void Start()
    {
        if (characterSelectView == null)
        {
            characterSelectView = GameObject.FindObjectOfType<characterSelectionView>();
        }
    }
    public void GetNFT()
    {
        Debug.LogWarningFormat("Change this before final build and also rename youngin, sledghammer and long shot");
        string acc = PlayerPrefs.GetString("Account");
        StartCoroutine(GetOtherNft());
        StartCoroutine(GetRequest("https://api.cryptofightclub.io/game/sdk/" + acc));
        
        //testing link
        //StartCoroutine(GetRequest("https://api.cryptofightclub.io/game/sdk/0xbecd7b5cfab483d65662769ad4fecf05be4d4d05"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    temp = webRequest;
                    Display();
                    break;
            }
        }
    }
    public IEnumerator GetOtherNft()
    {
        string uri = "https://api.cryptofightclub.io/game/sdk/koakuma/" + PlayerPrefs.GetString("Account");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("no connection");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string data = "{\"Items\":" + webRequest.downloadHandler.text + "}";
                    NFTInfo[] Data = JsonHelper.FromJson<NFTInfo>(data);
                    if (Data.Length > 0)
                    {
                        gameplayView.instance.hasOtherChainNft = true;
                    }
                    break;
            }
        }
    }



    void Display()
    {
        string data = "{\"Items\":" + temp.downloadHandler.text + "}";
        tutorialGameModel.currentNFTString = data;



        NFTInfo[] NFTData = JsonHelper.FromJson<NFTInfo>(data);


        NFTInfo[] used;
        if (gameplayView.instance.hasOtherChainNft)
        {
            NFTInfo tempNft = new NFTInfo() { name = "grane", id = 100000000 };
            NFTInfo[] tempArr = new NFTInfo[NFTData.Length + 1];
            for (int i = 0; i < tempArr.Length; i++)
            {
                if (i == 0)
                    tempArr[i] = tempNft;
                else
                    tempArr[i] = NFTData[i - 1];
            }
            tutorialGameModel.currentNFTArray = tempArr;
            used = tempArr;
        }

        else
        {
            tutorialGameModel.currentNFTArray = NFTData;
            used = NFTData;
        }
        if (used.Length == 0)
        {
            noNFTCanvas.SetActive(true);
            tutorialGameModel.userIsLogged.Value = false;
        }
        else
        {
            noNFTCanvas.SetActive(false);
            characterSelectView.SetData(used);
            tutorialGameModel.userIsLogged.Value = true;
        }


    }


    public void savedLoggedDisplay()
    {
        if (gameplayView.nftDataArray.Length == 0)
        {
            noNFTCanvas.SetActive(true);
            tutorialGameModel.userIsLogged.Value = false;
        }
        else
        {
            noNFTCanvas.SetActive(false);
            characterSelectView.SetData(tutorialGameModel.currentNFTArray);
            tutorialGameModel.userIsLogged.Value = true;
        }
    }

    //temp Fuction for skip
    public void Skip()
    {
        characterSelectView.Skip();
    }
}

