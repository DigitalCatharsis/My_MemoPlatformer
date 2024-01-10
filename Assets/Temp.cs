using My_MemoPlatformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public ScriptableObject scriptableObject;
    public bool test;

    private void Update()
    {
        DebugContainer.Instance.debug_Attack = test;
    }
}
