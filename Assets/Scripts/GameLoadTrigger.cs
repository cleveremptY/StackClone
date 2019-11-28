using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLoadTrigger : MonoBehaviour
{
    public UnityEvent onLoad;

    void Start()
    {
        onLoad.Invoke();
    }
}
