using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SkipScreenScreenView : MonoBehaviour
{
    [SerializeField] Button back, chicken, bear,warrior;

    string url;

    private void Start()
    {
        ObserveBtns();
    }

    public void ObserveBtns()
    {
        back.OnClickAsObservable()
            .Do(_ => Back())
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);
        chicken.OnClickAsObservable()
            .Do(_ => LoadChicken())
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this); 
        bear.OnClickAsObservable()
            .Do(_ => LoadBear())
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);
        warrior.OnClickAsObservable()
            .Do(_ => LoadWarrior())
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);
    }


    void Back()
    {
        gameObject.SetActive(false);
    }

    void LoadChicken()
    {
        url = "http://play.cryptofightclub.io/chicken-run";
        LoadGame();
    }
    void LoadBear()
    {
        url = "http://play.cryptofightclub.io/fight-the-bear";
        LoadGame();
    }
    void LoadWarrior()
    {
        url = "http://play.cryptofightclub.io/warrior";
        LoadGame();
    }
    void LoadGame()
    {
        Application.ExternalEval("window.open('" + url + "','_self')");
        Application.Quit();
    }
}

