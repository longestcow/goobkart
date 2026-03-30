using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{

    public Transform unmask1;
    public Transform mask1;
    public Transform masked1;
    public Transform unmask2;
    public Transform mask2;
    public Transform masked2;
    public Transform unmask3;
    public Transform mask3;
    public Transform masked3;
    public Transform unmask4;
    public Transform mask4;
    public Transform masked4;
    public Transform left;
    public Transform right;
    public AudioSource hoversfx;
    public AudioSource hitstab;
    public AudioSource hitstabecho;
    public AudioSource menumusic;
    public Transform camera;
    public Transform maincampos;
    public Transform optionscampos;
    public Transform mainmainmenu;
    public Transform optionsmenu;
    public Transform hidemainmainmenupos;
    public Transform unhidemainmainmenupos;
    public static bool americamode;
    public static float sensitivity = 0.5f;
    public static float musicvolume = 0.5f;
    public static float soundvolume = 0.5f;

    public Slider sens;
    public Slider music;
    public Slider sfx;
    public Dropdown resolution;
    public Dropdown fullscreen;
    public Toggle americamodetoggle;

    public audiovolumecontrol[] refreshthese;

    Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resolution.ClearOptions();
        List<string> options = new List<string>();
        int currentresolutionindex = 2;
        for (int i  = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentresolutionindex = i;
                print(i);
            }
            options.Add(resolutions[i].width + " x " + resolutions[i].height + (resolutions[i].width == 640 && resolutions[i].height == 480? " (PS2)":""));
        }
        resolution.AddOptions(options);
        resolution.value = currentresolutionindex;
        resolution.RefreshShownValue();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        masked1.transform.position = unmask1.transform.position;
        masked2.transform.position = unmask2.transform.position;
        masked3.transform.position = unmask3.transform.position;
        masked4.transform.position = unmask4.transform.position;
    }

    public void PlayGame()
    {
        menumusic.Stop();
        DontDestroyOnLoad(hitstabecho.gameObject.transform.parent.gameObject);
        hitstabecho.Play();
        SceneManager.LoadSceneAsync(1);
        
    }
        
    public void ShowOptions()
    {
        camera.DOMove(optionscampos.position, 0.5f).SetEase(Ease.InOutCubic);
        camera.DORotateQuaternion(optionscampos.rotation, 0.5f).SetEase(Ease.InOutCubic);
        mainmainmenu.DOMove(hidemainmainmenupos.position, 0.5f).SetEase(Ease.InOutCubic);
        optionsmenu.DOMove(unhidemainmainmenupos.position, 0.5f).SetEase(Ease.InOutCubic);
        hitstab.Play();

        sens.value = sensitivity;
        music.value = musicvolume;
        sfx.value = soundvolume;

        fullscreen.value = Screen.fullScreenMode == FullScreenMode.Windowed ? 0 : Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen? 1 : 2;

        americamodetoggle.isOn = !americamode;
    }
    public void HideOptions()
    {
        camera.DOMove(maincampos.position, 0.5f).SetEase(Ease.InOutCubic);
        camera.DORotateQuaternion(maincampos.rotation, 0.5f).SetEase(Ease.InOutCubic);
        optionsmenu.DOMove(hidemainmainmenupos.position, 0.5f).SetEase(Ease.InOutCubic);
        mainmainmenu.DOMove(unhidemainmainmenupos.position, 0.5f).SetEase(Ease.InOutCubic);
        hitstab.Play();

        sens.value = sensitivity;
        music.value = musicvolume;
        sfx.value = soundvolume;

        fullscreen.value = Screen.fullScreenMode == FullScreenMode.Windowed ? 0 : Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen ? 1 : 2;

        americamodetoggle.isOn = !americamode;
    }

    public void applysettings()
    {
        sensitivity = sens.value;
        musicvolume = music.value;
        soundvolume = sfx.value;

        FullScreenMode fsmode = fullscreen.value == 0 ? FullScreenMode.Windowed : fullscreen.value == 1 ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.FullScreenWindow;

        Screen.SetResolution(resolutions[resolution.value].width, resolutions[resolution.value].height, fsmode);

        americamode = !americamodetoggle.isOn;

        for (int i = 0; i < refreshthese.Length; i++)
        {
            refreshthese[i].RefreshVolume();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void movemask1()
    {
        mask1.DOMoveX(right.position.x, 0.3f).SetEase(Ease.OutCubic);
        hoversfx.Play();
    }
    public void unmovemask1()
    {

        mask1.DOMoveX(left.position.x, 0.3f).SetEase(Ease.InCubic);
    }
    public void movemask2()
    {
        mask2.DOMoveX(right.position.x, 0.3f).SetEase(Ease.OutCubic);
        hoversfx.Play();
    }
    public void unmovemask2()
    {

        mask2.DOMoveX(left.position.x, 0.3f).SetEase(Ease.InCubic);
    }
    public void movemask3()
    {
        mask3.DOMoveX(right.position.x, 0.3f).SetEase(Ease.OutCubic);
        hoversfx.Play();
    }
    public void unmovemask3()
    {

        mask3.DOMoveX(left.position.x, 0.3f).SetEase(Ease.InCubic);
    }
    public void movemask4()
    {
        mask4.DOMoveX(right.position.x, 0.3f).SetEase(Ease.OutCubic);
        hoversfx.Play();
    }
    public void unmovemask4()
    {
        mask4.DOMoveX(left.position.x, 0.3f).SetEase(Ease.InCubic);
    }


}
