using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Transform camera;
    public Transform maincampos;
    public Transform optionscampos;
    public Transform mainmainmenu;
    public Transform hidemainmainmenupos;
    public Transform unhidemainmainmenupos;
    public static bool americamode;
    public static float sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        
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
        SceneManager.LoadSceneAsync(1);
        hitstab.Play();
    }
    
    public void ShowOptions()
    {
        camera.DOMove(optionscampos.position, 0.5f).SetEase(Ease.InOutCubic);
        camera.DORotateQuaternion(optionscampos.rotation, 0.5f).SetEase(Ease.InOutCubic);
        mainmainmenu.DOMove(hidemainmainmenupos.position, 0.5f).SetEase(Ease.InOutCubic);
        hitstab.Play();
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
