using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    [SerializeField] private GameObject _boostObject;

    private Vector3 _boostScale;
    private Vector3 _spawnVector3;

    private float _spawnX, _spawnZ;
    private float _spawnY;

    private float _timer;

    void Start()
    {
        _boostScale = new Vector3(.2f, .2f, .2f);
        _spawnY = 4f;
        _timer = 1.5f;
    }

     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(transform.parent.gameObject);
            other.transform.localScale += _boostScale;
        }
      
    }

     public void SpawnBoost(float spawnCount)
     {
         for (int i = 0; i < spawnCount; i++)
         {
             _spawnX = Random.Range(-33f, 33f);
         _spawnZ = Random.Range(-33f, 33f);
         _spawnVector3 = new Vector3(_spawnX, _spawnY, _spawnZ);
         Instantiate(_boostObject, _spawnVector3, Quaternion.identity);
        }
     }

     void Update()
     {
         _timer -= Time.deltaTime;
         if (_timer <= 0)
         {
             SpawnBoost(Random.Range(1,3));
             _timer = 1f;
         }
     }

     ////Wait additional 2 second after start
     //private IEnumerator SpawnTime(float waitTime)
     //{
     //    yield return new WaitForSeconds(waitTime);
     //    SpawnBoost();
     //}
}
