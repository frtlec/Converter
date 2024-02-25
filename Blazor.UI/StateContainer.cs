using Blazor.UI.Models;

namespace Blazor.UI
{
    public class StateContainer
    {
        private int? globalCounter = 0;
        public int? Property
        {
            get => globalCounter;
            set
            {
                globalCounter = value;
                NotifyStateChanged();
            }
        }

        private List<FollowedProductResponse.SubItem> followedProducts = new List<FollowedProductResponse.SubItem>();

        public List<FollowedProductResponse.SubItem> FollowedProducts
        {
            get => followedProducts;
            set
            {
                followedProducts = value;
                NotifyStateChanged_FollowedProducts();
            }
        }

        public event Action? OnChange;
        public event Action? OnChange_Products;

        private void NotifyStateChanged() => OnChange?.Invoke();
        private void NotifyStateChanged_FollowedProducts() => OnChange_Products?.Invoke();


    }
}
