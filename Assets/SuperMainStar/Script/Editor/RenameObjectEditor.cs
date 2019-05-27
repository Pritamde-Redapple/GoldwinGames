using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(RenameObject))]
public class RenameObjectEditor : Editor {

    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        RenameObject script = (RenameObject)target;

        if (GUILayout.Button("Create Child"))
        {
            script.CreateChild();
        }
        if(GUILayout.Button("Rename")){
            script.RenameAllChild();
        }

        if (GUILayout.Button("Set Hover"))
        {
            script.SetHoverImage();
        }
    }
}
