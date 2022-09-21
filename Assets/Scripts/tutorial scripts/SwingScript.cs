using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwingScript : MonoBehaviour
{
    public bool xAxis;
    private void Start()
    {
        Invoke("StartSwing", Random.Range(1f, 4f));
        
    }
    void StartSwing()
    {
        Swing(true);
    }
    void Swing(bool dir)
    {
        if (xAxis)
        {
            if (dir)
                transform.DOLocalRotate(new Vector3(0f, 0f, 85f), 1f).OnComplete(() => Swing(!dir));
            else
                transform.DOLocalRotate(new Vector3(0f, 0f, -85f), 1f).OnComplete(() => Swing(!dir));
        }
        else
        {
            if (dir)
                transform.DOLocalRotate(new Vector3(0f, 90f, 85f), 1f).OnComplete(() => Swing(!dir));
            else
                transform.DOLocalRotate(new Vector3(0f, 90f, -85f), 1f).OnComplete(() => Swing(!dir));
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            other.GetComponent<TutorialPlayerScript>().DisableMovement();
            other.transform.DOMove(GetDir(other.transform)*2,0.5f);
        }
    }
    Vector3 GetDir(Transform p)
    {
        Vector3 res;
        res = new Vector3(p.transform.position.x, p.transform.position.y, p.transform.position.z) - new Vector3(transform.position.x, p.transform.position.y, transform.position.z);
        return res;
    }


}
