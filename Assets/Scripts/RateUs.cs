using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
public class RateUs : MonoBehaviour
{
    public string androidUrl;

    public void Rate()
    {
#if UNITY_ANDROID
        Application.OpenURL(androidUrl);
#endif

#if UNITY_IOS
    Device.RequestStoreReview();
#endif
    }
}