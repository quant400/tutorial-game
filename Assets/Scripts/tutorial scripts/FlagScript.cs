using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            other.GetComponent<TutorialPlayerScript>().DisableMovement();
            gameplayView.instance.GetComponent<uiView>().ActivateNectGame(games.warrior);
        }
    }
}
