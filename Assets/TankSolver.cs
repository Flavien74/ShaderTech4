using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSolver : MonoBehaviour
{
    [SerializeField]
    private float _solveTime = 1.0f;

    [SerializeField]
    private GameObject[] _go;

    private List<Material> _mat = new List<Material>();

    void Start()
    {
        foreach (var go in _go)
        {

            Material[] _newMats = go.GetComponent<Renderer>().materials;
            foreach (var mat in _newMats)
            {
                _mat.Add(mat);
                mat.SetFloat("_DisolveAmount", 1);
            }
        }

        StartCoroutine(Solve());
    }

    private IEnumerator Solve()
    {
        float elapsedTime = 0f;

        while(elapsedTime < _solveTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpDissolve = Mathf.Lerp(1.1f, 0f,(elapsedTime / _solveTime));

            for (int i = 0; i < _mat.Count; i++)
            {
                _mat[i].SetFloat("_DisolveAmount", lerpDissolve);
            }

            yield return null;
        }
    }
}
