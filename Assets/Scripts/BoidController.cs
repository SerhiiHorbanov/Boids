using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidController : MonoBehaviour
{
	[SerializeField] private Boid _Controlled;

	private readonly static List<Boid> Boids = new();
	
	private void SetControlled(Boid newControlled)
	{
		if (ReferenceEquals(newControlled, _Controlled)) 
			return;
		
		if (_Controlled is not null)
			Boids.Remove(_Controlled);
		
		_Controlled = newControlled;
		
		if (newControlled is not null)
			Boids.Add(_Controlled);
	}

	private void Start()
	{
		if (_Controlled is not null)
			Boids.Add(_Controlled);
	}

	private void Update()
	{
		Vector2 center = Vector2.zero;
		foreach (Boid boid in Boids)
			center += (Vector2)boid.transform.position;
		center /= Boids.Count;
		
		_Controlled.Direction = center - (Vector2)_Controlled.transform.position;
	}
	
	private void OnDestroy()
	{
		SetControlled(null);
	}
}
