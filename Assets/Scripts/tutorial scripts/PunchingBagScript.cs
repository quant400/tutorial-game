using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBagScript : MonoBehaviour
{

    [Tooltip("Direction of punch")]
    public Vector3 punchDirection;

    [Tooltip("Power of punch")]
    public float power;

    private Rigidbody bag;

    // Start is called before the first frame update
    void Start()
    {
        bag = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void punchIt()
    {
        
        bag.AddForce(punchDirection * power);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Punch")|| collision.gameObject.CompareTag("Kick"))
        {
            if (collision.gameObject.GetComponentInParent<TutorialPlayerScript>().GetAttacking())
            {
               
                Vector3 attackLoc = collision.GetContact(0).point;
                punchDirection = transform.position - attackLoc;
                punchIt();
                collision.gameObject.GetComponentInParent<TutorialPlayerScript>().SetAttacking(false);
            }
        }
    }
}
