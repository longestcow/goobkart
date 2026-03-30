using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiovolumecontrol : MonoBehaviour
{

    AudioSource source;
    float defaultvolume;
    public bool music;
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        defaultvolume = source.volume;
        RefreshVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshVolume()
    {
        source.volume = defaultvolume * (music?MainMenu.musicvolume:MainMenu.soundvolume);
    }
}
