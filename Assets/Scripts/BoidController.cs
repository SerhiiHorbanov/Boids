using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoidController : MonoBehaviour
{
	[SerializeField] private Boid _Controlled;
	[SerializeField] private float _ViewDistance;

	[SerializeField] private float _CosineSimilarityForCohesion;
	[SerializeField] private float _CohesionStrength = 1;
	
	[SerializeField] private float _AlignmentStrength = 1;
	
	[SerializeField] private float _SeparationDistance;
	[SerializeField] private float _SeparationStrength;

	private readonly static List<Boid> Boids = new();
	
	private float ViewDistanceSquared 
		=> _ViewDistance * _ViewDistance;
	private float AvoidingDistanceSquared
		=> _SeparationDistance * _SeparationDistance;
	
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
		int boidsInView = 0;
		int boidsWithinCosineSimilarity = 0;
		
		Vector2 center = Vector2.zero;
		float averageDirection = 0;
		
		Vector2 centerOfAvoided = Vector2.zero;
		int avoidedBoids = 0;
		
		foreach (Boid boid in Boids)
		{
			float distanceSquaredToBoid = (_Controlled.Position - boid.Position).sqrMagnitude;

			if (ViewDistanceSquared < distanceSquaredToBoid)
				continue;
			
			boidsInView++;
			
			center += boid.Position;
			
			float cosineSimilarity = Vector2.Dot(_Controlled._Direction, boid._Direction);
			if (cosineSimilarity > _CosineSimilarityForCohesion)
			{
				averageDirection += Vector2.Angle(Vector2.right, boid._Direction);
				boidsWithinCosineSimilarity++;
			}

			if (_SeparationDistance < distanceSquaredToBoid)
				continue;
			if (ReferenceEquals(boid, _Controlled))
				continue;
			
			avoidedBoids++;
			centerOfAvoided += boid.Position;
		}
		
		if (boidsInView > 0)
			center /= boidsInView;
		if (boidsWithinCosineSimilarity > 0)
			averageDirection /= boidsWithinCosineSimilarity;
		if (avoidedBoids > 0)
			centerOfAvoided /= avoidedBoids;
		
		Vector2 directionToCenter = (center - _Controlled.Position).normalized;
		Vector2 averageDirectionVector = new(Mathf.Cos(averageDirection * Mathf.Deg2Rad), Mathf.Sin(averageDirection * Mathf.Deg2Rad));
		Vector2 directionToCenterOfAvoided = (centerOfAvoided - _Controlled.Position).normalized;

		Vector2 cohesionForce = directionToCenter * _CohesionStrength;
		Vector2 alignmentForce = averageDirectionVector * _AlignmentStrength;
		Vector2 separationForce = avoidedBoids > 0 ? -directionToCenterOfAvoided * _SeparationStrength : Vector2.zero;
		
		Vector2 newDirection = cohesionForce + alignmentForce + separationForce;
		_Controlled.SetTargetDirection(newDirection);
	}
	
	private void OnDestroy()
	{
		SetControlled(null);
	}

	private void OnDrawGizmosSelected()
	{
		Vector2 position = _Controlled.Position;
		
		Handles.color = Color.green;
		Handles.DrawWireArc(position, Vector3.forward, Vector3.right, 360, _ViewDistance);
		
		Handles.color = Color.red;
		Handles.DrawWireArc(position, Vector3.forward, Vector3.right, 360, _SeparationDistance);
		
		float offset = transform.eulerAngles.z * Mathf.Deg2Rad;
		
		float angle = Mathf.Acos(_CosineSimilarityForCohesion);
		float x = Mathf.Cos(angle + offset) * _ViewDistance;
		float y = Mathf.Sin(angle + offset) * _ViewDistance;
		float x2 = Mathf.Cos(-angle + offset) * _ViewDistance;
		float y2 = Mathf.Sin(-angle + offset) * _ViewDistance;
		
		Handles.color = Color.yellow;
		Handles.DrawWireArc(position, Vector3.back, new(x, y, 0), angle * 2 * Mathf.Rad2Deg, _ViewDistance);
		
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(position, position + new Vector2(x, y));
		Gizmos.DrawLine(position, position + new Vector2(x2, y2));
	}
}
