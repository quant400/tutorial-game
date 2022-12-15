using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Toolkit;
using UniRx.Triggers;
using UniRx.Operators;

public class uiView : MonoBehaviour
{
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject characterSelectionPanel;
    [SerializeField] GameObject loginCanvas;
    [SerializeField] GameObject resultsCanvas;
    [SerializeField] GameObject leaderBoeardCanvas;
    [SerializeField] GameObject startCanvas;
    [SerializeField] GameObject nextGameCanvas;
    [SerializeField] GameObject SkipScreen;


    public Button loginBtn, PlayMode, Play, LeaderBoard, BackToCharacterSelection, Skip, tryout, backFromLeaderboard , tryagain , skipGame;
    [SerializeField] webLoginView webloginView;
    // Start is called before the first frame update
    private void Awake()
    {
        scenesView.LoadScene(tutorialGameModel.mainSceneLoadname.sceneName);
    }
    void Start()
    {
        ObserveBtns();
    }
    public void observeLogin()
    {
        tutorialGameModel.userIsLogged
          .Where(_ => _)
          .Do(_ => tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.Onlogged)
          .Subscribe()
          .AddTo(this);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ObserveBtns()
    {
        loginBtn.OnClickAsObservable()
            .Do(_=> webloginView.OnLogin(loginBtn, Skip, tryout))
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);
        PlayMode.OnClickAsObservable()
           .Do(_ => PlayMainButton())
           .Where(_ => PlaySounds.instance != null)
           .Do(_ => PlaySounds.instance.Play())
           .Subscribe()
           .AddTo(this);
        LeaderBoard.OnClickAsObservable()
          .Do(_ => openLeaderBoard())
          .Where(_ => PlaySounds.instance != null)
          .Do(_ => PlaySounds.instance.Play())
          .Subscribe()
          .AddTo(this);
        Skip.OnClickAsObservable()
        .Do(_ => webloginView.OnSkip())
        .Where(_ => PlaySounds.instance != null)
        .Do(_ => PlaySounds.instance.Play())
        .Subscribe()
        .AddTo(this);  

        tryout.OnClickAsObservable()
        .Do(_ => webloginView.OnTryout())
        .Where(_ => PlaySounds.instance != null)
        .Do(_ => PlaySounds.instance.Play())
        .Subscribe()
        .AddTo(this); 
        
        skipGame.OnClickAsObservable()
        .Do(_ => EnableSkipScreen())
        .Where(_ => PlaySounds.instance != null)
        .Do(_ => PlaySounds.instance.Play())
        .Subscribe()
        .AddTo(this);

    }
    public void closeLeaderBoard()
    {
        //leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(false);

    }
    public void openLeaderBoard()
    {
        //leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(true);

        tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.OnLeaderBoard;
    }
    public void PlayMainButton()
    {

       
        tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.OnCharacterSelection;

    }
    public void goToMenu(string menuName)
    {
      
        switch (menuName)
        {
            case "login":
                mainMenuCanvas.gameObject.SetActive(false);
                characterSelectionPanel.SetActive(false);
                loginCanvas.gameObject.SetActive(true);
                startCanvas.gameObject.SetActive(true);

                //leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(false);
                //resultsCanvas.SetActive(false);

                break;
            case "main":
                mainMenuCanvas.gameObject.SetActive(true);
                startCanvas.gameObject.SetActive(true);
                characterSelectionPanel.SetActive(false);
                loginCanvas.gameObject.SetActive(false);
                //resultsCanvas.SetActive(false);
                //leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(false);
                break;
            case "characterSelection":
                mainMenuCanvas.gameObject.SetActive(false);
                startCanvas.gameObject.SetActive(false);
                characterSelectionPanel.SetActive(true);
                loginCanvas.gameObject.SetActive(false);
                //resultsCanvas.SetActive(false);
                //leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(false);
                break;
            case "results":
                mainMenuCanvas.gameObject.SetActive(false);
                characterSelectionPanel.SetActive(false);
                //leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(false);
                loginCanvas.gameObject.SetActive(false);
               // resultsCanvas.SetActive(true);
                startCanvas.gameObject.SetActive(false);

                break;
            case "leaderboeard":
                break;
            case "characterSelected":
                mainMenuCanvas.gameObject.SetActive(false);
                characterSelectionPanel.SetActive(false);
                loginCanvas.gameObject.SetActive(false);
                startCanvas.gameObject.SetActive(false);
                //resultsCanvas.SetActive(false);
                //leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(false);
                break;

        }
    }

    void EnableSkipScreen()
    {
        SkipScreen.SetActive(true);
    }
    public void ActivateNectGame(games g)
    {
        nextGameCanvas.GetComponent<NextGameCanvasScript>().EnablePanel(g);
    }
    public void SetTryAgain(bool state)
    {
        /*if(gameplayView.instance.GetSessions()<10)
            tryagain.gameObject.SetActive(state);*/

    }
}
