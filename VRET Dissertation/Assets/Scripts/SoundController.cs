using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioSource[] voiceovers = new AudioSource[10];
    public AudioSource backgroundMusic;

	// Use this for initialization
	void Start () {
        backgroundMusic.Play();
	}

    public void playVoiceover(int level) {
        Debug.Log("Playing VO " + level);
        if (level != 0) {
            voiceovers[level - 1].Stop();
        }
        voiceovers[level].Play();
    }
}
