using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Popup : IDisposable
{
    private readonly Action _onClose;
    private readonly bool _addBackground;
    private readonly bool _closeOnBackground;
    public GameObject ItemTemplate;
    private const string _backgroundImagePrefab = "Prefabs/Popups/PopupBackground";
    protected GameObject BackgroundImage;
    protected Popup(string prefabName, Action onClose = null, bool addBackground = true, bool closeOnBackground = true)
    {
        ItemTemplate = ResourceHelper.InstantiatePrefab(prefabName, UIController.instance.PopupCanvas);
        if (ItemTemplate != null)
            PopupsManager.instance.AddPopup(this);
        _onClose = onClose;
        _addBackground = addBackground;
        _closeOnBackground = closeOnBackground;
    }
    public virtual void Dispose()
    {
        PopupsManager.instance.RemovePopup(this);
        if(BackgroundImage != null)
            UnityEngine.Object.Destroy(BackgroundImage);
        if(ItemTemplate  != null)
            UnityEngine.Object.Destroy(ItemTemplate);
        _onClose?.Invoke();
    }

    protected virtual void Show()
    {
        BackgroundImage = ResourceHelper.InstantiatePrefab(_backgroundImagePrefab, UIController.instance.PopupCanvas);
        BackgroundImage.transform.SetAsFirstSibling();
        BackgroundImage.name = GetType().Name + " Background";
        BackgroundImage.GetComponent<Image>().color = new Color(0, 0, 0, (_addBackground ? .5f : 0f));
        if (_closeOnBackground)
            BackgroundImage.GetComponent<Button>().onClick.AddListener(Dispose);
    }
}
