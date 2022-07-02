using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class EventNode
{
    public string title;
    public string guid;
    public string[] inputGUID;
    public string[] outputGUID;


    public virtual IEnumerator Execute()
    {

        Debug.LogFormat("Exe! {0} {1}", title, guid);
        yield return new WaitForSeconds(1f);
    }
}

#if UNITY_EDITOR
public class EventNodeEditor : Node
{
    public string guid;
    public List<Port> inputs;
    public List<Port> outputs;


    void OnEnable()
    {
        guid = GUID.Generate().ToString();
    }
}
#endif