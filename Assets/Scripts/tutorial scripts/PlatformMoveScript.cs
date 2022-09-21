using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlatformMoveScript : MonoBehaviour
{
    public bool Verticle;
    Vector3 originPos;
    private void Start()
    {
        originPos = transform.position;
        Invoke("StartMove", Random.Range(1f, 4f));

    }
    void StartMove()
    {
        Move(true);
    }
    void Move(bool dir)
    {
        if (Verticle)
        {

            if (dir)
                transform.DOMove(originPos + new Vector3(3f, 0f, 0f), 3f).OnComplete(() => Move(!dir));
            else
                transform.DOMove(originPos + new Vector3(-3f, 0f, 0f), 3f).OnComplete(() => Move(!dir));
        }
        else
        {
            if (dir)
                transform.DOMove(originPos + new Vector3(0f, 3f,0f), 3f).OnComplete(() => Move(!dir));
            else
                transform.DOMove(originPos + new Vector3(0f, -3f, 0f), 3f).OnComplete(() => Move(!dir));
        }
    }
}
