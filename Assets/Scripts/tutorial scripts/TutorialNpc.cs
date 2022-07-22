using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialNpc : MonoBehaviour
{
    Transform player;

    [SerializeField]
    float minDist;

    [SerializeField]
    float NpcSpeed;

    Transform placeForCombatTutorial;
    Transform placeAfterEnterGym;
    [SerializeField]
    TutorialPlayerScript tps;
    TutorialControler tC;
    Animator animNPC;

    bool following;
    bool noLongerFollow=false;
    bool movingToFront=false;
    public bool hit;

    //for testing 
    Camera cam;
    Plane[] planes;
    Collider objCollider;
    void Start()
    {
        animNPC=GetComponent<Animator>();
        var imp = GameObject.FindGameObjectWithTag("ImpPlace").transform;
        placeAfterEnterGym = imp.GetChild(0);
        placeForCombatTutorial = imp.GetChild(1);
        tC = GetComponentInChildren<TutorialControler>();
        Invoke("GetRefrences", 0.5f);
    }
    
    void GetRefrences()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tps = player.GetComponent<TutorialPlayerScript>();
        cam = player.GetChild(0).GetComponent<Camera>();
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        objCollider = GetComponent<Collider>();
    }

    public void MoveToFront()
    {
        movingToFront = true;
        tps.DisableMovement();
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        animNPC.SetBool("Walk",true);
        if (!GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        {
            Vector3 pos = new Vector3(player.transform.position.x, 0, player.transform.position.z) + new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z)*5;
            transform.DOMove(pos, 2f).OnComplete(() =>
            {
                tps.EnableMovement();
                transform.LookAt(player);
                animNPC.SetBool("Walk", false);
                movingToFront = false;
            });
        }
        else
        {
            tps.EnableMovement();
            transform.LookAt(player);
            animNPC.SetBool("Walk", false);
            movingToFront = false;
        }
    }
    public void InsideGym()
    {
        noLongerFollow = true;
        animNPC.SetBool("Walk", false);
        transform.position = placeAfterEnterGym.position;
        transform.rotation = placeAfterEnterGym.rotation;
    }
    public void Fighting()
    {
        animNPC.SetBool("Walk", false);
        transform.position = placeForCombatTutorial.position;
        transform.rotation = placeForCombatTutorial.rotation;
    }
    void GetHit()
    {
        hit = true;
        animNPC.SetTrigger("Hit");
        tC.GetHit();
    }

    private void OnCollisionEnter(Collision other)
    {
       
        if (other.gameObject.CompareTag("Punch") || other.gameObject.CompareTag("Kick"))
        {
            Debug.Log(1);
            if (other.gameObject.GetComponentInParent<TutorialPlayerScript>().GetAttacking())
            {
                //other.gameObject.GetComponentInParent<TutorialPlayerScript>().SetAttacking(false);
                GetHit();
            }
        }
    }
    void Update()
    {
        if (!movingToFront && player!=null)
        {
            if (Vector3.Distance(player.position, transform.position) > minDist && !noLongerFollow)
            {
                following = true;
                if (!animNPC.GetBool("Walk"))
                {
                    animNPC.SetBool("Walk", true);
                }
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                transform.Translate(Vector3.forward * NpcSpeed * Time.deltaTime);
            }
            else
            {
                if (animNPC.GetBool("Walk"))
                {
                    following = false;
                    animNPC.SetBool("Walk", false);
                }
            }
        }
    }
}
