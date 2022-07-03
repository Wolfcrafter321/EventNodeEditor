using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditor.Graphs;
public class NodeEditorGraphView : GraphView
{

    public Dictionary<string, EventNodeEditor> _nodes;   // GUID : node(editornode)
    
    public NodeEditorGraphView()
    {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        //AddElement(GenerateNode());

    }

    public void LoadNodes(EventNodeObject _receivedEventObject)
    {
         _nodes = new Dictionary<string, EventNodeEditor>();

        for (int i = 0; i < _receivedEventObject.nodes.Count; i++)
        {
            EventNodeEditor n = _receivedEventObject.nodes[i].CreateEditorNode();
            _nodes[_receivedEventObject.nodes[i].guid] = n;
            AddElement(n);
        }
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList();
        //return base.GetCompatiblePorts(startPort, nodeAdapter);
    }

}


public class EventNodeEditorWindow : EditorWindow
{
    NodeEditorGraphView graph;
    public EventNodeObject eventObj;

    private void OnEnable()
    {
        graph = new NodeEditorGraphView()
        {
            style = { flexGrow = 1, backgroundColor = new StyleColor(new Color(0.1f, 0.1f, 0.1f)) },
            name = "EventNode Graph",
        };

        Toolbar toolbar = new Toolbar();
        {
            // toolbar.Add(new ToolbarSpacer() { name = "Space", style = { height = 60 } });
            ToolbarButton tb1 = new ToolbarButton() { text = "Save Events" };
            ToolbarButton tb2 = new ToolbarButton() { text = "Load Events" };
            tb1.clickable.clicked += () => { Debug.Log("Save"); SaveNodes(); };
            tb2.clickable.clicked += () => { Debug.Log("Load"); LoadNodes(); };
            toolbar.Add(tb1);
            toolbar.Add(new ToolbarSpacer() { name = "Spaece", style = { width = 50 } });
            toolbar.Add(tb2);
        }

        graph.Add(toolbar);
        rootVisualElement.Add(graph);

        if (eventObj != null) LoadNodes();

    }

    public void SaveNodes()
    {
        eventObj.nodes = new List<EventNode>();

        foreach (var node in graph._nodes.Values)
        {
            eventObj.nodes.Add((EventNode)node.GetNode());
        }
    }

    public void LoadNodes()
    {
        if (eventObj != null) graph.LoadNodes(eventObj);
        else Debug.LogError("Try to load but the graph is null...");
    }
}
#endif