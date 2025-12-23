using UnityEngine;

public class ImgSlider : MonoBehaviour
{
    public RectTransform parentImage;
    public RectTransform sliderImage;

    public float speed = 2f;
    public float padding = 10f;

    float minY;
    float maxY;
    float time;

    bool isMoving;

    public float storedValue;

    void Start()
    {
        CalculateLimits();
        ResetSlider();
        StartSlider();
    }

    void Update()
    {
        if (!isMoving) return;

        time += Time.deltaTime * speed;

        float t = Mathf.Sin(time);
        float yPos = Mathf.Lerp(minY, maxY, (t + 1f) * 0.5f);

        Vector2 pos = sliderImage.anchoredPosition;
        pos.y = yPos;
        sliderImage.anchoredPosition = pos;
    }

    void CalculateLimits()
    {
        float parentHeight = parentImage.rect.height;
        float sliderHeight = sliderImage.rect.height;

        minY = -parentHeight / 2 + sliderHeight / 2 + padding;
        maxY = parentHeight / 2 - sliderHeight / 2 - padding;
    }

    public void StartSlider()
    {
        isMoving = true;
    }

    public void StopAndStore()
    {
        isMoving = false;
        storedValue = Mathf.InverseLerp(minY, maxY, sliderImage.anchoredPosition.y) * 2f - 1f;
        Debug.Log("Stored Slider Value: " + storedValue);
    }

    public void ResetSlider()
    {
        isMoving = false;
        time = 0f;

        Vector2 pos = sliderImage.anchoredPosition;
        pos.y = 0f;
        sliderImage.anchoredPosition = pos;

        storedValue = 0f;
        StartSlider();
    }

    public float GetValue()
    {
        return (1f - Mathf.Abs(storedValue));
    }
}
