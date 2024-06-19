namespace UserService.Main.Contracts;

public record AddNewUserOrUpdateRequest(
    Guid UserId,
    List<BucketItem> ProductIdsAndQuantity,
    string Comment,
    string Address,
    string PhoneNumber,
    String StoreId);