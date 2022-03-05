using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    private Vector3 _boostScale;
    private Vector3 _spawnVector3;
    private float _spawnX, _spawnZ;
    private float _spawnY;

    void Start()
    {
        _boostScale = new Vector3(.2f, .2f, .2f);
        _spawnY = 3f;
    }
   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Ground" && other.gameObject.tag =="Player")
        {
            Destroy(transform.parent.gameObject);
            other.transform.localScale += _boostScale;
            other.GetComponent<PlayerController>().Power += 1.2f;
            other.GetComponent<PlayerController>().Mass += .4f;
            other.GetComponent<PlayerController>().Score += 100;

        }
        else if (other.gameObject.tag != "Ground" && other.gameObject.tag == "Bot")
        {
            Destroy(transform.parent.gameObject);
            other.transform.localScale += _boostScale;
            other.GetComponent<BotController>().Power += 1f;
            other.GetComponent<BotController>().Mass += .4f;
            other.GetComponent<BotController>().Score += 100;
        }
    }
}
