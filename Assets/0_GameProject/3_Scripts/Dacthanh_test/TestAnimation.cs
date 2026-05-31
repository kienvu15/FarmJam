using System;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    private PlayerControl PlayerControl;
    public GameObject animal;
    public int i = 0;
    private void Awake()
    {
        PlayerControl = new PlayerControl();
        PlayerControl.Player.Newaction.performed += _ =>
        {
            animal.GetComponent<Animation>().StopMove();
            i++;
            if (i > 6) i = 0;
            PlayAnimation(i); // ✅ Gọi thẳng tại đây, chỉ 1 lần
        };
        var anim = animal.GetComponent<Animation>();
        PlayerControl.Player.test.performed += _ => anim.move();
    }
    private void PlayAnimation(int index)
    {
        var anim = animal.GetComponent<Animation>();
        switch (index)
        {
            case 0: anim.Stand();            break;
            case 1: anim.Stand_Animation1(); break;
            case 2: anim.Stand_Animation2(); break;
            case 3: anim.Stand_Animation3(); break;
            case 4: anim.Stand_Animation4(); break;
            case 5: anim.Stand_Animation5(); break;
            // case 6: anim.hit(); break;
            
        }
    }

    private void Start()
    {
        // ✅ Stand luôn được kích hoạt từ đầu
        animal.GetComponent<Animation>().Stand();
    }

    private void OnEnable()
    {
        if (PlayerControl != null)
            PlayerControl.Enable();
    }
}