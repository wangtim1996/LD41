using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;
    private List<AudioSource> audioList;
    public int poolSize = 30;

    // Use this for initialization
    void Awake () {
        audioList = new List<AudioSource>();
		if(Instance != null)
        {
            Debug.LogWarning("AudioManager singleton fail");
        }
        Instance = this;
        for(int i = 0; i < poolSize; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = false;
            audioList.Add(src);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    AudioSource GetAudioSource()
    {
        for(int i = 0; i < poolSize; i++)
        {
            if (!audioList[i].isPlaying)
            {
                return audioList[i];
            }
        }
        return null;
    }

    public void PlayClip(AudioClip clip)
    {
        AudioSource src = GetAudioSource();
        if(src != null)
        {
            src.clip = clip;
            src.Play();
            src.pitch = Random.Range(0.95f, 1.05f);
        }
    }


}
