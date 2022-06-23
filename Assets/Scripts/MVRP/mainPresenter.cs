using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Toolkit;
using UniRx.Triggers;
using UniRx.Operators;
using System;
using UnityEngine.SceneManagement;

    public class mainPresenter : MonoBehaviour
    {
    [SerializeField] gameplayView gameView;
    [SerializeField] webLoginView webView;
    [SerializeField] characterSelectionView characterSelectionView;
    [SerializeField] uiView uiView;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if ((scene.name == tutorialGameModel.singlePlayerScene1.sceneName)) 
        {
            Observable.Timer(TimeSpan.Zero)
                        .DelayFrame(2)
                        .Do(_ => tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.OnStartGame)
                        .Subscribe()
                        .AddTo(this);
        }
    }
    // Start is called before the first frame update
    void Start()
        {
        ObservePanelsStatus();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    // Update is called once per frame
    void Update()
        {

        }

    void ObservePanelsStatus()
    {
            tutorialGameModel.gameCurrentStep
                   .Subscribe(procedeGame)
                   .AddTo(this);

            void procedeGame(tutorialGameModel.GameSteps status)
            {
                switch (status)
                {
                    case tutorialGameModel.GameSteps.OnLogin:
                       
                        if (tutorialGameModel.userIsLogged.Value)
                        {
                        tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.OnCharacterSelection;

                        }

                    else
                        {
                        uiView.goToMenu("login");
                        uiView.observeLogin();
                        }
                    break;
                case tutorialGameModel.GameSteps.Onlogged:

                    uiView.goToMenu("main");
                    break;
                case tutorialGameModel.GameSteps.OnPlayMenu:

                    uiView.goToMenu("main");

                    break;
                case tutorialGameModel.GameSteps.OnLeaderBoard:

                    uiView.goToMenu("leaderboeard");
                    break;
                case tutorialGameModel.GameSteps.OnCharacterSelection:
                    uiView.goToMenu("characterSelection");
                    if (!gameplayView.instance.isTryout)
                    {
                        characterSelectionView.MoveRight();
                        characterSelectionView.MoveLeft();
                    }

                    //webView.checkUSerLoggedAtStart(); /// condisder when start load again .....  !!!! 
                    break;
                case tutorialGameModel.GameSteps.OnCharacterSelected:
                    uiView.goToMenu("characterSelected");
                    scenesView.loadSinglePlayerScene();
                    
                    break;
                case tutorialGameModel.GameSteps.OnStartGame:
                    Observable.Timer(TimeSpan.Zero)
                        .DelayFrame(2)
                        .Do(_ => gameView.StartGame())
                        .Subscribe()
                        .AddTo(this);

                    tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.OnGameRunning;

                    break;
                case tutorialGameModel.GameSteps.OnGameRunning:
                    Debug.Log("game Is running");
                    break;
                case tutorialGameModel.GameSteps.OnGameEnded:
                    uiView.goToMenu("results");
                    gameView.EndGame();
                    break;
                case tutorialGameModel.GameSteps.OnBackToCharacterSelection:
                    scenesView.LoadScene(tutorialGameModel.mainSceneLoadname.sceneName);
                    Observable.Timer(TimeSpan.Zero)
                       .DelayFrame(2)
                       .Do(_ => tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.OnCharacterSelection)
                       .Subscribe()
                       .AddTo(this);
                    break;
                case tutorialGameModel.GameSteps.onSceneLoaded:
                    Debug.Log("sceneLoaded");
                    break;


            }

            }
        }
    }


