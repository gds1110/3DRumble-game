using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UI_MainCardManager))]
public class EndGameButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Win Game"))
        {
            Managers.Game.SetGameEnding(true);
        }
        if(GUILayout.Button("Lose Game"))
        {
            Managers.Game.SetGameEnding(false);
        }
    }
}
