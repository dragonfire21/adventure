using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionArea : MonoBehaviour
{
    public delegate void TransitionCam(bool left, float posX);
    public event TransitionCam OnTransitionCam;
    [SerializeField] private float posX;
    [SerializeField] private bool left;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnTransitionCam?.Invoke(left, posX);       
            left = !left;
        }
    }
}
