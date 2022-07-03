using System.Collections;
using UnityEngine;

public class Node_Log : EventNode
{

    public string log;


    public override IEnumerator Execute()
    {
        base.Execute();

        Debug.Log(log);

        yield return null;
    }


}
