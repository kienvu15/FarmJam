using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private PlayerControl PlayerControl;
    public GameObject cube;
    private void Awake()
    {
        PlayerControl = new PlayerControl();
        // PlayerControl.Player.Newaction.performed += _ => Animation.instance.A(cube);
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        if (PlayerControl != null)
        {
            PlayerControl.Enable();
        }
    }
}