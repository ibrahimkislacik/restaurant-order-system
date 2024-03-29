namespace CFusionRestaurant.ViewModel.ExceptionManagement;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
