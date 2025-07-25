using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
	private static List<Boid> _boids;

	[SerializeField] private float _Speed;
	[SerializeField] private float _RotationSpeed;
	public Vector2 _Direction {get; private set;}
	public Vector2 _TargetDirection;

	public Vector2 Position
		=> transform.position;
	
	public void SetTargetDirection(Vector2 direction)
		=> _TargetDirection = direction.normalized;
	
	private void Update()
	{
		RotateDirectionTowardsTargetDirection();

		Vector2 delta = _Direction * (_Speed * Time.deltaTime);
		transform.position += (Vector3)delta;
	}
	
	private void RotateDirectionTowardsTargetDirection()
	{
		float currentRotation = Vector2.SignedAngle(Vector2.right, _Direction);
		float targetRotation = Vector2.SignedAngle(Vector2.right, _TargetDirection);

		currentRotation = Mathf.MoveTowardsAngle(currentRotation, targetRotation, _RotationSpeed * Time.deltaTime);
		
		_Direction = new(Mathf.Cos(currentRotation * Mathf.Deg2Rad), Mathf.Sin(currentRotation * Mathf.Deg2Rad));
	}
}
