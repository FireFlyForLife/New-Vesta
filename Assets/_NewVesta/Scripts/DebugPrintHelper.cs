using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPrintHelper : MonoBehaviour
{
    public void PrintNotify()
    {
        Debug.Log("Notify!");
    }

    public void PrintMessage(string message)
    {
        Debug.Log("Printing msg: " + message);
    }

    public void PrintSomething(object something)
    {
        Debug.Log("Printing: " + something);
    }
}
