using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class EventNode : ScriptableObject
{
    public string guid;
    public string[] inputGUID;
    public string[] outputGUID;

    // for editor
    public Vector2 _position;

    public virtual IEnumerator Execute()
    {
        Debug.LogFormat("Exe! {0}", guid);
        yield return null;
    }

#if UNITY_EDITOR
    public virtual EventNodeEditor CreateEditorNode()
    {
        EventNodeEditor node = new EventNodeEditor()
        {
            title = "Node",
            guid = guid,
            eventNode = this
        };

        //node.GeneratePort( node, Orientation.Horizontal, Direction.Input,  Port.Capacity.Single, typeof(Port), "in");

        //node.GeneratePort(node, Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port), "out");

        var f = node.GenerateField("String", "");
        node.contentContainer.Add(f);
        node.extensionContainer.Add(f);
        node.topContainer.Add(f);
        node.mainContainer.Add(f);

        node.SetPosition(new Rect(x: _position.x, y: _position.y, width: 100, height: 150));
        return node;
    }

    public virtual EventNode SaveNodeParams(EventNodeEditor editorNode)
    {
        guid = editorNode.guid;
        //inputGUID = editorNode.inputs.
        //outputGUID = editorNode.outputs.
        return this;
    }
#endif
}

#if UNITY_EDITOR
public class EventNodeEditor : Node
{
    public string guid;
    public List<Port> inputs;
    public List<Port> outputs;

    public EventNode eventNode;


    public EventNodeEditor()
    {
        inputs = new List<Port>();
        outputs = new List<Port>();
        if (string.IsNullOrEmpty(guid)) guid = GUID.Generate().ToString();
    }

    public VisualElement GenerateField(string type, string nm)
    {
        var ns = new TextField() { style = { minWidth = 100, minHeight = 30 } };
        //field.Add(ns);
        ns.RegisterCallback<FocusInEvent>(evt => { Input.imeCompositionMode = IMECompositionMode.On; });
        ns.RegisterCallback<FocusOutEvent>(evt => { Input.imeCompositionMode = IMECompositionMode.Auto; });
        //ns.value = var;
        ns.value = "";
        return ns;
    }

    public Port GeneratePort(EventNodeEditor n, Orientation orient, Direction direction, Port.Capacity capacity, System.Type type, string name = "port")
    {
        Port p = n.InstantiatePort(orient, direction, capacity, type);
        p.name = name;
        switch (direction)
        {
            case Direction.Input:
                inputs.Add(p);
                inputContainer.Add(p);
                break;
            case Direction.Output:
                outputs.Add(p);
                outputContainer.Add(p);
                break;
        }
        return p;
    }

    public object GetNode()
    {
        return eventNode.SaveNodeParams(this);
    }

}
#endif