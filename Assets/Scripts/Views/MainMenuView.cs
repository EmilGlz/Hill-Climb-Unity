using Scripts.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Scripts.Views
{
    public class MainMenuView : View
    {
        private MainMenuTabItemList _tabItemList;
        private List<View> _subViews;
        private View _currentView;

        public override void EnterView()
        {
            base.EnterView();
            var content = Utils.FindGameObject("Content", gameObject);
            var homeTabs = Utils.FindGameObject("HomeTabs", gameObject);
            _subViews = new List<View>();
            foreach (Transform tabItem in content.transform)
            {
                if (tabItem.gameObject.TryGetComponent(out View view))
                    _subViews.Add(view);
            }
            _tabItemList = new MainMenuTabItemList(ItemController.instance.mainMenuTabDatas, homeTabs.transform, UpdateView);
        }

        private void UpdateView(MainMenuTabData tabData)
        {
            if (_subViews == null || _subViews.Count == 0)
            {
                Debug.LogWarning("There is no subviews");
                return;
            }
            if (tabData == null)
                return;
            if (_currentView != null)
                _currentView.PauseResumeView(false);
            else
            {
                foreach (View view in _subViews) {
                    view.PauseResumeView(view.GetViewName() == tabData.viewName);
                }
            }
            _currentView = GetSubViewByClassName(tabData.viewName);
            if(IsStartButton(tabData))
            {
                UIController.instance.EnterView<GameView>();
                return;
            }
            _currentView.EnterView();
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
        }
    }
}