using Scripts.Items;
using Scripts.Managers;
using Scripts.UI;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Scripts.Views
{
    public class StagesShopView : View
    {
        public static string PrefabPath = "Prefabs/Views/ShopScrollView";
        private StagesList _stageList;
        private ScrollScaleController _scroll;

        public override async Task EnterView()
        {
            await base.EnterView();
            _scroll = gameObject.GetComponentInChildren<ScrollScaleController>();
            if (_scroll == null)
                _scroll = ResourceHelper.InstantiatePrefab(PrefabPath, transform).GetComponent<ScrollScaleController>();

            if (_stageList != null)
            {
                PauseResumeView(true);
                return;
            }
            var content = Utils.FindGameObject("Content", _scroll.gameObject);
            content.transform.DestroyAllChildren();
            _stageList = await StagesList.CreateAsync(ItemController.instance.userData.stages, content.transform, _scroll);
            Utils.RunAsync(() =>
            {
                _scroll.InitItems(_stageList.Items);
                StartCoroutine(_scroll.ScrollTo(_stageList.Items.FirstOrDefault(i => i.Data is StageData stageData && stageData == Settings.User.currentSelectedStage)));
            });
        }

        public StageData GetCurrentSelectedStageData()
        {
            var data = _scroll?.CurrentSelectedItem?.Data;
            return data == null ? null : (StageData)data;
        }

        public StageItem GetCurrentSelectedItem()
        {
            if (_scroll.CurrentSelectedItem == null)
                return null;
            return _scroll.CurrentSelectedItem as StageItem;
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