using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Log : EventNode
{

    public string log;


    public override IEnumerator Execute()
    {

        Debug.Log(log);

        yield return null;
    }


}
