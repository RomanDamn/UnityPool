using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
	[SerializeField] private Cube _prefab;
	[SerializeField] private float _repeatRate = 1f;
	[SerializeField] private int _poolCapacity = 5;
	[SerializeField] private int _poolMaxSize = 5;
	[SerializeField] private int _rangeOffset = 5;

	public ObjectPool<Cube> _pool;

	public void Start()
	{
		InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
	}

	private void Awake()
	{
		_pool = new ObjectPool<Cube>(
			createFunc: () => Instantiate(_prefab),
			actionOnGet: (cube) => ActionOnGet(cube),
			actionOnRelease: (cube) => ActionOnRelese(cube),
			actionOnDestroy: (cube) => Destroy(cube),
			collectionCheck: true,
			defaultCapacity: _poolCapacity,
			maxSize: _poolMaxSize);
	}

	private void ActionOnGet(Cube cube)
	{
		cube.transform.position = Random.insideUnitSphere * _rangeOffset + gameObject.transform.position;

		if(cube.TryGetComponent(out Rigidbody rb))
		{
			rb.velocity = Vector3.zero;
		}

		cube.GetComponent<CollisionHandler>().Died += ActionOnRelese;
		cube.gameObject.SetActive(true);
	}

	private void ActionOnRelese(Cube cube)
	{
		cube.GetComponent<CollisionHandler>().Died -= ActionOnRelese;
		cube.gameObject.SetActive(false);
	}

	private void GetCube()
	{
		_pool.Get();
	}
}
