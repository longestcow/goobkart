using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class mainmenutoplaytransition : MonoBehaviour
{
    public Transform theblackthing;
    public Transform onscreen;
    public Transform offscreen;
    public bool playscene;

    // Start is called before the first frame update
    void Start()
    {
        if (playscene) EndTrans();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartGame()
    {
        theblackthing.DOMoveY(onscreen.position.y, 0.5f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
    public void EndTrans()
    {
        theblackthing.DOMoveY(offscreen.position.y, 0.5f).SetEase(Ease.OutCubic);
    }
    public void StartTrans()
    {
        theblackthing.DOMoveY(onscreen.position.y, 1f).SetEase(Ease.OutCubic);
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
