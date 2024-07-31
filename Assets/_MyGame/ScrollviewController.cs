using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScrollviewController : MonoBehaviour
{
    public TMP_Text textTrack;
    public RectTransform borderPoint;
    public ContentItem[] contentItems;
    public RectTransform currentRectTransform;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < contentItems.Length; i++)
        {
            var currentItem = contentItems[i];

            if (currentItem.isPin)
            {
                var previousItem = contentItems[i - 4];
                var point = currentRectTransform.InverseTransformPoint(borderPoint.position);
                var isBetween = IsNumberBetween(point.y, previousItem.GetComponent<RectTransform>().localPosition.y,
                    currentItem.GetComponent<RectTransform>().localPosition.y);
                if (isBetween)
                {
                    Debug.Log($"Between {previousItem.index} {currentItem.index}");
                    textTrack.SetText($"{currentItem.index}");
                }
            }
        }
    }

    [ContextMenu("UpdateChildIndex")]
    void UpdateChildIndex()
    {
        contentItems = transform.GetComponentsInChildren<ContentItem>();
        for (int i = 0; i < contentItems.Length; i++)
        {
            contentItems[i].index = i + 1;
        }
    }

    [ContextMenu("CheckPin")]
    void CheckPin()
    {
        for (int i = 0; i < contentItems.Length; i++)
        {
            if (contentItems[i].index % 5 == 0)
            {
                contentItems[i].isPin = true;
            }
        }
    }

    [ContextMenu("CheckPoint")]
    void CheckPoint()
    {
        var point = currentRectTransform.InverseTransformPoint(borderPoint.position);
        Debug.Log($"{contentItems[0].GetComponent<RectTransform>().localPosition}");
        Debug.Log($"{point}");
        Debug.Log($"{contentItems[4].GetComponent<RectTransform>().localPosition}");
        // var isBetween = (contentItems[0].GetComponent<RectTransform>().localPosition.y <= point.y
        //  && point.y <= contentItems[4].GetComponent<RectTransform>().localPosition.y) ||
        //  (contentItems[0].GetComponent<RectTransform>().localPosition.y >= point.y
        //     && point.y >= contentItems[4].GetComponent<RectTransform>().localPosition.y);
        var isBetween = IsNumberBetween(point.y, contentItems[0].GetComponent<RectTransform>().localPosition.y,
            contentItems[4].GetComponent<RectTransform>().localPosition.y);
        Debug.Log(isBetween);

    }

    public bool IsNumberBetween(float num, float bound1, float bound2)
    {
        float lower = Mathf.Min(bound1, bound2);
        float upper = Mathf.Max(bound1, bound2);
        return num >= lower && num <= upper;
    }


    // Checks if point P is between points A and B
    public bool IsPointBetween(Vector2 A, Vector2 B, Vector2 P)
    {
        // Check if the points are collinear by using the cross product
        Vector2 AB = B - A;
        Vector2 AP = P - A;

        float crossProduct = AB.x * AP.y - AB.y * AP.x;
        if (Mathf.Abs(crossProduct) > Mathf.Epsilon)
        {
            return false; // P is not collinear with A and B
        }

        // Check if P is within the bounds of A and B
        float dotProduct = Vector2.Dot(AP, AB);
        if (dotProduct < 0)
        {
            return false; // P is outside the segment AB, on the side of A
        }

        float squaredLengthAB = AB.sqrMagnitude;
        if (dotProduct > squaredLengthAB)
        {
            return false; // P is outside the segment AB, on the side of B
        }

        return true; // P is between A and B
    }
}
