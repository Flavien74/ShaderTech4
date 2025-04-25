using System.Collections;
using System.Collections.Generic;
using Tanks.Complete;
using UnityEngine;

public class TankSolver : MonoBehaviour
{
    [SerializeField]
    private float _solveTime = 1.0f;

    [SerializeField]
    private float _blinkTime = 1.0f;

    [SerializeField]
    private GameObject[] _allElements;

    [SerializeField]
    private GameObject[] _coloredElements;

    [SerializeField]
    private Material _hitMaterial;

    [SerializeField]
    private Material _baseMaterial;

    [SerializeField]
    private Material _hitOnScreenMaterial;

    private List<Material> _mat = new List<Material>();

    private TankMovement _movement;

    void Start()
    {
        TankHealth.OnPlayerHit += HandlePlayerHit;
        TankHealth.OnPlayerDeath += HandlePlayerDeath;

        _movement = GetComponent<TankMovement>();

        foreach (var go in _allElements)
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
    private void HandlePlayerHit(GameObject player)
    {
        if (this == null || player == null) return;

        if (player != gameObject)
        {
            return;
        }

        if (!_movement.m_IsComputerControlled)
        {
            StartCoroutine(Blink(_hitOnScreenMaterial, "_VignettePower",8,1.5f,false));
        }
        foreach (var go in _coloredElements)
        {
            if (go == null) continue;

            Renderer _rend = go.GetComponent<Renderer>();
            if (_rend == null) continue;

            int matCount = _rend.materials.Length;
            Material[] newMats = new Material[matCount];

            for (int i = 0; i < matCount; i++)
            {
                newMats[i] = _hitMaterial;
            }

            _rend.materials = newMats;

            foreach (var mat in newMats)
            {
                StartCoroutine(Blink(mat, "_Transparency",1,0,true));
            }
        }    
    }
    private void HandlePlayerDeath()
    {
        Debug.Log("test");
        foreach (var go in _coloredElements)
        {
            if (go == null) continue;

            Renderer _rend = go.GetComponent<Renderer>();

            if (_rend == null) continue;

            int matCount = _rend.materials.Length;
            Material[] newMats = new Material[matCount];

            for (int i = 0; i < matCount; i++)
            {
                newMats[i] = _baseMaterial;
            }

            _rend.materials = newMats;
        }
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
    private IEnumerator Blink(Material mat,string floatName,float maxValue, float minValue,bool isOnCar)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _blinkTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpTransparency = Mathf.Lerp(maxValue, minValue, (elapsedTime / _blinkTime));

            mat.SetFloat(floatName, lerpTransparency);

            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < _blinkTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpTransparency = Mathf.Lerp(minValue, maxValue, (elapsedTime / _blinkTime));

            mat.SetFloat(floatName, lerpTransparency);

            yield return null;
        }
        mat.SetFloat(floatName, maxValue);

        if (isOnCar)
        {
            foreach (var go in _coloredElements)
            {
                if (go == null) continue;

                Renderer _rend = go.GetComponent<Renderer>();
                if (_rend == null) continue;

                int matCount = _rend.materials.Length;
                Material[] newMats = new Material[matCount];

                for (int i = 0; i < matCount; i++)
                {
                    newMats[i] = _baseMaterial;
                }

                _rend.materials = newMats;
            }
        }
    }
}
