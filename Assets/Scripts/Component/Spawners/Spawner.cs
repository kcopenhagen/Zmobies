
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] protected int poolSize = 100;
    [SerializeField] protected float spawnInterval = 5f;
    [SerializeField] protected string source;
    [SerializeField] protected DieSpawner dieSpawner;
    [SerializeField] protected int cost = 0;
    [SerializeField] protected GameObject thoughtBox;

    private Vector2 spawnAreaSize;
    private List<GameObject> pool;
    private RectTransform thoughtRect;
    private readonly float boxWidth2DecisionTimeFactor = 1f / 100f;
    private readonly float boxHeight2SpawnTimeFactor = 1f / 50f;
    private Coroutine myRoutine;
    public static List<Spawner> All { get; } = new List<Spawner>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr==null)
        {
            Debug.Log("Spawner sprite renderer not found");
            spawnAreaSize = new Vector2(1, 1);
        }
        else
        {
            sr.color = ColorManager.Instance.SelectPanelColor(source);
            spawnAreaSize = sr.bounds.size;
        } 

        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
        All.Add(this);
        ResetSpawnInterval(99999f);
    }

    private IEnumerator SpawnRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnInterval);

            SpawnObject();
        }
    }

    void SpawnObject()
    {
        if (!CurrencyManager.Instance.SpendBodies(cost))
            return;

        thoughtRect = thoughtBox.GetComponent<RectTransform>();

        Vector2 spawnPos = (Vector2)transform.position + new Vector2(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f)
        );

        foreach (var obj2 in pool)
        {
            if (!obj2.activeInHierarchy)
            {
                obj2.transform.position = spawnPos;
                var mover = obj2.GetComponent<CircleMove>();
                obj2.SetActive(true);
                if (thoughtRect == null)
                    Debug.Log("No thought rect");
                mover.GetComponent<CircleWander>().SetDirectionChangeInterval(thoughtRect.rect.width * boxWidth2DecisionTimeFactor);
                mover.GetComponent<CircleWander>().StartWandering();
                List<DieSettings> sourceDice = dieSpawner.GetDiceOfSource(source);
                mover.AddDice(sourceDice);

                return;
            }
        }
    }
    private void OnDestroy()
    {
        All.Remove(this);
    }

    public void ResetSpawnInterval(float spawnInt)
    {
        if (spawnInt <= 0)
        {
            thoughtRect = thoughtBox.GetComponent<RectTransform>();
            if (thoughtRect != null)
                spawnInterval = thoughtRect.rect.height * boxHeight2SpawnTimeFactor;
        }
        else spawnInterval = spawnInt;

        if (myRoutine != null)
        {
            StopCoroutine(myRoutine);
        }
        myRoutine = StartCoroutine(SpawnRoutine());
    }
}
