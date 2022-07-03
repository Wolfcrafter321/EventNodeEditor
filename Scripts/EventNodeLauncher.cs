using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditor.Graphs;
using UnityEditor;
#endif

public class EventNodeLauncher : MonoBehaviour
{


    [Header("The Event")]
    public EventNodeObject events;

    [Header("Run Way")]
    public bool onStart;

    [Header("Status")]
    public bool isEventPlaying;

    private void Start()
    {
        if (onStart) RunEventNode();
    }

    private void RunEventNode()
    {
        if (events == null) return;

        Debug.Log("Event Started.");
        isEventPlaying = true;
        StartCoroutine(Execute());
    }

    public IEnumerator Execute()
    {
        EventNode nextNode = events.nodes[0];

        while (true)
        {

            yield return StartCoroutine(nextNode.Execute());

            //if (nextNode != null)
            if (nextNode.outputGUID != null && nextNode.outputGUID.Length > 0)
            {
                nextNode = events.GetNextNode(nextNode);
            }
            else
            {
                Debug.Log("No more Event connections. Finishing Event."); 
                FinishEvent();
                break;
            }
            //else { Debug.Log("Next Node is null. Finishing Event."); FinishEvent();  break; }
        }

        yield return null;
    }

    void FinishEvent()
    {
        Debug.Log("Event Finished.");
        isEventPlaying = false;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(EventNodeLauncher))]
public class EventNodeLauncherInspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EventNodeLauncher targ = target as EventNodeLauncher;
        base.OnInspectorGUI();

        GUILayout.Space(30);
        if (GUILayout.Button("Open Window", GUILayout.Height(55)))
        {
            if (targ.events == null) targ.events = CreateEventNodeObject();
            var w = (EventNodeEditorWindow)EditorWindow.GetWindow(typeof(EventNodeEditorWindow));
            w.titleContent = new GUIContent("EventNode Editor");
            w.eventObj = targ.events;
            w.LoadNodes();
        }
        if (GUILayout.Button("Delete"))
        {
            if (EditorUtility.DisplayDialog("Delete EventNode.", "Delete the event node object. The created data will be reset. Are you sure?", "Sure", "No"))
            {
                DestroyImmediate(targ.events);
                targ.events = null;
            }
        }
        if (GUILayout.Button("Create DEBUG MODE"))
        {
            targ.events = CreateEventNodeObject();
            targ.events.nodes = new List<EventNode>();
            targ.events.nodes.Add(CreateEventNode("Node_Wait"));
            targ.events.nodes.Add(CreateEventNode());
            targ.events.nodes.Add(CreateEventNode("Node_Log"));
        }

    }

    EventNodeObject CreateEventNodeObject()
    {
        EventNodeObject ev = (EventNodeObject)CreateInstance(typeof(EventNodeObject));
        ev.name = "EventNodeObject";
        return ev;
    }

    EventNode CreateEventNode(string type = "None")
    {
        EventNode ev;
        switch (type)
        {
            case "None":
            default:
                ev = (EventNode)CreateInstance(typeof(EventNode));
                ev.name = "EventNode";
                ev.guid = GUID.Generate().ToString();
                return ev;
            case "Node_Log":
                ev = (Node_Log)CreateInstance(typeof(Node_Log));
                ev.name = "Node_Log";
                ev.guid = GUID.Generate().ToString();
                return ev;
            case "Node_Wait":
                ev = (Node_Wait)CreateInstance(typeof(Node_Wait));
                ev.name = "Node_Wait";
                ev.guid = GUID.Generate().ToString();
                return ev;
        }

    }
}

#endif