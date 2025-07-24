using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
	private static List<Boid> _boids;

	[SerializeField] private float _Speed;
	private Vector2 _direction;

	public Vector2 Direction
	{
		get => _direction;
		set => _direction = value.normalized;
	}
	
	private void Update()
	{
		Vector2 delta = _direction * (_Speed * Time.deltaTime);
		transform.position += (Vector3)delta;
	}
}
