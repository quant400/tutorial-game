using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class TutorialPlayerScript : MonoBehaviour
{
    Animator playerAnim;
    bool punch = true;
    private Quaternion originalRot;
    private PlayerSFXController pSFXC;
    Vector3 fightPos;
    TutorialControler tC;
    bool attacking=false;
    bool fightingTutorialStarted=false;
    public bool thingPickedUp = false;

    [SerializeField]
    Transform playerHand;

    Transform throwableObject;
    Transform throwableObjectParent;
    public bool CloseToPick;
   
    void Awake()
    {
        playerAnim = GetComponent<Animator>();
        pSFXC = GetComponent<PlayerSFXController>();
        tC = GameObject.FindGameObjectWithTag("TutorialController").GetComponent<TutorialControler>();
    }
    public void EnableFightTutorial()
    {
        fightingTutorialStarted = true;
        DisableMovement();
        transform.position = fightPos;
        var x=GameObject.FindGameObjectWithTag("PunchingBag");
        tC.StartFightTutorial();
        transform.LookAt(new Vector3(x.transform.position.x,transform.position.y,x.transform.position.z));
        playerAnim.SetBool("Fight", true);
        
    }


    public void DisableFightTutorial()
    {
        EnableMovement();
        playerAnim.SetBool("Block", false);
        playerAnim.SetBool("Fight",false);
    }

    void EnableThrowTutorial()
    {
        throwableObject.gameObject.SetActive(true);
    }
    public void DisableMovement()
    {
        GetComponent<CharacterController>().enabled = false;
        
        if (!fightingTutorialStarted)
        {
            GetComponent<ThirdPersonController>().enabled = false;
            transform.GetComponent<Animator>().SetFloat("Speed", 0);
            transform.GetChild(0).LookAt(transform.GetChild(2));
        }
    }

    public void EnableMovement()
    {
        GetComponent<CharacterController>().enabled = true;
        GetComponent<ThirdPersonController>().enabled = true;
    }

    public void Punch()
    {

        originalRot = transform.localRotation;
        SetAttacking(true);
        pSFXC.PlaySwoosh();
        int p = Random.Range(1, 3);
        playerAnim.SetFloat("PunchVal", p);
        playerAnim.SetTrigger("Punch");
        playerAnim.SetBool("Block", false);
        if(tC.fightTutorialDone)
        {
            DisableMovement();
            Invoke("AttackDone",0.5f);
        }


    }

    public void Kick()
    {

        originalRot = transform.localRotation;
        SetAttacking(true);
        int p = Random.Range(1, 3);
        pSFXC.PlaySwoosh();
        playerAnim.SetFloat("KickVal", p);
        playerAnim.SetTrigger("Kick");
        if (tC.fightTutorialDone)
        {
            DisableMovement();
            Invoke("AttackDone", 1.5f);
        }

    }

    public void Block()
    {
        
        playerAnim.SetBool("Block", true);

    }

    public void StopBlock()
    {
        playerAnim.SetBool("Block", false);
        AttackDone();
    }

    public void CloseTo(Transform thing )
    {   if (!thingPickedUp)
        {
            throwableObject = thing;
            CloseToPick = true;
        }
    }
    public void PickUpThing()
    {
        if (!thingPickedUp)
        {
            thingPickedUp = true;
            tC.ObjectPickedUp = true;
            throwableObjectParent = throwableObject.parent;
            throwableObject.GetComponent<SphereCollider>().enabled = false;
            throwableObject.GetComponent<BoxCollider>().enabled = false;
            throwableObject.GetComponent<Rigidbody>().isKinematic = true;
            throwableObject.parent = playerHand;
            throwableObject.localPosition = new Vector3(-0.1f, 0.025f, -0.12f);
            throwableObject.GetComponent<PickableObject>().SetPickUpKey(false);
            playerAnim.SetBool("PickUp", true);
            throwableObject.GetComponent<PickableObject>().EnableThrowKey();
        }
    }

    public void ThrowThing()
    {
        playerAnim.SetBool("PickUp", false);
        thingPickedUp = false;
        throwableObject.GetComponent<Rigidbody>().isKinematic = false;
        throwableObject.GetComponent<PickableObject>().DisableThrowKey();
        throwableObject.parent = throwableObjectParent;
        throwableObject.GetComponent<Rigidbody>().AddForce((20 * transform.forward+transform.up*10),ForceMode.Impulse);
        throwableObject.GetComponent<PickableObject>().Invoke("EnableCollider",0.1f);
        throwableObject.GetComponent<BoxCollider>().enabled = true;
       
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            punch = !punch;
        if (Input.GetMouseButtonDown(0) && fightingTutorialStarted && !thingPickedUp)
        {
            if (tC.fightTutorialDone)
            {
                //playerAnim.applyRootMotion = true;
                playerAnim.SetBool("Fight", true);
            }

            if (punch)
            {
                Punch();
            }
               
            else
                Kick();
        }
        if (Input.GetMouseButtonDown(1) && fightingTutorialStarted && !thingPickedUp)
        {
            if (tC.fightTutorialDone)
            {
                //playerAnim.applyRootMotion = true;
                playerAnim.SetBool("Fight", true);
                DisableMovement();
            }
            Block();
        }
        if (Input.GetMouseButtonUp(1) && !thingPickedUp)
        {
            StopBlock();
        }

        if(Input.GetKeyDown(KeyCode.E) && CloseToPick && !thingPickedUp)
        {
            PickUpThing();
        }
        if (Input.GetMouseButtonDown(0) && thingPickedUp)
        {
            ThrowThing();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FightTutorialStarter"))
        {
            fightPos = other.transform.position;
            EnableFightTutorial();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("EnterGym"))
        {
            tC.EnterRing();
            other.GetComponent<MeshCollider>().enabled = false;

        }
    }

    void AttackDone()
    {
        //transform.rotation = originalRot;
        EnableMovement();
        playerAnim.SetBool("Fight", false);
    }
    public bool GetAttacking()
    {
        return attacking; 
    }
    public void SetAttacking(bool val)
    {
        attacking = val;
    }
}

