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
            other.GetComponent<CharacterController>().enabled = true;
            other.GetComponent<StarterAssets.ThirdPersonController>().enabled = true;
            gameplayView.instance.GetComponent<uiView>().ActivateNectGame(games.warrior);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            Invoke("EnableFlag", 3f);
        }
    }



    void EnableFlag()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }
}
