using Scripts.Managers;
using Scripts.UI.Popups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Scripts.Views
{
    public class MainMenuView : View
    {
        private MainMenuTabItemList _tabItemList;
        private List<View> _subViews;
        private View _currentView;

        public async override Task EnterView()
        {
            await base.EnterView();

            var backGround = Utils.FindGameObject("Background", gameObject);
            var arf = backGround.GetComponent<AspectRatioFitter>();
            var texture = backGround.GetComponent<Image>().mainTexture;
            arf.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            arf.aspectRatio = texture.width / (float)texture.height;


            var content = Utils.FindGameObject("Content", gameObject);
            var homeTabs = Utils.FindGameObject("HomeTabs", gameObject);
            var settingsButton = Utils.FindGameObject("SettingsButton", gameObject);
            content.GetComponent<RectTransform>().SetHeight(Device.Height * 0.72f);
            // Hide settings button in main menu (only show in game view)
            settingsButton.SetActive(false);
            _subViews = new List<View>();
            foreach (Transform tabItem in content.transform)
            {
                if (tabItem.gameObject.TryGetComponent(out View view))
                    _subViews.Add(view);
            }
            homeTabs.transform.DestroyAllChildren();
            _tabItemList = await MainMenuTabItemList.CreateAsync(ItemController.instance.mainMenuTabDatas, homeTabs.transform, UpdateView);
            Settings.OnPurchase += UpdateBudgetUI;
            UpdateBudgetUI();
        }

        public View CurrentView => _currentView;

        private void UpdateBudgetUI()
        {
            var priceText = Utils.FindGameObject("PriceText", gameObject).GetComponent<TMP_Text>();
            priceText.text = Settings.User.budget.ToString("#,##0");
        }

        private async void UpdateView(MainMenuTabData tabData)
        {
            if (_subViews == null || _subViews.Count == 0)
            {
                Debug.LogWarning("There is no subviews");
                return;
            }
            if (tabData == null)
                return;

            if (_currentView is CarShopView carView)
            {
                var currentCarData = carView.GetCurrentSelectedCarData();
                if (currentCarData != null)
                {
                    if (!currentCarData.isOpened)
                    {

                        UnlockPopup.Create(currentCarData, () =>
                        {
                            var currentItem = carView.GetCurrentSelectedItem();
                            currentItem?.Reload();
                        });
                        return;
                    }
                    else
                        Settings.User.currentSelectedCar = currentCarData;
                }
            }

            if (_currentView is StagesShopView stageView)
            {
                var currentStageData = stageView.GetCurrentSelectedStageData();
                if (currentStageData != null)
                {
                    if (!currentStageData.isOpened)
                    {
                        UnlockPopup.Create(currentStageData, () =>
                        {
                            var currentItem = stageView.GetCurrentSelectedItem();
                            currentItem?.Reload();
                        });
                        return;
                    }
                    else
                        Settings.User.currentSelectedStage = currentStageData;
                }
            }

            if (_currentView != null)
                _currentView.PauseResumeView(false);
            else
            {
                foreach (View view in _subViews)
                {
                    view.PauseResumeView(view.GetViewName() == tabData.viewName);
                }
            }
            _currentView = GetSubViewByClassName(tabData.viewName);
            if (IsStartButton(tabData))
            {
                UIController.instance.EnterView<GameView>();
                return;
            }
            await _currentView.EnterView();
            _currentView.gameObject.SetActive(true);
        }

        private bool IsStartButton(MainMenuTabData tabData)
        {
            return tabData.viewName == "GameView";
        }

        private View GetSubViewByClassName(string className)
        {
            return _subViews.FirstOrDefault(v => v.GetViewName() == className);
        }

        public override void ExitView()
        {
            base.ExitView();
            Settings.OnPurchase -= UpdateBudgetUI;
            foreach (View view in _subViews)
            {
                if (view != null)
                    view.ExitView();
            }

            if (_tabItemList != null)
            {
                _tabItemList.Dispose();
                _tabItemList = null;
            }

            gameObject.SetActive(false);
        }
    }
}