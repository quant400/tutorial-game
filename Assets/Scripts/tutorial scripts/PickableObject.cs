using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField]
    GameObject pickUpKey, ThrowKey;
    Transform cam;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SetPickUpKey(true);
            other.GetComponent<TutorialPlayerScript>().CloseTo(transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            SetPickUpKey(false);
            other.GetComponent<TutorialPlayerScript>().CloseToPick=false;
        }
    }

    public void SetPickUpKey(bool state)
    {
        pickUpKey.SetActive(state);
    }
    public void EnableThrowKey()
    {
        transform.rotation = Quaternion.identity;
        pickUpKey.SetActive(false);
        ThrowKey.SetActive(true);
    }
    public void DisableThrowKey()
    {
        ThrowKey.SetActive(false);
    }
    public void EnableCollider()
    {
        GetComponent<SphereCollider>().enabled = true;
    }
    private void LateUpdate()
    {
         
        this.transform.LookAt(new Vector3(cam.position.x,this.transform.position.y,cam.position.z));

    }
}
