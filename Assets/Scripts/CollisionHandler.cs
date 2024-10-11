using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Cube))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private string _collisionTag = "Collision";

	private int _minSecondsToDissapear = 2;
	private int _maxSecondsToDissapear = 6;

	private bool _hasCollision = false;

    public event Action<Cube> Died;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == _collisionTag && !_hasCollision)
        {
			int secondsToDissapear = UnityEngine.Random.Range(_minSecondsToDissapear, _maxSecondsToDissapear);
			ToggleCollision();
			ChangeColor();
			StartCoroutine(Dissapear(secondsToDissapear));
		}
    }

    private IEnumerator Dissapear(int secondsToDissapear)
    {
        yield return new WaitForSeconds(secondsToDissapear);

		ToggleCollision();
		var cube = GetComponent<Cube>();
		Died?.Invoke(cube);
	}

	private void ToggleCollision()
	{
		_hasCollision = !_hasCollision;
	}

	private void ChangeColor()
	{
		Color color = new Color(
			UnityEngine.Random.Range(0f, 1f),
			UnityEngine.Random.Range(0f, 1f),
			UnityEngine.Random.Range(0f, 1f)
		 );

		gameObject.GetComponent<Renderer>().material.color = color;
	}
}
