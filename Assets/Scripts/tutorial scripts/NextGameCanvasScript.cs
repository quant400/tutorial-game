using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public enum games
{
    chicken,
    bear
}
public class NextGameCanvasScript : MonoBehaviour
{
    [SerializeField]
    Transform panel;
    [SerializeField]
    TMP_Text txt;
    [SerializeField]
    Button yes, no;

    string url;

    private void Start()
    {
        panel.localScale = Vector3.zero;
        ObserveBtns();
    }
    public void EnablePanel(games g)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        //replace links later
        if (g == games.chicken)
        {
            SetText("Goto Chickedn Run?");
            SetLink("http://staging-play.cryptofightclub.io/chicken-run");
        }

        else if (g == games.bear)
        {
            SetText("Goto Fight The Bear?");
            SetLink("http://staging-play.cryptofightclub.io/fight-the-bear");
        }
        panel.DOScale(Vector3.one, 1f).OnComplete(() => Time.timeScale = 0);
        player.GetComponent<StarterAssets.StarterAssetsInputs>().SetCursorLock(false);
    }
    public void DisablePanel()
    {
        Time.timeScale = 1;
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<TutorialPlayerScript>().EnableMovement();
        StartCoroutine( player.GetComponent<StarterAssets.ThirdPersonController>().LockCursorAfter(0));
        panel.DOScale(Vector3.zero, 1f);
    }
    public void SetText(string text)
    {
        txt.text = text.ToUpper();
    }

    public void SetLink(string link)
    {
        url = link;
    }

    public void Yes()
    {
        Debug.Log(url);
        Application.ExternalEval("window.open('" + url + "','_self')");
        Application.Quit();

    }

    public void No()
    {
        DisablePanel();
    }


    public void ObserveBtns()
    {
        yes.OnClickAsObservable()
            .Do(_ => Yes())
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);

        no.OnClickAsObservable()
            .Do(_ => No())
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);
    }

}
