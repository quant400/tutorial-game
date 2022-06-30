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
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tps = player.GetComponent<TutorialPlayerScript>();
        animNPC=GetComponent<Animator>();
        var imp = GameObject.FindGameObjectWithTag("ImpPlace").transform;
        placeAfterEnterGym = imp.GetChild(0);
        placeForCombatTutorial = imp.GetChild(1);
        tC = GetComponentInChildren<TutorialControler>();
    }

    public void MoveToFront()
    {
        movingToFront = true;
        tps.DisableMovement();
        animNPC.SetBool("Walk",true);
        Vector3 pos = player.transform.position + player.transform.forward * 5;
        transform.DOMove(pos, 2f).OnComplete(() =>
        {
            tps.EnableMovement();
            transform.LookAt(player);
            animNPC.SetBool("Walk", false);
            movingToFront = false;
        });
    }
    public void InsideGym()
    {
        noLongerFollow = true;
        transform.position = placeAfterEnterGym.position;
        transform.rotation = placeAfterEnterGym.rotation;
    }
    public void Fighting()
    {
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
                other.gameObject.GetComponentInParent<TutorialPlayerScript>().SetAttacking(false);
                GetHit();
            }
        }
    }
    void Update()
    {
        if (!movingToFront)
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
                if (animNPC.GetBool("Walk") && !following)
                {
                    following = false;
                    animNPC.SetBool("Walk", false);
                }
            }
        }
    }
}
