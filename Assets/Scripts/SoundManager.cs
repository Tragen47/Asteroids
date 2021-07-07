using UnityEngine;
using System.Collections.Generic;
using System.Linq;

class SoundManager : MonoBehaviour
{
    static List<AudioSource> AudioSource;

    void Awake() => AudioSource = GetComponents<AudioSource>().ToList();

    public static void PlaySound(string name) => AudioSource.Find(source => source.clip.name == name).Play();
}