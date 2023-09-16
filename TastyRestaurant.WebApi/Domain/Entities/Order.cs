using TastyRestaurant.WebApi.Domain.Abstract;
using TastyRestaurant.WebApi.Domain.Enums;
using TastyRestaurant.WebApi.Domain.Exceptions;

namespace TastyRestaurant.WebApi.Domain.Entities;

// Order entity is an aggregate root - it is an entry point for modifying the order bounded context
// also responsible for keeping consistency rules
public sealed class Order : AggregateRoot<Guid>
{
    // empty constructor for EF purposes
    private Order() { }

    // original order items list - encapsulated
    private readonly List<OrderItem> _orderItems = new();

    public OrderStatusEnum Status { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime UpdateDate { get; private set; }

    // getter returning copy of order items so that we encapsulate the original collection
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.ToList();

    // order price is a sum of all order items, can be calculated on the fly
    public decimal Price
    {
        get { return _orderItems.Sum(x => x.TotalPrice); }
        private set {} // added private setter to allow EF to store this calculated property in the DB
    }

    // private constructor - order creation only via CreateOrder factory method
    private Order(Guid id, Guid userId, IEnumerable<OrderItem> initialOrderItems) : base(id)
    {
        // initialize order items collection
        SetOrderItemsCollection(initialOrderItems);

        // assign user to order
        UserId = userId;

        // set initial order status
        Status = OrderStatusEnum.Created;

        // set order status initial dates
        var currentDate = DateTime.Now;
        CreationDate = currentDate;
        UpdateDate = currentDate;
    }

    // factory method for creating initial order
    public static Order Create(Guid id, Guid userId, IEnumerable<OrderItem> initialOrderItems)
    {
        if (id == Guid.Empty)
            throw new GuidRequiredException("Invalid order id guid. Order id is required.");

        if (userId == Guid.Empty)
            throw new GuidRequiredException("Invalid user id guid. User id is required.");

        return new Order(id, userId, initialOrderItems);
    }

    // method for updating the order items list
    public void UpdateOrderItems(IEnumerable<OrderItem> orderItems)
    {
        // if status <> 'created' -> order has already been processed and it's not possible to make any changes
        if (Status != OrderStatusEnum.Created)
            throw new OrderAlreadyProcessedException("Making order items changes is not possible. Order has already been processed.");

        SetOrderItemsCollection(orderItems);
    }

    // method to mark order as ready to pickup/deliver
    public void Ready()
    {
        // setting 'ready' status available only from 'created' status
        if (Status == OrderStatusEnum.Completed)
            throw new InvalidStatusChangeException("Cannot mark order as 'Ready'. Order already completed.");
        
        if (Status == OrderStatusEnum.Ready)
            throw new InvalidStatusChangeException("Cannot change order to 'Ready'. Order status already set to 'Ready'.");

        if (Status == OrderStatusEnum.Cancelled)
            throw new InvalidStatusChangeException("Cannot change order to 'Ready'. Order already cancelled.");

        // not possible to mark empty order 

        // change status
        Status = OrderStatusEnum.Ready;
        UpdateDate = DateTime.Now;
    }

    // method to mark delivered order as 'completed'
    public void Complete()
    {
        // setting 'complete' status available only from 'ready' status
        if (Status == OrderStatusEnum.Completed)
            throw new InvalidStatusChangeException("Cannot mark order as 'Completed'. Order already completed.");

        if (Status == OrderStatusEnum.Created)
            throw new InvalidStatusChangeException("Cannot change order to 'Completed'. Order is just created, is not ready yet.");

        if (Status == OrderStatusEnum.Cancelled)
            throw new InvalidStatusChangeException("Cannot change order to 'Completed'. Order already cancelled.");

        // change status
        Status = OrderStatusEnum.Completed;
        UpdateDate = DateTime.Now;
    }

    // method to cancel order
    public void Cancel()
    {
        // cannot cancel order if it is already completed or cancelled
        if (Status == OrderStatusEnum.Completed)
            throw new InvalidStatusChangeException("Cannot mark order as 'Cancelled'. Order already completed.");

        if (Status == OrderStatusEnum.Cancelled)
            throw new InvalidStatusChangeException("Cannot mark order as 'Cancelled'. Order already cancelled.");

        // change status
        Status = OrderStatusEnum.Cancelled;
        UpdateDate = DateTime.Now;
    }

    // method to combine duplicated order items - multiple items for the same menu item
    private IEnumerable<OrderItem> GroupOrderItems(IEnumerable<OrderItem> orderItems)
    {
        var result = new List<OrderItem>();

        // group by common menu item
        var grouppedItemsList = orderItems.GroupBy(x => x.MenuItem);

        // for each group - create new combined order item
        foreach (var grouppedItems in grouppedItemsList)
        {
            var commonMenuItem = grouppedItems.Key;
            var totalQuantity = grouppedItems.Sum(x => x.Quantity);

            var newOrderItem = OrderItem.Create(commonMenuItem, totalQuantity);
            result.Add(newOrderItem);
        }

        return result;
    }

    private void SetOrderItemsCollection(IEnumerable<OrderItem> orderItems)
    {
        // verify if order has any items
        if (orderItems == null || !orderItems.Any())
            throw new CannotCreateEmptyOrderException("No items ordered. Cannot create empty order.");

        // group order items - if we have multiple times the same order item - combine them into 1 position
        var grouppedOrderItems = GroupOrderItems(orderItems);

        // add combined order items
        _orderItems.Clear();
        _orderItems.AddRange(grouppedOrderItems);

        // update updateDate
        UpdateDate = DateTime.Now;
    }
}