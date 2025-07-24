using System;
using UnityEngine;

[RequireComponent(typeof(Boid))]
public class BoidBodyRotator : MonoBehaviour
{
    private Boid _boid;
    [SerializeField] private GameObject _Body;
    
    private void Start()
        => _boid = GetComponent<Boid>();

    private void LateUpdate()
    {
        float signedAngle = Vector2.SignedAngle(_boid.Direction, Vector2.right);
        _Body.transform.eulerAngles = Vector3.back * signedAngle;
    }
}
