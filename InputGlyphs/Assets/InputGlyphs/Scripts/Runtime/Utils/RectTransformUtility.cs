using UnityEngine;

public static class RectTransformUtility
{
    // https://discussions.unity.com/t/test-if-ui-element-is-visible-on-screen/554949/36
    public static bool IsBecomeVisible(this RectTransform rectTransform)
    {
        Vector3[] v = new Vector3[4];
        rectTransform.GetWorldCorners(v);

        float maxY = Mathf.Max(v[0].y, v[1].y, v[2].y, v[3].y);
        float minY = Mathf.Min(v[0].y, v[1].y, v[2].y, v[3].y);

        if (maxY < 0 || minY > Screen.height)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
