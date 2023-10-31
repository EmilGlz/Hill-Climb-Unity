using DG.Tweening;
using Scripts.Managers;
using Scripts.UI.Popups;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockCarPopup : Popup
{
    private const string _prefabName = "Prefabs/Popups/UnlockCarPopup";
    private readonly CarData _carData;

    public UnlockCarPopup(CarData car, Action onClose = null, Action onPopupShowed = null, bool addBackground = true, bool closeOnBackground = true) 
        : base(_prefabName, onClose, onPopupShowed, addBackground, closeOnBackground)
    {
        _carData = car;
    }

    public static void Create(CarData car)
    {
        if (PopupsManager.IsShown<UnlockCarPopup>())
            return;
        var popup = new UnlockCarPopup(car);
        popup.Show();
    }

    protected override void Show()
    {
        base.Show();
        // TODO show car datas

        BackButton = Utils.FindGameObject("CloseButton", ItemTemplate).GetComponent<Button>();

        var title = Utils.FindGameObject("CarTitle", ItemTemplate).GetComponent<TMP_Text>();
        title.text = _carData.itemName;

        var description = Utils.FindGameObject("CarDescription", ItemTemplate).GetComponent<TMP_Text>();
        description.text = _carData.description;

        var icon = Utils.FindGameObject("CarIcon", ItemTemplate).GetComponent<Image>();
        icon.sprite = _carData.icon;

        var priceText = Utils.FindGameObject("PriceText", ItemTemplate).GetComponent<TMP_Text>();
        priceText.text = _carData.price.ToString();

        var buyButton = Utils.FindGameObject("BuyButton", ItemTemplate).GetComponent<Button>();
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyCar);
    }

    private void BuyCar()
    {
        if (Settings.User.budget >= _carData.price)
        {
            Settings.User.currentSelectedCar = _carData;
            Settings.User.budget -= _carData.price;
            Dispose();
        }
        else
        {
            var buyButton = Utils.FindGameObject("BuyButton", ItemTemplate).GetComponent<Button>();
            var errorText = Utils.FindGameObject("ErrorText", ItemTemplate).GetComponent<TMP_Text>();
            errorText.text = "Not enough funds!";
            errorText.color = new Color(255, 0, 0, 0);
            errorText.gameObject.SetActive(true);
            errorText.DOColor(Color.red, 0.5f);
            buyButton.interactable = false;
        }
        // TODO if we have money, but car. Else, show notification of not enough funds
    }
}
