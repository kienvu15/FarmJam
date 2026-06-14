using UnityEngine;
using DG.Tweening;

public class Animation : MonoBehaviour
{
    private Sequence standAnim;
    private Sequence StandAnim1;
    private Sequence standAnim2;
    private Sequence standAnim3;
    private Sequence standAnim4;
    private Sequence standAnim5;
    private Sequence runAnim;
    private Sequence hitAnim;

    private int lifeTime = 3;
    public float speed = 3f;

    private Vector3 initPos;
    private Vector3 initRot;
    private Vector3 initScale;

    private bool isMoving = false;

    private void Awake()
    {
        initPos   = transform.localPosition;
        initRot   = transform.localEulerAngles;
        initScale = transform.localScale;
        Stand();
    }

    private void ResetAndStand()
    {
        Transform t = gameObject.transform;
        KillAll();

        t.DOLocalMove(initPos, 0.5f).SetEase(Ease.InOutSine);
        t.DOLocalRotate(initRot, 0.5f).SetEase(Ease.InOutSine);
        t.DOScale(initScale, 0.5f).SetEase(Ease.InOutSine)
            .OnComplete(Stand);
    }

    // ===== STAND =====
    public void Stand()
    {
        if (isMoving) return; // Không stand khi đang move

        Transform t = gameObject.transform;
        KillAll();
        standAnim = DOTween.Sequence();

        standAnim.Join(
            t.DOLocalMoveY(initPos.y - 0.04f, 0.4f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
        );
        standAnim.Join(
            t.DOScaleX(initScale.x * 1.04f, 0.4f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
        );
        standAnim.Join(
            t.DOScaleY(initScale.y * 0.97f, 0.4f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
        );
    }

    // ===== STAND ANIMATIONS =====
    public void Stand_Animation1()
    {
        if (isMoving) return;

        Transform t = gameObject.transform;
        KillAll();
        StandAnim1 = DOTween.Sequence();

        StandAnim1.Join(
            t.DOScaleY(initScale.y * 1.08f, 0.6f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
        );
        StandAnim1.Join(
            t.DOLocalMoveY(initPos.y + 0.1f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
        );

        Sequence rockSeq = DOTween.Sequence().SetId("rockSeq");
        rockSeq.Append(t.DOLocalRotate(new Vector3(0, 0, -6f), 0.6f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine));
        rockSeq.Append(t.DOLocalRotate(new Vector3(0, 0,  6f), 0.6f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine));
        rockSeq.Append(t.DOLocalRotate(new Vector3(0, 0,  6f), 0.6f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine));
        rockSeq.Append(t.DOLocalRotate(new Vector3(0, 0, -6f), 0.6f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine));
        rockSeq.SetLoops(-1);
        StandAnim1.Join(rockSeq);

        DOVirtual.DelayedCall(lifeTime, ResetAndStand);
    }

    public void Stand_Animation2()
    {
        if (isMoving) return;
        Transform t = gameObject.transform;
        KillAll();
        standAnim2 = DOTween.Sequence();

        standAnim2.Append(t.DOLocalRotate(new Vector3(initRot.x, -60f, initRot.z), 0.5f).SetEase(Ease.OutBack));
        standAnim2.Join(t.DOScaleY(initScale.y * 0.97f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine));
        standAnim2.Join(t.DOLocalMoveY(initPos.y - 0.03f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine));

        DOVirtual.DelayedCall(lifeTime, ResetAndStand);
    }

    public void Stand_Animation3()
    {
        if (isMoving) return;
        Transform t = gameObject.transform;
        KillAll();
        standAnim3 = DOTween.Sequence();

        standAnim3.Append(t.DOLocalRotate(new Vector3(initRot.x, 60f, initRot.z), 0.6f).SetEase(Ease.OutBack));
        standAnim3.Join(t.DOScaleY(initScale.y * 0.97f, 0.6f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine));
        standAnim3.Join(t.DOLocalMoveY(initPos.y - 0.03f, 0.6f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine));

        DOVirtual.DelayedCall(lifeTime, ResetAndStand);
    }

    public void Stand_Animation4()
    {
        if (isMoving) return;
        Transform t = gameObject.transform;
        KillAll();
        standAnim4 = DOTween.Sequence();

        standAnim4.Join(t.DOLocalMoveY(initPos.y - 0.03f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine));

        Sequence lookSeq = DOTween.Sequence();
        lookSeq.Append(t.DOLocalRotate(new Vector3(initRot.x, -60f, initRot.z), 0.6f).SetEase(Ease.InOutSine));
        lookSeq.AppendInterval(0.8f);
        lookSeq.Append(t.DOLocalRotate(new Vector3(initRot.x, initRot.y, initRot.z), 0.6f).SetEase(Ease.InOutSine));
        lookSeq.Append(t.DOLocalRotate(new Vector3(initRot.x, 60f, initRot.z), 0.6f).SetEase(Ease.InOutSine));
        lookSeq.AppendInterval(0.8f);
        lookSeq.Append(t.DOLocalRotate(new Vector3(initRot.x, initRot.y, initRot.z), 0.6f).SetEase(Ease.InOutSine));
        lookSeq.SetLoops(-1);
        standAnim4.Join(lookSeq);

        DOVirtual.DelayedCall(lifeTime, ResetAndStand);
    }

    public void Stand_Animation5()
    {
        if (isMoving) return;
        Transform t = gameObject.transform;
        KillAll();
        standAnim5 = DOTween.Sequence();

        standAnim5.Append(t.DOLocalRotate(new Vector3(-60f, initRot.y, initRot.z), 0.6f).SetEase(Ease.OutBack));
        standAnim5.Join(t.DOScaleZ(initScale.z * 1.5f, 0.6f).SetEase(Ease.OutBack));
        standAnim5.Join(t.DOLocalMoveY(initPos.y + 0.1f, 0.6f).SetEase(Ease.OutBack));
        standAnim5.Join(t.DOScaleY(initScale.y * 0.97f, 0.6f).SetDelay(0.4f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine));

        DOVirtual.DelayedCall(lifeTime, ResetAndStand);
    }

    // ===== MOVE + RUN =====
    private void Run()
    {
        Transform t = gameObject.transform;
        runAnim = DOTween.Sequence();

        // Nhún lên xuống nhanh như đang chạy
        runAnim.Join(
            t.DOLocalMoveY(initPos.y + 0.12f, 0.2f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
        );

        // Nghiêng về phía trước khi chạy
        runAnim.Join(
            t.DOLocalRotate(new Vector3(initRot.x - 15f, initRot.y, initRot.z), 0.15f)
                .SetEase(Ease.OutQuad)
        );

        // Squash & Stretch nhanh khi chạy
        runAnim.Join(
            t.DOScaleY(initScale.y * 1.1f, 0.2f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
        );
        runAnim.Join(
            t.DOScaleX(initScale.x * 0.95f, 0.2f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
        );
    }

    public void move()
    {
        if (isMoving) return;
        isMoving = true;

        KillAll();
        Run();

        // ✅ Di chuyển vô hạn theo Z, không có OnComplete
        gameObject.transform.DOLocalMoveZ(initPos.z + 999f, 999f / speed)
            .SetEase(Ease.Linear);
    }

// ✅ Gọi từ bên ngoài để dừng move
    public void StopMove()
    {
        if (!isMoving) return;
        isMoving = false;
        KillAll();

        Transform t = gameObject.transform;
        t.DOLocalMove(initPos, 0.3f).SetEase(Ease.OutQuad);
        t.DOLocalRotate(initRot, 0.3f).SetEase(Ease.OutQuad);
        t.DOScale(initScale, 0.3f).SetEase(Ease.OutQuad)
            .OnComplete(Stand);
    }

    // ===== HIT =====
    public void hit()
    {
        int hitLifeTime = 1;
        Transform t = gameObject.transform;

        KillAll();
        isMoving = false; // Dừng move nếu đang chạy

        hitAnim = DOTween.Sequence();

        // Rung lắc nhanh khi bị hit
        hitAnim.Append(
            t.DOShakePosition(0.3f, strength: 0.15f, vibrato: 20, randomness: 0)
        );

        // Đổi màu đỏ (nếu có Renderer)
        var renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            hitAnim.Join(
                renderer.material.DOColor(Color.red, 0.1f)
                    .SetLoops(4, LoopType.Yoyo)
            );
        }

        // Bẹp xuống rồi bật lại
        hitAnim.Join(
            t.DOScaleY(initScale.y * 0.6f, 0.1f)
                .SetLoops(4, LoopType.Yoyo)
                .SetEase(Ease.OutBounce)
        );

        // Hết hit → về Stand
        DOVirtual.DelayedCall(hitLifeTime, () =>
        {
            KillAll();
            t.DOLocalRotate(initRot, 0.3f);
            t.DOScale(initScale, 0.3f).SetEase(Ease.OutBack)
                .OnComplete(Stand);
        });
    }

    // ===== KILL ALL =====
    private void KillAll()
    {
        StandAnim1?.Kill(true);
        standAnim?.Kill(true);
        standAnim2?.Kill(true);
        standAnim3?.Kill(true);
        standAnim4?.Kill(true);
        standAnim5?.Kill(true);
        runAnim?.Kill(true);
        hitAnim?.Kill(true);
        DOTween.Kill("rockSeq");
        DOTween.Kill(gameObject.transform);
    }

    private void OnDestroy()
    {
        KillAll();
    }
}