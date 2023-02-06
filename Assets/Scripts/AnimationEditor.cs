#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class AnimationEditor : EditorWindow {
    private float indent = 20f;
    
    private string findString;
    private string replaceString;

    private string[] replacementMethods = new string[] { "Individual", "Folder" };
    private int currentReplacementMethod = 1;
    private string folderPath = "";
    private AnimationClip[] selectedAnims = new AnimationClip[0];

    [MenuItem("Game Tools/Animation Editor")]
    static void Init() {
        AnimationEditor window = (AnimationEditor)CreateInstance(typeof(AnimationEditor));
        window.Show();
    }

    void OnGUI() {
        SetupGUI();
        RenderReplacementMethod();
        ReplaceGUI();
    }

    void SetupGUI() {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(indent);
        EditorGUILayout.LabelField("Replacement Method:", GUILayout.Width(200f));
        currentReplacementMethod = EditorGUILayout.Popup(currentReplacementMethod, replacementMethods, GUILayout.Width(200f));
        EditorGUILayout.EndHorizontal();
    }

    void RenderReplacementMethod() {
        switch(currentReplacementMethod) {
            case 0: // Individual
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(indent);
                EditorGUILayout.LabelField("Selected Clip:", GUILayout.Width(200f));
                if(selectedAnims.Length != 1) { selectedAnims = new AnimationClip[1]; }
                selectedAnims[0] = EditorGUILayout.ObjectField(selectedAnims[0], typeof(AnimationClip), false, GUILayout.Width(200f)) as AnimationClip;
                EditorGUILayout.EndHorizontal();
                break;
            case 1: // Folder
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(indent);
                EditorGUILayout.LabelField("Folder Path: Assets/", GUILayout.Width(120f));
                folderPath = EditorGUILayout.TextField(folderPath, GUILayout.Width(200f));
                if(GUILayout.Button("Search", GUILayout.Width(100f))) {
                    selectedAnims = FindAnimClips(folderPath);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(indent);
                if (selectedAnims.Length > 0 && selectedAnims[0] != null) {
                    EditorGUILayout.LabelField(selectedAnims.Length + " Clips Selected", GUILayout.Width(200f));
                }
                else {
                    EditorGUILayout.LabelField("No Clips Selected", GUILayout.Width(200f));
                }
                EditorGUILayout.EndHorizontal();
                break;
            default:
                Debug.Log("Error: Invalid Replacement Method");
                break;
        }
    }

    AnimationClip[] FindAnimClips(string path) {
        try {
            path = "Assets/" + path;
            string[] files = Directory.GetFiles(path, "*.anim", SearchOption.AllDirectories);
            AnimationClip[] animationClips = new AnimationClip[files.Length];
            for (int i = 0; i < animationClips.Length; i++) {
                animationClips[i] = AssetDatabase.LoadAssetAtPath<AnimationClip>(files[i]);
            }
            return animationClips;
        } catch (Exception) {
            return new AnimationClip[0];
        }
    }

    void ReplaceGUI() {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(indent);
        EditorGUILayout.LabelField("Old Term:", GUILayout.Width(70f));
        findString = EditorGUILayout.TextField(findString, GUILayout.Width(200f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(indent);
        EditorGUILayout.LabelField("New Term:", GUILayout.Width(70f));
        replaceString = EditorGUILayout.TextField(replaceString, GUILayout.Width(200f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(indent);
        if (GUILayout.Button("Replace", GUILayout.Width(100f))) { ReplaceAnimationPaths(); }
        EditorGUILayout.EndHorizontal();
    }

    void ReplaceAnimationPaths() {
        for(int animIndex = 0; animIndex < selectedAnims.Length; animIndex++) {
            EditorCurveBinding[] curveData = AnimationUtility.GetCurveBindings(selectedAnims[animIndex]);
            for (int i = 0; i < curveData.Length; i++) {
                string newPath = curveData[i].path.Replace(findString, replaceString);
                AnimationCurve curve = AnimationUtility.GetEditorCurve(selectedAnims[animIndex], curveData[i]);
                AnimationUtility.SetEditorCurve(selectedAnims[animIndex], curveData[i], null);
                curveData[i].path = newPath;
                AnimationUtility.SetEditorCurve(selectedAnims[animIndex], curveData[i], curve);
            }
        }
    }
}

#endif