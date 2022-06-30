using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenDoor : MonoBehaviour
{
    public bool left;
    public bool rotateDoor;
    bool open=false;
    Vector3 originaPos;
    Quaternion originalRot;
    bool inOrOut;
    private void Start()
    {
        originaPos = transform.position;
        originalRot = transform.rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!open)
            {
                if (rotateDoor)
                {
                    if (other.transform.position.z > transform.position.z)
                        inOrOut = true;
                    else
                        inOrOut = false;
                    OpenRotate();
                }
                   
                else
                    Open();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (open && !rotateDoor)
            {

                Close();

            }
              
        }
    }

    void Open()
    {
        open = true;
        if(left)
        {
            transform.DOMove(transform.position+Vector3.right*2f, 1f);

        }
       else
        {
            transform.DOMove(transform.position + Vector3.right * -2f, 1f);

        }


    }

    void OpenRotate()
    {
        open = true;
        if (!inOrOut)
        {
            if (left)
                transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, -90), 1f).OnComplete(()=>Invoke("CloseRotate",2f));
            else

                transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, 90), 1f).OnComplete(() => Invoke("CloseRotate", 2f));


        }
        else
        {
            if (left)
                transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, 90), 1f).OnComplete(() => Invoke("CloseRotate", 2f));
            else

                transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, -90), 1f).OnComplete(() => Invoke("CloseRotate", 2f));
        }
    }

    void Close()
    {

        transform.DOMove(originaPos, 1f).OnComplete(() => open = false);


    }

    void CloseRotate()
    {
        transform.DORotate(originalRot.eulerAngles, 1f).OnComplete(() => open = false);
    }
}
