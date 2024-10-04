using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private string _respawnTag = "Respawn";
    [SerializeField] private string _collisionTag = "Collision";

    private bool _hasCollision = false;

    private void ChangeColor()
    {
        Color color = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
         );

        gameObject.GetComponent<Renderer>().material.color = color;
    }

    private void ReturnToPool()
    {
        var pool = GameObject.FindWithTag(_respawnTag);

        if (pool != null)
        {
            if (pool.TryGetComponent(out Spawner spawner))
            {
                spawner._pool.Release(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == _collisionTag && _hasCollision == false)
        {
            int secondsToDissapear = Random.Range(2, 6);

            ToggleCollision();
            ChangeColor();
            Invoke(nameof(ReturnToPool), secondsToDissapear);
        }
    }

    public void ToggleCollision()
    {
        _hasCollision = !_hasCollision;
    }
}
