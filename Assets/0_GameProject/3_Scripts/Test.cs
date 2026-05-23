using UnityEngine;

public class InputTest : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
            Debug.Log("ANY KEY!");

        if (Input.GetMouseButtonDown(0))
            Debug.Log("MOUSE!");
    }
}