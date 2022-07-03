using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

public class Node_Wait : EventNode
{

    public float time;

    public override IEnumerator Execute()
    {
        base.Execute();

        yield return new WaitForSeconds(time);
    }


#if UNITY_EDITOR
    public override EventNodeEditor CreateEditorNode()
    {
        EventNodeEditor node = new EventNodeEditor()
        {
            title = "Wait",
            guid = UnityEditor.GUID.Generate().ToString(),
            eventNode = this
        };

        node.GeneratePort(node, Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port), "in");
        node.GeneratePort(node, Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port), "out");

        // here for field generation commands.

        return node;
    }
    public override EventNode SaveNodeParams(EventNodeEditor editorNode)
    {
        base.SaveNodeParams(editorNode);
        //time = editorNode.fields[0];
        time = Random.Range(1.0f, 3.0f);
        return this;
    }
#endif
}