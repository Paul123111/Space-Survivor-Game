using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour {
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Slider masterLvl;
    [SerializeField] Slider musicLvl;
    [SerializeField] Slider sfxLvl;
    [SerializeField] Slider ambienceLvl;

    public void SetMasterVolume() {
        masterMixer.SetFloat("MasterVolume", masterLvl.value);
    }

    public void SetMusicVolume() {
        masterMixer.SetFloat("Music", musicLvl.value);
    }

    public void SetSfxVolume() {
        masterMixer.SetFloat("SFX", sfxLvl.value);
    }

    public void SetAmbienceVolume() {
        masterMixer.SetFloat("Ambience", ambienceLvl.value);
    }
    public void MuteMasterVolume() {
        masterLvl.value = -80;
        masterMixer.SetFloat("MasterVolume", masterLvl.value);
    }

    public void MuteSFXVolume() {
        sfxLvl.value = -80;
        masterMixer.SetFloat("SFX", sfxLvl.value);
    }

    public void MuteMusicVolume() {
        musicLvl.value = -80;
        masterMixer.SetFloat("Music", musicLvl.value);
    }

    public void MuteAmbienceVolume() {
        ambienceLvl.value = -80;
        masterMixer.SetFloat("Ambience", ambienceLvl.value);
    }

}
