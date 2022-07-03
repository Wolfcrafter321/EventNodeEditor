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
public class EventNodeLauncherEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EventNodeLauncher targ = target as EventNodeLauncher;
        base.OnInspectorGUI();

        GUILayout.Space(30);
        if (GUILayout.Button("Open Window"))
        {
            if (targ.events == null) targ.events = CreateNode();
            var w = EditorWindow.GetWindow(typeof(EventNodeEditorWindow));
            w.titleContent = new GUIContent("EventNode Editor");
        }
        if (GUILayout.Button("Create"))
        {
            targ.events = CreateNode();
        }
        if (GUILayout.Button("Delete"))
        {
            DestroyImmediate(targ.events);
            targ.events = null;
        }
        if (GUILayout.Button("Create DEBUG MODE"))
        {
            targ.events = CreateNode();
            targ.events.nodes = new List<EventNode>();
            targ.events.nodes.Add(CreateEventNode("Node_Wait"));
            targ.events.nodes.Add(CreateEventNode());
            targ.events.nodes.Add(CreateEventNode("Node_Log"));
        }
    }

    EventNodeObject CreateNode()
    {
        EventNodeObject ev = (EventNodeObject)ScriptableObject.CreateInstance(typeof(EventNodeObject));
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
                ev = (EventNode)ScriptableObject.CreateInstance(typeof(EventNode));
                ev.name = "EventNode";
                return ev;
            case "Node_Log":
                ev = (Node_Log)ScriptableObject.CreateInstance(typeof(Node_Log));
                ev.name = "Node_Log";
                return ev;
            case "Node_Wait":
                ev = (Node_Wait)ScriptableObject.CreateInstance(typeof(Node_Wait));
                ev.name = "Node_Wait";
                return ev;
        }

        return null;
    }
}

public class EventNodeEditorWindow : EditorWindow
{
    NodeEditorGraphView graph;

    private void OnEnable()
    {
        graph = new NodeEditorGraphView()
        {
            style = { flexGrow = 1, backgroundColor = new StyleColor(new Color(0.1f, 0.1f, 0.1f)) },
            name = "EventNode Graph",
        };
        Toolbar toolbar = new Toolbar();
        {
            toolbar.Add(new ToolbarSpacer() { name = "Space", style = { height = 60 } });
            ToolbarButton tb1 = new ToolbarButton() { text = "Save Events" };
            tb1.clickable.clicked += () => { Debug.Log("Hoge"); };
            //tb1.clickable.clicked += () => gv.SaveGraph();
            toolbar.Add(tb1);
            toolbar.Add(new ToolbarSpacer() { name = "Spaece", style = { width = 50 } });
        }

        graph.Add(toolbar);
        rootVisualElement.Add(graph);
    }
}
#endif