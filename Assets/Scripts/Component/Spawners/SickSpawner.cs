using System.Collections.Generic;
using UnityEngine;


public class SickSpawner : MonoBehaviour
{
    public void SpawnSicko(Vector2 spawnPos, Vector2 spawnDir, int partsValue)
    {
        GameObject obj = GameManager.Instance.GetSickPool.Get();
        if (obj != null)
        {
            obj.transform.position = spawnPos;
            obj.SetActive(true);
            CircleMove mover = obj.GetComponent<CircleMove>();
            mover.PartsValue = partsValue;
            mover.SetMoveDirection(spawnDir);
            mover.GetComponent<CircleWander>().StartWandering();

            DieSpawner dieSpawner = UIManager.Instance.GetComponent<DieSpawner>();
            List<DieSettings> sourceDice = dieSpawner.GetDiceOfSource("Sickness");

            mover.AddDice(sourceDice);
            return;
        }
    }

}
