using Scripts.Items;
using Scripts.Managers;
using Scripts.UI;
using UnityEngine.UI;
namespace Scripts.Views
{
    public class CarShopView : View
    {
        private CarItemList _carItemList;
        public override void EnterView()
        {
            base.EnterView();
            var back = Utils.FindGameObject("BackButton", gameObject).GetComponent<Button>();
            back.onClick.RemoveAllListeners();
            back.onClick.AddListener(() =>
            {
                UIController.instance.EnterView<GameView>();
            });
            var content = Utils.FindGameObject("Content", gameObject);
            _carItemList = new CarItemList(ItemController.instance.CarDatas, content.transform);
            ScrollScaleController.instance.InitItems();
        }

        public override void ExitView()
        {
            _carItemList.Dispose();
            _carItemList = null;
        }

    }
}
