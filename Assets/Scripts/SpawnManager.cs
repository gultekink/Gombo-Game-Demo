using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawnObjects;

    [SerializeField] private Transform[] _spawnTransforms;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _spawnObjects.Length; i++)
        {
            Instantiate(_spawnObjects[Random.Range(0, _spawnObjects.Length)],
                _spawnTransforms[Random.Range(0, _spawnTransforms.Length)].position,Quaternion.identity);
        }
    }
}
