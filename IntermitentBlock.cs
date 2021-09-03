using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermitentBlock : MonoBehaviour
{
    [DisallowMultipleComponent] 
    public class Oscillator : MonoBehaviour
    {

        [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f); 
        [SerializeField] float periodTime = 2f; 
        [SerializeField] [Range(0, 1)] float movementRate; 

        Vector3 startingPos;

        void Start()
        {
            startingPos = transform.position;
        }

        void Update()
        {
            if (periodTime < Mathf.Epsilon)
            {
                return;
            }
            const float tau = Mathf.PI * 2; 
            float rawSinWave = Mathf.Sin(tau * (Time.time / periodTime));

            movementRate = (rawSinWave / 2f) + 0.5f;

            Vector3 offsetPosition = movementVector * movementRate;
            transform.position = startingPos + offsetPosition;
        }
    }
}
