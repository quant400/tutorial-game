using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ToOtherGame : MonoBehaviour
{
    [SerializeField]
    games toWhich;
    string Url;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            switch(toWhich)
            {
                case games.chicken:
                    Url = "http://staging-play.cryptofightclub.io/chicken-run";
                    break;
                case games.bear:
                    Url = "http://staging-play.cryptofightclub.io/fight-the-bear";
                    break;

            }
            Application.ExternalEval("window.open('" + Url + "','_self')");
            Application.Quit();
        }
    }
}
