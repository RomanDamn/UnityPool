using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private string _collisionTag = "Collision";

	private bool _hasCollision = false;

    public event Action<GameObject> Died;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == _collisionTag && !_hasCollision)
        {
			int secondsToDissapear = UnityEngine.Random.Range(2, 6);
			ToggleCollision();
			ChangeColor();
			StartCoroutine(Dissapear(secondsToDissapear));
		}
    }

    private IEnumerator Dissapear(int secondsToDissapear)
    {
        yield return new WaitForSeconds(secondsToDissapear);
		ToggleCollision();
		Died?.Invoke(gameObject);
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
