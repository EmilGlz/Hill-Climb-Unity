using Scripts.Items;
using Scripts.Managers;
using Scripts.UI;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
namespace Scripts.Views
{
    public class CarShopView : View
    {
        public static string PrefabPath = "Prefabs/Views/ShopScrollView";
        private CarItemList _carItemList;
        private ScrollScaleController _scrollView;
        public override async Task EnterView()
        {
            await base.EnterView();
            _scrollView = gameObject.GetComponentInChildren<ScrollScaleController>();
            if (_scrollView == null)
                _scrollView = ResourceHelper.InstantiatePrefab(PrefabPath, transform).GetComponent<ScrollScaleController>();

            if (_carItemList != null)
            {
                PauseResumeView(true);
                return;
            }
            var content = Utils.FindGameObject("Content", _scrollView.gameObject);
            content.transform.DestroyAllChildren();
            _carItemList = await CarItemList.CreateAsync(ItemController.instance.userData.ownedCars, content.transform, _scrollView);
            Utils.RunAsync(() =>
            {
                _scrollView.InitItems(_carItemList.Items);
                StartCoroutine(_scrollView.ScrollTo(_carItemList.Items.FirstOrDefault(i => i.Data is CarData carData && carData == Settings.User.currentSelectedCar)));
            });
        }

        public CarData GetCurrentSelectedCarData()
        {
            return _scrollView.CurrentSelectedItem?.Data as CarData;
        }

        public CarItem GetCurrentSelectedItem()
        {
            if (_scrollView.CurrentSelectedItem == null)
                return null;
            return _scrollView.CurrentSelectedItem as CarItem;
        }


        public override void ExitView()
        {
            if (_carItemList != null)
            {
                _carItemList.Dispose();
                _carItemList = null;
            }
            if (_scrollView != null)
            {
                Destroy(_scrollView.gameObject);
                _scrollView = null;
            }
        }
    }
}
