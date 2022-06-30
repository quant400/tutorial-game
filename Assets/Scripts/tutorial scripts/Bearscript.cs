using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bearscript : MonoBehaviour
{
    Animator anim;
    AudioSource aS;
    bool canHit;
    private void Start()
    {
        anim = GetComponent<Animator>();
        aS = GetComponent<AudioSource>();
        ResetCanHit();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Punch") || collision.gameObject.CompareTag("Kick"))
        {
            if(collision.gameObject.GetComponentInParent<TutorialPlayerScript>().GetAttacking() && canHit)
            {
                canHit = false;
                Invoke("ResetCanHit", 2f);
                transform.LookAt(new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z));
                transform.Translate(transform.forward * -1);
                if (!gameplayView.instance.GetSFXMuted())
                {
                    aS.Play();
                }
                anim.SetTrigger("Hit");
                collision.gameObject.GetComponentInParent<TutorialPlayerScript>().DisableMovement();
                gameplayView.instance.GetComponent<uiView>().ActivateNectGame(games.bear);
            }
        }
    }

    void ResetCanHit()
    {
        canHit = true;
    }
}
