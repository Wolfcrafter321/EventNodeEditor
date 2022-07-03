using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

public class Node_Log : EventNode
{

    public string log;

    public override IEnumerator Execute()
    {
        base.Execute();

        Debug.Log(log);

        yield return null;
    }


#if UNITY_EDITOR
    public override EventNodeEditor CreateEditorNode()
    {
        EventNodeEditor node = new EventNodeEditor()
        {
            title = "Log",
            guid = UnityEditor.GUID.Generate().ToString(),
            eventNode = this
        };

        node.GeneratePort(node, Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port), "in");
        node.GeneratePort(node, Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port), "out");

        return node;
    }

    public override EventNode SaveNodeParams(EventNodeEditor editorNode)
    {
        base.SaveNodeParams(editorNode);
        //log = editorNode.fields[0];
        log = "hjogehigeiheg";
        return this;
    }
#endif
}