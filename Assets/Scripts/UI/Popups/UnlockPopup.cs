using DG.Tweening;
using Scripts.Managers;
using Scripts.UI.Popups;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnlockPopup : Popup
{
    protected override string PrefabName => "Prefabs/Popups/UnlockPopup";
    private readonly CarData _carData;
    private readonly StageData _stageData;
    private readonly Action _onBuy;

    public UnlockPopup(StageData data, Action onBuy, Action onClose = null, Action onPopupShowed = null, bool addBackground = true, bool closeOnBackground = true)
        : base(onClose, onPopupShowed, addBackground, closeOnBackground)
    {
        _carData = null;
        _stageData = data;
        _onBuy = onBuy;
    }

    public UnlockPopup(CarData car, Action onBuy, Action onClose = null, Action onPopupShowed = null, bool addBackground = true, bool closeOnBackground = true)
    : base(onClose, onPopupShowed, addBackground, closeOnBackground)
    {
        _carData = car;
        _stageData = null;
        _onBuy = onBuy;
    }

    public static void Create(StageData data, Action onBuy)
    {
        if (PopupsManager.IsShown<UnlockPopup>())
            return;
        var popup = new UnlockPopup(data, onBuy);
        popup.Show();
    }

    public static void Create(CarData data, Action onBuy)
    {
        if (PopupsManager.IsShown<UnlockPopup>())
            return;
        var popup = new UnlockPopup(data, onBuy);
        popup.Show();
    }

    private bool IsCarItem => _carData != null;

    protected override void Show()
    {
        BackButton = Utils.FindGameObject("CloseButton", ItemTemplate).GetComponent<Button>();

        var title = Utils.FindGameObject("ContentTitle", ItemTemplate).GetComponent<TMP_Text>();

        var description = Utils.FindGameObject("ContentDescription", ItemTemplate).GetComponent<TMP_Text>();

        var icon = Utils.FindGameObject("Icon", ItemTemplate).GetComponent<Image>();

        var priceText = Utils.FindGameObject("PriceText", ItemTemplate).GetComponent<TMP_Text>();

        var buyButton = Utils.FindGameObject("BuyButton", ItemTemplate).GetComponent<Button>();

        if (IsCarItem)
        {
            title.text = _carData.itemName;
            description.text = _carData.description;
            icon.sprite = _carData.icon;
            priceText.text = _carData.price.ToString();
        }
        else
        {
            title.text = _stageData.itemName;
            description.text = _stageData.description;
            icon.sprite = _stageData.icon;
            priceText.text = _stageData.price.ToString();
        }

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(Buy);
        base.Show();
    }

    private void Buy()
    {
        var buyButton = Utils.FindGameObject("BuyButton", ItemTemplate).GetComponent<Button>();
        var errorText = Utils.FindGameObject("ErrorText", ItemTemplate).GetComponent<TMP_Text>();
        if (IsCarItem)
        {
            if (Settings.User.budget >= _carData.price)
            {
                Settings.User.currentSelectedCar = _carData;
                Settings.User.budget -= _carData.price;
                _carData.isOpened = true;
                _onBuy.Invoke();
                Settings.OnPurchase?.Invoke();
                Dispose();
            }
            else
            {
                errorText.text = "Not enough funds!";
                errorText.color = new Color(255, 0, 0, 0);
                errorText.gameObject.SetActive(true);
                errorText.DOColor(Color.red, 0.5f);
                buyButton.interactable = false;
            }
        }
        else
        {
            if (Settings.User.budget >= _stageData.price)
            {
                Settings.User.currentSelectedStage = _stageData;
                Settings.User.budget -= _stageData.price;
                _stageData.isOpened = true;
                _onBuy.Invoke();
                Dispose();
            }
            else
            {
                errorText.text = "Not enough funds!";
                errorText.color = new Color(255, 0, 0, 0);
                errorText.gameObject.SetActive(true);
                errorText.DOColor(Color.red, 0.5f);
                buyButton.interactable = false;
            }
        }
        // TODO if we have money, but car. Else, show notification of not enough funds
    }
}
