using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventNodeObject : ScriptableObject
{

    public List<EventNode> nodes;

    private void Reset()
    {
        nodes = new List<EventNode>();
        nodes.Add(new EventNode());

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
