using System.Collections;
using System.Collections.Generic;
using StagPoint.Eval.Parser;
using UnityEngine;

public class Ignitor : MonoBehaviour
{
    [Tooltip("Flame to ignite")]
    public GameObject flame;
    
    public void Ignite()
    {
        flame.SetActive(true);
        
        // deactivate owner to make sure we don't try to ignite the same ignitor later
        gameObject.SetActive(false);
    }
}
