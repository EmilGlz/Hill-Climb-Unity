using Scripts.Items;
using Scripts.Managers;
using Scripts.UI;

namespace Scripts.Views
{
    public class StagesShopView : View
    {
        public static string PrefabPath = "Prefabs/Views/ShopScrollView";
        private StagesList _stageList;

        public override void EnterView()
        {
            ScrollScaleController scrollView = gameObject.GetComponentInChildren<ScrollScaleController>();
            if (scrollView == null)
                scrollView = ResourceHelper.InstantiatePrefab(PrefabPath, transform).GetComponent<ScrollScaleController>();

            if (_stageList != null)
            {
                PauseResumeView(true);
                return;
            }
            base.EnterView();
            var content = Utils.FindGameObject("Content", scrollView.gameObject);
            _stageList = new StagesList(ItemController.instance.userData.stages, content.transform);
            Utils.RunAsync(() =>
            {
                ScrollScaleController.instance.InitItems();
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

            ScrollScaleController scrollView = gameObject.GetComponentInChildren<ScrollScaleController>();
            if (scrollView != null)
                Destroy(scrollView.gameObject);
        }
    }
}