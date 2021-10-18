 using UnityEngine;
 using UnityEditor;
 using System.Linq;
 
 public class RenameChildren : EditorWindow {
     private static readonly Vector2Int size = new Vector2Int(250, 100);
     private string Name;
     [MenuItem("GameObject/Rename children")] public static void ShowWindow() {
         EditorWindow window = GetWindow<RenameChildren>();
         window.minSize = size;
         window.maxSize = size;
     }
     private void OnGUI() {
         if (GUILayout.Button("Rename children")) {
             GameObject[] selectedObjects = Selection.gameObjects;
             for (int objectI = 0; objectI < selectedObjects.Length; objectI++) {
                Transform selectedObjectT = selectedObjects[objectI].transform;
                for (int childI = 0; childI < selectedObjectT.childCount; childI++)
                {
                    Name = selectedObjectT.GetChild(childI).name;
                    if (Name.Contains('(')) 
                    {
                        selectedObjectT.GetChild(childI).name = Name.Remove(Name.IndexOf('(') - 1, Name.IndexOf(')') - Name.IndexOf('(') + 2);
                    }
                }
             }
         }
     }
 }
