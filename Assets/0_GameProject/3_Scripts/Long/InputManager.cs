using UnityEngine;

public class InputManager : MonoBehaviour
{
    private AnimalPiece selectedAnimal;

    private Vector2 startMousePos;

    private bool isDragging;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    void StartDrag()
    {
        startMousePos = Input.mousePosition;

        Ray ray =
            Camera.main.ScreenPointToRay(
                Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            AnimalPiece animal =
                hit.collider.GetComponent<AnimalPiece>();

            if (animal != null)
            {
                selectedAnimal = animal;

                isDragging = true;
            }
        }
    }

    void EndDrag()
    {
        if (!isDragging)
            return;

        Vector2 endMousePos =
            Input.mousePosition;

        Vector2 delta =
            endMousePos - startMousePos;

        Vector2Int moveDir =
            GetDirection(delta);

        selectedAnimal.SlideMove(moveDir);

        isDragging = false;

        selectedAnimal = null;
    }

    Vector2Int GetDirection(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) >
            Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
                return Vector2Int.right;
            else
                return Vector2Int.left;
        }
        else
        {
            if (delta.y > 0)
                return Vector2Int.up;
            else
                return Vector2Int.down;
        }
    }
}
