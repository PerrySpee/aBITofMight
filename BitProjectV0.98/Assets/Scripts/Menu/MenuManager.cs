using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{

    enum MenuState
    {
        Title,
        StartMenu,
        Instruction,
        Options
    }

    MenuState menuState;

    enum StartState
    {
        Start,
        Instruction,
        Options,
        Exit

    }

    enum OptionsState
    {
        SFX,
        Music,
        Pixelate
    }

    OptionsState optionsState;
    StartState startState;
    public Canvas title;
    public Canvas startMenu;
    public Canvas instruction;
    public Canvas options;

    public Image startMenuSelector;
    public Image optionMenuSelector;

    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider pixelateSlider;

    public AudioMixer masterAudioMixer;

    [RangeAttribute(3,8)]public int pixelateAmount;
    public int sfxVolume;
    public int musicVolume;

    public Text pixelateAmountText;
    public Text musicAmountText;
    public Text sfxAmountText;

    private bool selectionVertLock;
    private bool selectionHorLock;

    public AudioClip testSound;


    void Start()
    {
        SetFirstTimeOptions();

        pixelateAmount = (int)PlayerPrefs.GetFloat("PixelSize");
        pixelateSlider.value = pixelateAmount;
        pixelateAmountText.text = pixelateAmount.ToString();
        sfxVolume = (int)PlayerPrefs.GetFloat("sfxVolume");
        sfxSlider.value = sfxVolume;
        sfxAmountText.text = sfxVolume.ToString() + "%";
        musicVolume = (int)PlayerPrefs.GetFloat("musicVolume");
        musicSlider.value = musicVolume;
        musicAmountText.text = musicVolume.ToString() + "%";
        menuState = MenuState.Title;

       if( !PlayerPrefs.HasKey("sfxVolume"))
       {
           SetSFXVolume();
       }
    }

    void Update()
    {
        ShowMenu();
    }

    void ShowMenu()
    {
        switch (menuState)
        {
            case MenuState.Title:
                title.gameObject.SetActive(true);
                startMenu.gameObject.SetActive(false);
                instruction.gameObject.SetActive(false);
                options.gameObject.SetActive(false);

                if (Input.GetButton("StartP1"))
                {
                    menuState = MenuState.StartMenu;
                }
                break;

            case MenuState.StartMenu:
                title.gameObject.SetActive(false);
                startMenu.gameObject.SetActive(true);
                instruction.gameObject.SetActive(false);
                options.gameObject.SetActive(false);

                switch (startState)
                {
                    case StartState.Start:
                        if (Input.GetButton("JumpP1"))
                            StartCoroutine(StartGame());
                        break;

                    case StartState.Instruction:
                        if (Input.GetButton("JumpP1"))
                            menuState = MenuState.Instruction;
                        break;

                    case StartState.Options:
                        if (Input.GetButton("JumpP1"))
                            menuState = MenuState.Options;
                        break;

                    case StartState.Exit:
                        if (Input.GetButton("JumpP1"))
                            Application.Quit();
                        break;
                }


                if (Input.GetAxis("VertP1") > 0.3f && startState != StartState.Exit && selectionVertLock == true)
                {
                    startState++;
                    selectionVertLock = false;
                    startMenuSelector.rectTransform.position = new Vector3(startMenuSelector.rectTransform.position.x, startMenuSelector.rectTransform.position.y - 70, startMenuSelector.rectTransform.position.z);
                }
                if (Input.GetAxis("VertP1") < -0.3f && startState != StartState.Start && selectionVertLock == true)
                {
                    startState--;
                    selectionVertLock = false;
                    startMenuSelector.rectTransform.position = new Vector3(startMenuSelector.rectTransform.position.x, startMenuSelector.rectTransform.position.y + 70, startMenuSelector.rectTransform.position.z);
                }

                if (Input.GetAxis("VertP1") == 0 && selectionVertLock == false)
                {
                    selectionVertLock = true;
                }

                Debug.Log(startState);

                break;

            case MenuState.Instruction:
                title.gameObject.SetActive(false);
                startMenu.gameObject.SetActive(false);
                instruction.gameObject.SetActive(true);
                options.gameObject.SetActive(false);


                if (Input.GetButton("Atk3P1"))
                {
                    menuState = MenuState.StartMenu;
                }
                break;

            case MenuState.Options:
                title.gameObject.SetActive(false);
                startMenu.gameObject.SetActive(false);
                instruction.gameObject.SetActive(false);
                options.gameObject.SetActive(true);
                switch (optionsState)
                {
                    case OptionsState.SFX:
                        if (Input.GetAxis("HorP1") > 0.3f && selectionHorLock == true)
                        {
                            selectionHorLock = false;
                            sfxSlider.value += 5;
                            sfxAmountText.text = (sfxSlider.value / 80 * 100).ToString("F0") +  "%";

                            SetSFXVolume();
                            sfxSlider.GetComponent<AudioSource>().PlayOneShot(testSound);
                        }
                        else if (Input.GetAxis("HorP1") < -0.3f && selectionHorLock == true)
                        {
                            selectionHorLock = false;
                            sfxSlider.value -= 5;
                            sfxAmountText.text = (sfxSlider.value / 80 * 100).ToString("F0") + "%";
                            SetSFXVolume();
                            sfxSlider.GetComponent<AudioSource>().PlayOneShot(testSound);
                        }
                        break;

                    case OptionsState.Music:
                        if (Input.GetAxis("HorP1") > 0.3f)
                        {
                            musicSlider.value++;
                            musicAmountText.text = (musicSlider.value / 80 * 100).ToString("F0") + "%";
                            SetMusicVolume();
                        }
                        else if (Input.GetAxis("HorP1") < -0.3f )
                        {
                            musicSlider.value--;
                            musicAmountText.text = (musicSlider.value / 80 * 100).ToString("F0") + "%";
                            SetMusicVolume();
                        }
                        break;

                    case OptionsState.Pixelate:
                        if (Input.GetAxis("HorP1") > 0.3f && selectionHorLock == true)
                        {
                            selectionHorLock = false;
                            pixelateSlider.value++;
                            pixelateAmount = (int)pixelateSlider.value;
                            pixelateAmountText.text = pixelateSlider.value.ToString();
                            PlayerPrefs.SetFloat("PixelSize", pixelateSlider.value);
                        }
                        else if (Input.GetAxis("HorP1") < -0.3f && selectionHorLock == true)
                        {
                            selectionHorLock = false;
                            pixelateSlider.value--;
                            pixelateAmount = (int)pixelateSlider.value;
                            pixelateAmountText.text = pixelateSlider.value.ToString();
                            PlayerPrefs.SetFloat("PixelSize", pixelateSlider.value);
                        }
                        break;
                }

                if (Input.GetAxis("HorP1") == 0 && selectionHorLock == false)
                {
                    selectionHorLock = true;
                }

                if (Input.GetAxis("VertP1") > 0.3f && optionsState != OptionsState.Pixelate && selectionVertLock == true)
                {
                    optionsState++;
                    selectionVertLock = false;
                    optionMenuSelector.rectTransform.position = new Vector3(optionMenuSelector.rectTransform.position.x, optionMenuSelector.rectTransform.position.y - 70, optionMenuSelector.rectTransform.position.z);
                }
                if (Input.GetAxis("VertP1") < -0.3f && optionsState != OptionsState.SFX && selectionVertLock == true)
                {
                    optionsState--;
                    selectionVertLock = false;
                    optionMenuSelector.rectTransform.position = new Vector3(optionMenuSelector.rectTransform.position.x, optionMenuSelector.rectTransform.position.y + 70, optionMenuSelector.rectTransform.position.z);
                }

                if (Input.GetAxis("VertP1") == 0 && selectionVertLock == false)
                {
                    selectionVertLock = true;
                }


                if (Input.GetButton("Atk3P1"))
                {
                    Application.LoadLevel(Application.loadedLevel);
                    menuState = MenuState.StartMenu;
                }
                break;
        }
    }

    public void SetSFXVolume()
    {
        masterAudioMixer.SetFloat("sfxVol", sfxSlider.value - 80);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }

    public void SetMusicVolume()
    {
        masterAudioMixer.SetFloat("musicVol", musicSlider.value - 80);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }

    IEnumerator StartGame()
    {
        AsyncOperation async = Application.LoadLevelAsync(1);
        yield return async;
    }

    void SetFirstTimeOptions()
    {
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            SetSFXVolume();
            Debug.Log("sfx set");
        }
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            SetMusicVolume();
            Debug.Log("music set");
        }
        if (!PlayerPrefs.HasKey("PixelSize"))
        {
            PlayerPrefs.SetFloat("PixelSize", 3);
            Debug.Log("pixel set");
        }
    }
}
