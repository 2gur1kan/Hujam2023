#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(SoundDataBase))]
public class SoundEnumGenerator:Editor
{
    SoundDataBase soundListSO;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        soundListSO = (SoundDataBase)target;

        if (GUILayout.Button("Update Sound List"))
        {
            UpdateList();
            UpdateEnum();
        }
    }

    public void UpdateList()
    {      
        string[] enumValues = new string[soundListSO.soundList.Count];
        for (int i = 0; i < soundListSO.soundList.Count; i++)
        {
            enumValues[i] = soundListSO.soundList[i].soundName;
            enumValues[i] = enumValues[i].Replace('-','_');
            enumValues[i] = enumValues[i].Replace('i', 'ı');
            enumValues[i] = enumValues[i].ToUpper();

            soundListSO.soundList[i].soundName = enumValues[i];
            Debug.Log(soundListSO.soundList[i].soundName);

            AssetDatabase.Refresh();
        }
    }

    public void UpdateEnum()
    {
        // Enum file path
        string enumFilePath = "Assets/Scripts/Sound/SoundEnum.cs";

        string[] enumValues = new string[soundListSO.soundList.Count];
        for (int i = 0; i < soundListSO.soundList.Count; i++)
        {
            enumValues[i] = soundListSO.soundList[i].soundName;
        }

        string enumContent = "public enum SoundEnum\n{\n";
        for (int i = 0; i < enumValues.Length; i++)
        {
            enumContent += "    " + enumValues[i] + ",\n";
        }
        enumContent += "}";

        System.IO.File.WriteAllText(enumFilePath, enumContent);

        AssetDatabase.Refresh();
    }
}
#endif