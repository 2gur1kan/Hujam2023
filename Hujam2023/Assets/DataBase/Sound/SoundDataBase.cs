using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundList", menuName = "Create Sound List")]
public class SoundDataBase : ScriptableObject
{
    public List<Sound> soundList;
}

[Serializable]
public class Sound
{
    public string soundName;
    public AudioClip sound;
}