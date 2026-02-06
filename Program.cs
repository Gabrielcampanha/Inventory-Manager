using InventoryManager.Services;

namespace InventoryManager

{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new EstoqueService();
            service.Executar();
        }
    }
}
