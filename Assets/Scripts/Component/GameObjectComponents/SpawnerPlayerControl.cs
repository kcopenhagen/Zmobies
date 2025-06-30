using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpawnerPlayerControl : MonoBehaviour
{
    [SerializeField] private GameObject pauseMarker;

    private Collider2D col;
    private Camera mainCamera;
    private bool paused = true;
    public void Awake()
    {
        col = GetComponent<Collider2D>();
        mainCamera = Camera.main;
    }
    public bool IsTouched(Vector2 screenPosition)
    {
        if (col == null || mainCamera == null) return false;

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPosition);
        return col.OverlapPoint(worldPos);
    }

    public void Touched()
    {
        if (!paused)
        {
            GetComponent<Spawner>().ResetSpawnInterval(99999f);
            pauseMarker.SetActive(true);
            paused = !paused;
        }
        else
        {
            GetComponent<Spawner>().ResetSpawnInterval(-1f);
            pauseMarker.SetActive(false);
            paused = !paused;
        }
    }
}
