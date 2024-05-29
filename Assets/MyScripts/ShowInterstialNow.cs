using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInterstialNow : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        Debug.Log("Loading Enable");
        UIHandler.Instance.ShowloadingAdStatic();
    }
}
