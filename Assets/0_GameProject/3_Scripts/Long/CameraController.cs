using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float padding = 2f;

    public float height = 10f;

    public void FitCamera(
        int gridWidth,
        int gridHeight,
        float cellSize)
    {
        Camera cam =
            Camera.main;

        float worldWidth =
            gridWidth * cellSize;

        float worldHeight =
            gridHeight * cellSize;

        Vector3 center =
            new Vector3(
                worldWidth / 2f - cellSize / 2f,
                0,
                worldHeight / 2f - cellSize / 2f);

        transform.position =
            center +
            new Vector3(
                0,
                height,
                0);

        transform.rotation =
            Quaternion.Euler(
                90f,
                0f,
                0);

        float aspect =
            (float)Screen.width /
            Screen.height;

        float verticalSize =
            worldHeight / 2f;

        float horizontalSize =
            worldWidth / aspect / 2f;

        cam.orthographicSize =
            Mathf.Max(
                verticalSize,
                horizontalSize)
            + padding;
    }
}