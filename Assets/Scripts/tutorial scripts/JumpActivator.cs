using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpActivator : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<StarterAssets.ThirdPersonController>().JumpHeight = 3;
            other.GetComponent<StarterAssets.ThirdPersonController>().GroundedOffset = 0;
            other.GetComponent<StarterAssets.ThirdPersonController>().GroundedRadius=0.1f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<StarterAssets.ThirdPersonController>().JumpHeight = 0.5f;
            other.GetComponent<StarterAssets.ThirdPersonController>().GroundedOffset = 0.2f;
            other.GetComponent<StarterAssets.ThirdPersonController>().GroundedRadius = 0.28f;
        }
    }
}
