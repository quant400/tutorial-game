using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorDoorScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(1);
        if (other.CompareTag("Player"))
        {
            other.GetComponent<TutorialPlayerScript>().DisableMovement();
            other.GetComponent<CharacterController>().enabled = true;
            other.GetComponent<StarterAssets.ThirdPersonController>().enabled = true;
            gameplayView.instance.GetComponent<uiView>().ActivateNectGame(games.warrior);
            GetComponent<BoxCollider>().enabled = false;
            Invoke("ActivateDoor",3f);

        }
    }

    void ActivateDoor()
    {
        GetComponent<BoxCollider>().enabled = true;
    }
}
