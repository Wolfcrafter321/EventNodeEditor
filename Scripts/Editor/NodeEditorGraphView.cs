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

    public NodeEditorGraphView()
    {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        AddElement(GenerateNode());

    }

    EventNodeEditor GenerateNode()
    {
        EventNodeEditor node = new EventNodeEditor()
        {
            title = "HelloWorld",
            guid = GUID.Generate().ToString()
           };
        node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));
        return node;
    }

}
#endif