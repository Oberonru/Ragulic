namespace Core.Models
{
    public class PlayerStatistic
    {
        public string PlayerId;
        public int InventoryInteractCount = 0;
        public int Death = 0;
        public int IncomingGold = 0;

        public void AddInteractCunt(int count) => InventoryInteractCount += count;
        public void CalculateDeath(int value) => Death += value;
    }
}