using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSolver : MonoBehaviour
{
    [SerializeField]
    private float _solveTime = 1.0f;

    [SerializeField]
    private Material _mat;

    private int _solveAmount = Shader.PropertyToID("_DissolveAmount");
    private int _spiralSolveAmount = Shader.PropertyToID("_SpiralForce");


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
