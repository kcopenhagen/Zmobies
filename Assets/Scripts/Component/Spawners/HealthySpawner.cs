using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;


public class HealthySpawner : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private DieSpawner dieSpawner;
    [SerializeField] private TextMeshProUGUI procTimeText;
    [SerializeField] private TextMeshProUGUI spawnTimeText;

    private float spawnInterval;
    private float procTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateNumbers();
        StartCoroutine(SpawnerRoutine());
    }

    private IEnumerator SpawnerRoutine()
    {
        yield return new WaitUntil(() => dieSpawner.GetUsedDice().Count == 0);

        // Keep spawning healthy objects forever
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        float angle = Random.Range(0f,2*Mathf.PI);
        Vector2 spawnPos = new Vector2(radius*Mathf.Cos(angle), radius*Mathf.Sin(angle));
        GameObject obj = GameManager.Instance.GetHealthyPool.Get();

        if (obj != null ) {
            UpdateNumbers();

            obj.transform.position = spawnPos;
            obj.SetActive(true);
            var mover = obj.GetComponent<HealthyMove>();

            mover.GetComponent<CircleWander>().SetDirectionChangeInterval(procTime);
            mover.GetComponent<CircleWander>().StartWandering();

            mover.PartsValue = UIManager.Instance.LevelTracker;
            if (mover.PartsValue == 0)
            {
                mover.PartsValue = 1;
            }

            List<DieSettings> sourceDice = dieSpawner.GetDiceOfSource("Healthy");
            mover.AddDice(sourceDice);
            
            return;
        }
    }

    public void UpdateNumbers()
    {
        float.TryParse(procTimeText.text, out procTime);
        float.TryParse(spawnTimeText.text, out spawnInterval);

    }
}
