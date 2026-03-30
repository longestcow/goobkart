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
        theblackthing.DOMoveY(onscreen.position.y, 0.1f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(1);
    }
    public void EndTrans()
    {
        theblackthing.DOMoveY(offscreen.position.y, 0.1f).SetEase(Ease.OutCubic);
    }

    public IEnumerator EndGame()
    {
        theblackthing.DOMoveY(onscreen.position.y, 0.1f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(0);
    }

}
