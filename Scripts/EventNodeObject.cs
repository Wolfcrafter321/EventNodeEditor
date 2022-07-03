using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditor.Graphs;
using UnityEditor;
#endif

[System.Serializable, CreateAssetMenu]
public class EventNodeObject : ScriptableObject
{

    public List<EventNode> nodes;

    private void Reset()
    {
        Debug.Log("EventObject Reset.");
        nodes = new List<EventNode>();
        //nodes.Add(new EventNode());
    }

    public EventNode GetNextNode(EventNode node, int portID = 0)
    {

        string targGUID = node.outputGUID[portID];

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].guid == targGUID)
            {
                return nodes[i];
            }
        }

        return null;
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(EventNodeObject))]
public class EventNodeObjectInspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EventNodeObject targ = target as EventNodeObject;
        base.OnInspectorGUI();

        GUILayout.Space(30);
        if (GUILayout.Button("Open Window"))
        {
            var w = EditorWindow.GetWindow(typeof(EventNodeEditorWindow));
            w.titleContent = new GUIContent("EventNode Editor");
        }
    }

}
#endif