using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIView : MonoBehaviour
{
    public static GameUIView instance;
    [SerializeField]
    AudioClip Abient , Music;
    [SerializeField]
    AudioSource audioSource;
    public bool sfxMuted =false;
    public bool musicMuted =false;
    [SerializeField]
    Button muteSfx, muteMusic,settings, closeSettings ;
    Image sfxButtonImage, musicButtonImage;

    float defaultMusicVol;

    Sprite regularImage;
    [SerializeField]
    Sprite disableImage;

    [SerializeField]
    GameObject SettingsPanel;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
          
        }
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
       
    }
    private void Start()
    {
        ObserveGameObverBtns();

        defaultMusicVol = audioSource.volume;
        sfxButtonImage = muteSfx.GetComponent<Image>();
        musicButtonImage = muteMusic.GetComponent<Image>();
        regularImage = sfxButtonImage.sprite;
        if (PlayerPrefs.HasKey("SFX"))
        {
            if (PlayerPrefs.GetString("SFX") == "off")
            {
                sfxMuted = true;
                MuteSFX();
            }

        }
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetString("Music") == "off")
            {
                musicMuted = true;
                MuteMusic();
            }
        }

       
    }

    public void ObserveGameObverBtns()
    {

        muteSfx.OnClickAsObservable()
            .Do(_ => MuteSFX())
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);

        muteMusic.OnClickAsObservable()
            .Do(_ => MuteMusic())
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);

        settings.OnClickAsObservable()
            .Do(_ => OpenSettings())
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);

        closeSettings.OnClickAsObservable()
            .Do(_ => CloseSettings())
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);


    }



   

    public void MuteSFX()
    {
        if (PlayerPrefs.HasKey("SFX") && PlayerPrefs.GetString("SFX") == "off")
        {
            //SFX.volume = defaultMusicVol;
            sfxButtonImage.sprite = regularImage;//sfxButtonImage.color = new Color(1f, 1f, 1f, 1f);
            sfxButtonImage.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f); 
            PlayerPrefs.SetString("SFX", "on");
            GameUIView.instance.sfxMuted = false;
        }
        else
        {
            sfxButtonImage.sprite = disableImage;//sfxButtonImage.color = new Color(1f, 1f, 1f, 0.5f);
            sfxButtonImage.transform.GetChild(0).GetComponent<Image>().color = new Color(0.9450981f, 0.1215686f, 0.172549f, 1f);
            //SFX.volume = 0;
            PlayerPrefs.SetString("SFX", "off");
            GameUIView.instance.sfxMuted = true;
        }

    }

    public void MuteMusic()
    {
        if (audioSource.volume == 0)
        {
            musicButtonImage.sprite = regularImage; //musicButtonImage.color = new Color(1f, 1f, 1f, 1f);
            musicButtonImage.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            audioSource.volume = defaultMusicVol;
            PlayerPrefs.SetString("Music", "on");
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            musicButtonImage.sprite = disableImage;//musicButtonImage.color = new Color(1, 1, 1, 0.5f);
            musicButtonImage.transform.GetChild(0).GetComponent<Image>().color = new Color(0.9450981f, 0.1215686f, 0.172549f, 1f);
            audioSource.volume = 0;
            PlayerPrefs.SetString("Music", "off");
        }
    }

    void OpenSettings()
    {
        SettingsPanel.SetActive(true);
        Time.timeScale = 0;
    }


    void CloseSettings()
    {
        SettingsPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
