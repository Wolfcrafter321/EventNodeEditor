using System.Collections;
using UnityEngine;

public class Node_Wait : EventNode
{

    public float time;


    public override IEnumerator Execute()
    {
        base.Execute();

        yield return new WaitForSeconds(time);
    }


}
