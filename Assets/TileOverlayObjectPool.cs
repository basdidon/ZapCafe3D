using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOverlayObjectPool : MonoBehaviour
{
    public static TileOverlayObjectPool Instance { get; private set; }

    [SerializeField] GameObject tileOverlayPrefab;
    [SerializeField] int poolSize;

    List<GameObject> poolObjects;

    private void Awake()
    {
        if(Instance!= null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        poolObjects = new();
    }

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var _clone = Instantiate(tileOverlayPrefab, transform);
            _clone.SetActive(false);
            poolObjects.Add(_clone);
        }
    }
}
