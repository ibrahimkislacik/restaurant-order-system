using AutoMapper;
using CFusionRestaurant.BusinessLayer.Abstract.OrderManagement;
using CFusionRestaurant.BusinessLayer.Abstract.UserManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.OrderManagement;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.OrderManagement;
using CFusionRestaurant.ViewModel.OrderManagement.Request;
using CFusionRestaurant.ViewModel.OrderManagement.Response;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CFusionRestaurant.BusinessLayer.Concrete.OrderManagement;

/// <summary>
/// Service class responsible for handling operations related to orders.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public OrderService(IRepository<Order> orderRepository,
        IRepository<Product> productRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<OrderViewModel>> ListAsync()
    {
        var orders = await _orderRepository.ListAsync().ConfigureAwait(false);
        return _mapper.Map<List<OrderViewModel>>(orders);
    }

    public async Task<List<OrderViewModel>> ListForUserAsync()
    {
        var filter = Builders<Order>.Filter.Eq(p => p.OrderUserInfo.UserId, ObjectId.Parse(_currentUserService.UserId));
        var orders = await _orderRepository.ListAsync(filter).ConfigureAwait(false);
        return _mapper.Map<List<OrderViewModel>>(orders);
    }

    public async Task<OrderViewModel> GetAsync(string id)
    {
        var order = await _orderRepository.GetAsync(id).ConfigureAwait(false);
        return _mapper.Map<OrderViewModel>(order);
    }

    public async Task<OrderInsertResponseViewModel> InsertAsync(OrderInsertRequestViewModel orderInsertRequestViewModel)
    {
        ValidateOrderProducts(orderInsertRequestViewModel);
        var products = await GetActiveProductsForTodayAsync(orderInsertRequestViewModel).ConfigureAwait(false);
        var order = CreateOrder(orderInsertRequestViewModel, products);
        await _orderRepository.InsertAsync(order).ConfigureAwait(false);

        var result = new OrderInsertResponseViewModel()
        {
            OrderNo = order.OrderNo
        };

        return result;
    }

    private void ValidateOrderProducts(OrderInsertRequestViewModel orderInsertRequestViewModel)
    {
        if (!orderInsertRequestViewModel.OrderProducts.Any())
        {
            throw new BusinessException("At least one product must be selected");
        }
    }

    private async Task<List<Product>> GetActiveProductsForTodayAsync(OrderInsertRequestViewModel orderInsertRequestViewModel)
    {
        //Add filter for selected product ids
        var productFilters = new List<FilterDefinition<Product>>();
        foreach (var orderProduct in orderInsertRequestViewModel.OrderProducts)
        {
            productFilters.Add(Builders<Product>.Filter.Eq(p => p.Id, ObjectId.Parse(orderProduct.ProductId)));
        }

        var generalFilter = Builders<Product>.Filter.Or(productFilters);

        //Add filter for, Is the product selected on the same day?
        switch (DateTime.Today.DayOfWeek)
        {
            case DayOfWeek.Monday:
                generalFilter &= Builders<Product>.Filter.Where(p => p.IsActiveOnMonday);
                break;
            case DayOfWeek.Tuesday:
                generalFilter &= Builders<Product>.Filter.Where(p => p.IsActiveOnTuesday);
                break;
            case DayOfWeek.Wednesday:
                generalFilter &= Builders<Product>.Filter.Where(p => p.IsActiveOnWednesday);
                break;
            case DayOfWeek.Thursday:
                generalFilter &= Builders<Product>.Filter.Where(p => p.IsActiveOnThursday);
                break;
            case DayOfWeek.Friday:
                generalFilter &= Builders<Product>.Filter.Where(p => p.IsActiveOnFriday);
                break;
            case DayOfWeek.Saturday:
                generalFilter &= Builders<Product>.Filter.Where(p => p.IsActiveOnSaturday);
                break;
            case DayOfWeek.Sunday:
                generalFilter &= Builders<Product>.Filter.Where(p => p.IsActiveOnSunday);
                break;
        }

        return await _productRepository.ListAsync(generalFilter).ConfigureAwait(false);
    }

    private Order CreateOrder(OrderInsertRequestViewModel orderInsertRequestViewModel, List<Product> products)
    {
        var order = new Order()
        {
            OrderNo = OrderNumberGenerator.GenerateOrderNumber(),
            OrderProducts = new List<OrderProduct>(),
            Total = 0,
            OrderUserInfo = new OrderUserInfo()
            {
                Id = ObjectId.GenerateNewId(),
                EMail = _currentUserService.EMail,
                Name = _currentUserService.Name,
                UserId = ObjectId.Parse(_currentUserService.UserId)
            }
        };

        foreach (var orderProduct in orderInsertRequestViewModel.OrderProducts)
        {
            var product = products.SingleOrDefault(x => x.Id == ObjectId.Parse(orderProduct.ProductId));
            if (product == null)
            {
                //product not found, return business exception
                throw new BusinessException($"Product with Id = {orderProduct.ProductId} not found");
            }

            order.OrderProducts.Add(new OrderProduct
            {
                Id = ObjectId.GenerateNewId(),
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = orderProduct.Quantity,
                Note = orderProduct.Note,
            });
            order.Total += product.Price * orderProduct.Quantity;
        }

        order.CreatedDateTime = DateTime.Now;
        order.Id = ObjectId.GenerateNewId();

        return order;
    }
}
