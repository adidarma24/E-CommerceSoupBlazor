namespace MyApp.BlazorUI.Models // Sesuaikan namespace
{   
    public record FoodItem(int Id, string Image, string Category, string Title, string Price);
    
    public record FoodType(string Image, string Name);
    
    public record StatItem(string Number, string Description);
}