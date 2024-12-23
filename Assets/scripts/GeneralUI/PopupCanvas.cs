using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PopupCanvas : MonoBehaviour
{
    [SerializeField] PopupWindow PopupWindowprefab;
    [SerializeField] UnityEngine.UI.Image Penal;
    List<Window> windows;

    // Start is called before the first frame update
    void Start()
    {
        windows = new List<Window>();
    }

    // Update is called once per frame

    public PopupWindow CallPopupWindow(bool EnableCloseButton, string TitlebarText, bool EnableApplyButton, bool EnableCancelButton, string MainText)
    {
        PopupWindow result = Instantiate(PopupWindowprefab, Vector2.zero, Quaternion.identity, gameObject.transform);
        windows.Add(result);
        windowsUpdate();
        result.UEOnDestroy.AddListener((window) =>
        {
            windows.Remove(window);
            windowsUpdate();
        });

        result.Init(EnableCloseButton, TitlebarText, EnableApplyButton, EnableCancelButton, MainText);

        return result;
    }
    void windowsUpdate()
    {
        if (windows.Count == 0)
        {
            Penal.gameObject.SetActive(false);

        }
        else
        {
            Penal.gameObject.SetActive(true);
        }
    }
}
