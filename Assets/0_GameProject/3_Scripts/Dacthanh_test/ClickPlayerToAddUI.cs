using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClickPlayerToAddUI : MonoBehaviour
{
    public Transform scrollViewContent;
    public GameObject imagePrefab;

    private readonly List<GameObject> listAnimalImage = new();
    private readonly HashSet<GameObject> mergingImages = new(); // ← THÊM: track ảnh đang merge

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        if (!hit.collider.CompareTag("Player")) return;

        if (CheckLost(hit.collider.GetComponent<Animal>()))
            Add(hit.collider.GetComponent<Animal>());
        else
            Debug.Log("You are lost");
    }

    private bool CheckLost(Animal animal)
    {
        return listAnimalImage.Count < 4 ||
               listAnimalImage.Any(x => x.GetComponent<IdOfImage>().Id == animal.Id);
    }

    private void Add(Animal animal)
    {
        GameObject matchedImage = null;
        int insertIndex = -1;

        for (int i = 0; i < listAnimalImage.Count; i++)
        {
            if (listAnimalImage[i] == null) continue;

            // ← THÊM: bỏ qua ảnh đang trong quá trình merge
            if (mergingImages.Contains(listAnimalImage[i])) continue;

            if (listAnimalImage[i].GetComponent<IdOfImage>().Id == animal.Id)
            {
                matchedImage = listAnimalImage[i];
                insertIndex = i + 1;
                break;
            }
        }

        var image = Instantiate(imagePrefab, scrollViewContent);
        image.GetComponent<Image>().sprite = animal.sprite;
        image.GetComponent<IdOfImage>().Id = animal.Id;

        if (insertIndex >= 0)
        {
            image.transform.SetSiblingIndex(insertIndex);
            listAnimalImage.Insert(insertIndex, image);

            // ← THÊM: đánh dấu cả 2 đang merge
            mergingImages.Add(image);
            mergingImages.Add(matchedImage);

            StartCoroutine(AddAndMerge(image, matchedImage));
        }
        else
        {
            listAnimalImage.Add(image);

            RectTransform rect = image.GetComponent<RectTransform>();
            rect.localScale = Vector3.zero;

            Sequence seq = DOTween.Sequence();
            seq.Append(rect.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
            seq.Join(rect.DOShakeRotation(0.5f, new Vector3(0, 0, 4), 8, 90));
        }
    }

    private IEnumerator AddAndMerge(GameObject newImage, GameObject oldImage)
    {
        RectTransform rect = newImage.GetComponent<RectTransform>();
        rect.localScale = Vector3.zero;

        Sequence appear = DOTween.Sequence();
        appear.Append(rect.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        appear.Join(rect.DOShakeRotation(0.5f, new Vector3(0, 0, 4), 8, 90));

        yield return appear.WaitForCompletion();

        yield return Merge(newImage, oldImage);
    }

    public IEnumerator Merge(GameObject image1, GameObject image2)
    {
        if (image1 == null || image2 == null)
        {
            // ← THÊM: cleanup nếu bị null
            mergingImages.Remove(image1);
            mergingImages.Remove(image2);
            yield break;
        }

        RectTransform rect1 = image1.GetComponent<RectTransform>();
        RectTransform rect2 = image2.GetComponent<RectTransform>();

        Vector2 pos1 = rect1.anchoredPosition;
        Vector2 pos2 = rect2.anchoredPosition;
        Vector2 middle = (pos1 + pos2) * 0.5f;

        Sequence merge = DOTween.Sequence();
        merge.Join(rect1.DOAnchorPos(middle, 0.25f).SetEase(Ease.InQuad));
        merge.Join(rect2.DOAnchorPos(middle, 0.25f).SetEase(Ease.InQuad));
        merge.Join(rect1.DOShakeRotation(0.25f, new Vector3(0, 0, 2), 6, 90));
        merge.Join(rect2.DOShakeRotation(0.25f, new Vector3(0, 0, 3), 8, 90));

        yield return merge.WaitForCompletion();

        listAnimalImage.Remove(image1);
        mergingImages.Remove(image1); // ← THÊM
        Destroy(image1);

        rect2.localRotation = Quaternion.identity;

        Sequence success = DOTween.Sequence();
        success.Append(rect2.DOScale(1.15f, 0.15f));
        success.Append(rect2.DOScale(1f, 0.15f));
        success.Join(rect2.DOShakeRotation(0.5f, new Vector3(0, 0, 4), 10, 90));

        yield return success.WaitForCompletion();

        listAnimalImage.Remove(image2);
        mergingImages.Remove(image2); // ← THÊM
        Destroy(image2);
    }
}