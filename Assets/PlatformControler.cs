using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControler : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    Transform _startPoint;

    [SerializeField]
    Transform _endPoint;

    [SerializeField]
    float _timeToReach;
    #endregion

    #region Unity Lifecycle
    void Start()
    {
        transform.position = _startPoint.position; 
    }

    void Update()
    {
        if (_isForward)
        {

            _timerMovement += Time.deltaTime;
            if (_timerMovement >= _timeToReach )
            {
                _isForward = false;
            }
        }
        else
        {
            _timerMovement -= Time.deltaTime;
            if (_timerMovement <= 0f)
            {
                _isForward = true;
            }
        }
    
        Vector3 newPosition = Vector3.Lerp(_startPoint.position, _endPoint.position, _timerMovement / _timeToReach);

        transform.position = newPosition;
    }
    #endregion

    #region Methods

    #endregion

    #region Private & Protected


    private bool _isForward = true;
    private float _timerMovement;

    #endregion
}
