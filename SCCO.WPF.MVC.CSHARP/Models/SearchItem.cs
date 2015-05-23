namespace SCCO.WPF.MVC.CS.Models
{
    public class SearchItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode
        {
            get; set; }
        public SearchItem(int itemId, string itemName)
        {
            ItemId = itemId;
            ItemName = itemName;
        }

        public SearchItem(string itemCode, string itemName)
        {
            ItemCode = itemCode;
            ItemName = itemName;
        }
    }
}
