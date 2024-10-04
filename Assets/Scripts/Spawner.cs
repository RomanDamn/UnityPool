using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity= 5;
    [SerializeField] private int _poolMaxSize = 5;
    [SerializeField] private int _rangeOffset = 5;

    public ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRelese(obj),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    void Start()
    {
        InvokeRepeating(nameof(GetSphere), 0.0f, _repeatRate);
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = Random.insideUnitSphere * _rangeOffset + gameObject.transform.position;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void ActionOnRelese(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = Color.white;

        if (obj.TryGetComponent(out CollisionHandler collisionHandler))
        {
            collisionHandler.ToggleCollision();
        }
        
        obj.SetActive(false);
    }

    private void GetSphere()
    {
        _pool.Get();
    }
}
