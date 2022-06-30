using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using StarterAssets;
public enum TutorailStage
{
    mouse,
    movement,
    jump,
    sprint,
    punch,
    kick,
    block,
    Throw,
    none

}
public class TutorialControler : MonoBehaviour
{
    [SerializeField]
    Sprite mouse, arrow, space, shift, attack, swap, block, pickUp;
    [SerializeField]
    TMP_Text instructions;
    [SerializeField]
    Image keys;
    [SerializeField]
    GameObject panel, thingsToDisableAfterMovement;
    TutorailStage stage;
    bool taskDone;
    TutorialPlayerScript player;
    public bool fightTutorialDone = false;

    Transform camToFace;

    TutorialNpc tNCP;

    [SerializeField]
    GameObject toOtherGames;
    
    [SerializeField]
    Transform throwableObjects;

    public bool ObjectPickedUp = false;
    bool hit=false;
    private void Start()
    {
        stage = TutorailStage.none;
        Invoke("MouseTutorial", 1f);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TutorialPlayerScript>();
        camToFace = player.transform.GetChild(0);
        tNCP = GetComponentInParent<TutorialNpc>();
        player.GetComponent<ThirdPersonController>().enabled = false;
        toOtherGames.SetActive(false);
    }
    #region TutorialStarters

    void MouseTutorial()
    {
        player.GetComponent<ThirdPersonController>().enabled = true;
        stage = TutorailStage.mouse;
        taskDone = false;
        instructions.text = ("Use the  mouse to look around").ToUpper();
        keys.sprite = mouse;
        panel.transform.DOScale(Vector3.one, 1f);
    }
    void MovementTutorial()
    {
        stage = TutorailStage.movement;
        taskDone=false;
        instructions.text =("Use the A, W, S, D keys to move").ToUpper();
        keys.sprite = arrow;
        panel.transform.DOScale(Vector3.one, 1f);

    }

    void JumpTutorial()
    {
        stage = TutorailStage.jump;
        taskDone = false;
        instructions.text = ("Use the Space Bar to jump").ToUpper();
        keys.sprite = space;
        panel.transform.DOScale(Vector3.one, 1f);

    } 
    
    void SprintTutorial()
    {
        stage = TutorailStage.sprint;
        taskDone = false;
        instructions.text = ("Use the Shift Key to run").ToUpper();
        keys.sprite = shift;
        panel.transform.DOScale(Vector3.one, 1f);

    }

    void PunchTutorial()
    {
        stage = TutorailStage.punch;
        taskDone = false;
        instructions.text = ("use the left mouse button to attack the punching bag").ToUpper();
        keys.sprite = attack;
        panel.transform.DOScale(Vector3.one, 1f);

    }

    void SwapTutorial()
    {
        stage = TutorailStage.kick;
        taskDone = false;
        instructions.text = ("use the Q key to switch to kick and attack the punching bag").ToUpper();
        keys.sprite = swap;
        panel.transform.DOScale(Vector3.one, 1f);

    }

    void BlockTutorial()
    {
        stage = TutorailStage.block;
        taskDone = false;
        instructions.text = ("use the right mouse button to block attacks ").ToUpper();
        keys.sprite = block;
        panel.transform.DOScale(Vector3.one, 1f);
    }

    void ThrowTutorial()
    {
        stage = TutorailStage.Throw;
        taskDone = false;
        instructions.text = ("Some objects can be picked up  with the \"E\"key and thrown with the attack button").ToUpper();
        keys.sprite = pickUp;
        throwableObjects.gameObject.SetActive(true);
        panel.transform.DOScale(Vector3.one, 1f);

    }

    void EnterGym()
    {
        
        instructions.text = ("Now head into the gym").ToUpper();
        keys.gameObject.SetActive(false);
        panel.transform.DOScale(Vector3.one, 1f);
        Invoke("EndText", 2f);
        tNCP.Invoke("InsideGym", 2f);

    }
    public void EnterRing()
    {

        instructions.text = ("Go into the circle in the ring to learn how to fight").ToUpper();
        keys.gameObject.SetActive(false);
        panel.transform.DOScale(Vector3.one, 1f);
        Invoke("EndText", 2f);

    }

    void LeaveGame()
    {
        instructions.text = ("Now head head outside and explore the metaverse").ToUpper();
        keys.gameObject.SetActive(false);
        panel.transform.DOScale(Vector3.one, 1f);
        Invoke("EndText", 2f);
        toOtherGames.SetActive(true);

    }

