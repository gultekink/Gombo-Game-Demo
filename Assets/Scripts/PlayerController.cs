using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour

{  
    [Header("Player Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _currentPower;
    [SerializeField] private float _currentMass;
    [SerializeField] private int _currentScore;

    public Transform ColliderTransform;
    
    private Collider _weaknessCollider;

    private static Vector2 _startTouch;
    private Vector2 _swipeDelta;
    
    private Rigidbody _rigidbody;
    private bool _isGameStart;
    public bool _isGameFinish;
    private bool _isGameVictory;
    #region Props

    public float Power
    {
        get => _currentPower;
        set => _currentPower = value;
    }

    public float Mass
    {
        get => _currentMass;
        set => _currentMass = value;
    }

    public int Score
    {
        get => _currentScore;
        set => _currentScore = value;
    }

    #endregion


    void Awake()
    {
        _weaknessCollider = ColliderTransform.GetChild(0).GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();

        _currentPower =2;
        _currentMass = _rigidbody.mass;
        _currentScore = 0;
        _isGameFinish = false;
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        RotatePlayer();
        _rigidbody.mass = _currentMass;
    }

    void RotatePlayer()
    {
        if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    _startTouch = touch.position;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    _swipeDelta = (touch.position - _startTouch).normalized;
                    SetRotation();
                }

                if (touch.phase == TouchPhase.Ended )
                {
                    _startTouch = Vector2.zero;
                }
            }
    }

    void SetRotation()
    {
        if (_swipeDelta.y > 0)
        {
            player.rotation = Quaternion.Euler(0, 90f * _swipeDelta.x, 0);
        }
        else
        {
            player.rotation = Quaternion.Euler(0, 180 - (90f * _swipeDelta.x), 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();

        if (collision.gameObject.CompareTag("Bot"))
        {
            if (collision.gameObject.name == "BotWeakness" && collision.gameObject != _weaknessCollider)
            {
                _currentPower *= 2;
                Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
                enemyRigidbody.AddForce(awayFromPlayer * _currentPower, ForceMode.Impulse);
            }

            if (_currentMass > enemyRigidbody.mass)
            {
                Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
                enemyRigidbody.AddForce(awayFromPlayer * _currentPower, ForceMode.Impulse);
                
            }
        }
    }

    void FinishGame()
    {
        if (transform.position.y < -5)
        {
            _isGameFinish = true;
            Debug.Log("Game Over!");
        }
    }

}

