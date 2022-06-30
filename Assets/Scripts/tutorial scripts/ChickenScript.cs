using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenScript : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    int range;
    [SerializeField]
    float timeToWander;
    NavMeshAgent nav;
    [SerializeField]
    AudioClip sound1, sound2;
    AudioSource aS;
    float timeLeftToWander;
    GameObject player;
    bool played;
    float timefornextSound;
    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        aS = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        Wander();
    }
    void Update()
    {
        if ((transform.position - nav.destination).magnitude < 3 || timeLeftToWander <= 0)
        {
            Wander();

        }
        else
        {
            timeLeftToWander -= Time.deltaTime;
        }
        if (player != null)
        {
            if ((transform.position - player.transform.position).magnitude < 10)
            {
                if (timefornextSound <= 0)
                {
                    if (!gameplayView.instance.GetSFXMuted())
                    {
                        aS.clip = sound1;
                        aS.Play();
                    }

                    timefornextSound = Random.Range(2, 6);
                }
                else
                {
                    timefornextSound -= Time.deltaTime;
                }
            }
            else
            {
                timefornextSound = 0;
            }
        }
      

    }
    Vector3 GetRandomLocation(int maxDistance)
    {
       
        Vector3 randomPos = Random.insideUnitSphere * Random.Range(10, maxDistance) + transform.position;

        NavMeshHit hit;
       
        bool found = NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);
        if (!found)
        {
            return GetRandomLocation(range);
        }
        else
            return hit.position;
    }


    void Wander()
    {
        timeLeftToWander = timeToWander;
        NavMeshPath path = new NavMeshPath();
        nav.CalculatePath(GetRandomLocation(range), path);
        nav.SetPath(path);


    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (!gameplayView.instance.GetSFXMuted())
            {
                aS.clip = sound2;
                aS.Play();
            }
            HideChicken();
            other.GetComponent<TutorialPlayerScript>().DisableMovement();
            gameplayView.instance.GetComponent<uiView>().ActivateNectGame(games.chicken);
        }
    }

    void HideChicken()
    {
        GetComponent<BoxCollider>().enabled = false;
        foreach(Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
        Invoke("ShowChicken", 5f);
    }
    public void ShowChicken()
    {
        GetComponent<BoxCollider>().enabled = true;
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }
    }
}