    public void StartFightTutorial()
    {
        tNCP.Fighting();
        PunchTutorial();
    }

    #endregion TutorialStarters

    private void Update()
    {
        if (stage != TutorailStage.none && !taskDone)
        {
            CheckTaskDone();
        }
    }

    public void GetHit()
    {
        taskDone = false;
        hit = true;
        instructions.text = ("Hey! That hurts.").ToUpper();
        keys.gameObject.SetActive(false);
        panel.transform.DOScale(Vector3.one, 1f);
        Invoke("TurnOffTutorial", 1.5f);
    }
    void EndText()
    {
        panel.transform.DOScale(Vector3.zero, 1f).OnComplete(()=>keys.gameObject.SetActive(true));
        if (hit == true)
        {
            Debug.Log(2);
            hit = false;
            if (stage == TutorailStage.Throw)
            {
                Invoke("ThrowTutorial",1f);
            }
            else
            {
                Invoke("LeaveGame",1f);
            }
        }
    }
    void TurnOffTutorial()
    {
        panel.transform.DOScale(Vector3.zero, 1f).OnComplete(() => keys.color = Color.white);
        if (hit)
        {
            Invoke("EndText", 2f);
        }
        else if (stage == TutorailStage.mouse)
        {
            stage = TutorailStage.none;
            Invoke("MovementTutorial", 2f);
        }
        else if (stage == TutorailStage.movement)
        {
            tNCP.MoveToFront();
            stage = TutorailStage.none;
            Invoke("JumpTutorial", 2f);
        }
        else if(stage == TutorailStage.jump)
        {
            tNCP.MoveToFront();
            stage = TutorailStage.none;
            Invoke("SprintTutorial", 2f);
        }
        else if(stage == TutorailStage.sprint)
        {
            tNCP.MoveToFront();
            stage = TutorailStage.none;
            thingsToDisableAfterMovement.SetActive(false);
            EnterGym();
            
        }

        else if (stage == TutorailStage.punch)
        {
            stage = TutorailStage.none;
            Invoke("SwapTutorial", 2f);
        }

        else if (stage == TutorailStage.kick)
        {
            stage = TutorailStage.none;
            Invoke("BlockTutorial", 2f);

        }
        else if (stage == TutorailStage.block)
        {
            stage = TutorailStage.none;
            fightTutorialDone=true;
            player.DisableFightTutorial();
            Invoke("ThrowTutorial", 2f);

        }

        else if (stage == TutorailStage.Throw)
        {
            stage = TutorailStage.none;
            Invoke("LeaveGame", 2f);

        }
        
    }

    void CheckTaskDone()
    {
        if (stage == TutorailStage.mouse)
        {
            if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0 || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0)
            {
                keys.color = Color.green;
                taskDone = true;
                Invoke("TurnOffTutorial", 3f);

            }
        }
        else if (stage == TutorailStage.movement)
        {
            if(Mathf.Abs( Input.GetAxis("Horizontal")) > 0 ||Mathf.Abs( Input.GetAxis("Vertical")) >0)
            {
                keys.color = Color.green;
                taskDone = true;
                Invoke("TurnOffTutorial", 3f);

            }
        }
        else if (stage == TutorailStage.jump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                keys.color = Color.green;
                taskDone = true;
                Invoke("TurnOffTutorial", 3f);
            }
        }
        else if (stage == TutorailStage.sprint)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                keys.color = Color.green;
                taskDone = true;
                Invoke("TurnOffTutorial", 3f);
            }
        }

        else if (stage == TutorailStage.punch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                keys.color = Color.green;
                taskDone = true;
                Invoke("TurnOffTutorial", 3f);
            }
        }

        else if (stage == TutorailStage.kick)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                keys.color = Color.green;
                taskDone = true;
                Invoke("TurnOffTutorial", 3f);
            }
        }

        else if (stage == TutorailStage.block)
        {
            if (Input.GetMouseButtonDown(1))
            {
                keys.color = Color.green;
                taskDone = true;
                Invoke("TurnOffTutorial", 3f);
            }
        }

        else if (stage == TutorailStage.Throw)
        {
            if (ObjectPickedUp)
            {
                keys.color = Color.green;
                taskDone = true;
                Invoke("TurnOffTutorial", 3f);
            }
        }

    }

    private void LateUpdate()
    {
        //transform.LookAt(camToFace);
        this.transform.LookAt(new Vector3(camToFace.position.x, this.transform.position.y, camToFace.position.z));
    }
}
