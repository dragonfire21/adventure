using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] private TransitionArea transition;
    [SerializeField] private float currentPositionX;
    // Start is called before the first frame update
    void Start()
    {
        transition.OnTransitionCam += MoveCam;
    }

    void MoveCam(bool left ,float posX)
    {
        if(currentPositionX != posX)
        {
            if (left)
            {
                currentPositionX -= posX;
                transform.position = new(currentPositionX, 0, transform.position.z);
            }
            else
            {
                currentPositionX += posX;
                transform.position = new(currentPositionX, 0, transform.position.z);
            }
        }
        else
        {
            if (left)
            {
                currentPositionX -= posX;
                transform.position = new(currentPositionX, 0, transform.position.z);
            }
            else
            {
                currentPositionX += posX;
                transform.position = new(currentPositionX, 0, transform.position.z);
            }
        }
    }
}
