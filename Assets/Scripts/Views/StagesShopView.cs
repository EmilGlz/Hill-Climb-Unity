using Scripts.Items;
using Scripts.Managers;
using Scripts.UI;

namespace Scripts.Views
{
    public class StagesShopView : View
    {
        public static string PrefabPath = "Prefabs/Views/ShopScrollView";
        private StagesList _stageList;
        private ScrollScaleController _scroll;

        public override void EnterView()
        {
            _scroll = gameObject.GetComponentInChildren<ScrollScaleController>();
            if (_scroll == null)
                _scroll = ResourceHelper.InstantiatePrefab(PrefabPath, transform).GetComponent<ScrollScaleController>();

            if (_stageList != null)
            {
                PauseResumeView(true);
                return;
            }
            base.EnterView();
            var content = Utils.FindGameObject("Content", _scroll.gameObject);
            _stageList = new StagesList(ItemController.instance.userData.stages, content.transform, _scroll);
            Utils.RunAsync(() =>
            {
                _scroll.InitItems(_stageList.Items);
            });
        }

        public override void ExitView()
        {
            base.ExitView();
            if (_stageList != null)
            {
                _stageList.Dispose();
                _stageList = null;
            }
            if (_scroll != null)
            {
                _scroll.Dispose();
                _scroll = null;
            }

            ScrollScaleController scrollView = gameObject.GetComponentInChildren<ScrollScaleController>();
            if (scrollView != null)
                Destroy(scrollView.gameObject);
        }
    }
}