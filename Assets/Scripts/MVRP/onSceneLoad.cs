using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onSceneLoad : MonoBehaviour
{
    
    void Start()
    {
        tutorialGameModel.lastSavedStep = tutorialGameModel.gameCurrentStep.Value;
        tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.GameSteps.onSceneLoaded;
        tutorialGameModel.gameCurrentStep.Value = tutorialGameModel.lastSavedStep;

    }
}
