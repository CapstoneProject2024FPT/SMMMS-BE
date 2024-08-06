using System.Net.NetworkInformation;

namespace SAM.BusinessTier.Constants;

public static class ApiEndPointConstant
{

    public const string RootEndPoint = "/api";
    public const string ApiVersion = "/v1";
    public const string ApiEndpoint = RootEndPoint + ApiVersion;

    public static class Authentication
    {
        public const string AuthenticationEndpoint = ApiEndpoint + "/auth";
        public const string Login = AuthenticationEndpoint + "/login";
    }
    public static class Notification
    {
        public const string NotificationsEndPoint = "/notifications";
        public const string NotificationEndPoint = NotificationsEndPoint + "/{id}";
        public const string NotificationReadEndPoint = NotificationEndPoint + "/read";
    }
    public static class User
    {
        public const string UsersEndPoint = ApiEndpoint + "/users";
        public const string StaffEndPoint = ApiEndpoint + "/staff";
        public const string UserEndPoint = UsersEndPoint + "/{id}";
        public const string UserEndPointMember = UsersEndPoint + "/{id}";
        public const string UserToRankEndPoint = UsersEndPoint + "/{id}/addRank";
        public const string UserEndPointChangePassword = UsersEndPoint + "/{id}/password";
        public const string StaffEndPoints = ApiEndpoint + "/register/staff";
        public const string AccountEndPoint = ApiEndpoint + "/register/customer";
        public const string GetTaskOfStaff = StaffEndPoint + "/task";

    }

    public static class Category
    {
        public const string CategoriesEndPoint = ApiEndpoint + "/categories";
        public const string CategoryEndPoint = CategoriesEndPoint + "/{id}";
        public const string CategoryAddDiscountEndPoint = CategoryEndPoint + "/discount";
    }
    public static class NewsCategory
    {
        public const string NewsCategoriesEndPoint = ApiEndpoint + "/newsCategory";
        public const string NewsCategoryEndPoint = NewsCategoriesEndPoint + "/{id}";
    }
    public static class Origin
    {
        public const string OriginsEndPoint = ApiEndpoint + "/origin";
        public const string OriginEndPoint = OriginsEndPoint + "/{id}";
    }
    public static class Address
    {
        public const string AddressSEndPoint = ApiEndpoint + "/address";
        public const string AddressEndPoint = AddressSEndPoint + "/{id}";
    }
    public static class Admin
    {
        public const string AdminsEndPoint = ApiEndpoint + "/admin";
        public const string AdminEndPoint = AdminsEndPoint + "/{id}";
        public const string AdminDashBoardEndPoint = AdminsEndPoint + "/dashBoard";
        public const string AdminCountOrdersDashBoardEndPoint = AdminDashBoardEndPoint + "/countOrders";

    }
    public static class City
    {
        public const string CitiesEndPoint = ApiEndpoint + "/city";
        public const string CityEndPoint = CitiesEndPoint + "/{id}";
    }
    public static class District
    {
        public const string DistrictsEndPoint = ApiEndpoint + "/district";
        public const string DistrictEndPoint = DistrictsEndPoint + "/{id}";
    }
    public static class Ward
    {
        public const string WardsEndPoint = ApiEndpoint + "/ward";
        public const string WardEndPoint = WardsEndPoint + "/{id}";
    }
    public static class Brand
    {
        public const string BrandsEndPoint = ApiEndpoint + "/brand";
        public const string BrandEndPoint = BrandsEndPoint + "/{id}";
    }
    public static class Favorite
    {
        public const string FavoritesEndPoint = ApiEndpoint + "/favorite";
        public const string FavoriteEndPoint = FavoritesEndPoint + "/{id}";
    }
    public static class Inventory
    {
        public const string InventoriesEndPoint = ApiEndpoint + "/inventories";
        public const string InventoryEndPoint = InventoriesEndPoint + "/{id}";
        public const string MachineriesInventoryDetailEndPoint = InventoryEndPoint + "/machinery";
    }
    public static class Product
    {
        public const string MachineriesEndPoint = ApiEndpoint + "/machinery";
        public const string MachineriesEndPointNoPaginate = MachineriesEndPoint + "/noPaginate";
        public const string MachineryEndPoint = MachineriesEndPoint + "/{id}";
        public const string MachineriesEndPointDetail = MachineriesEndPoint + "/{id}/detail";
        public const string MachineriesUpdateStatusEndPoint = MachineriesEndPoint + "/updateStatus/{id}";
        public const string MachineryAddComponentEndPoint = MachineriesEndPoint + "/{id}/addComponent";


    }
    public static class MachineryComponent
    {
        public const string MachineryComponentsEndPoint = ApiEndpoint + "/machineryComponent";
        public const string MachineryComponentsEndPointNoPaginate = ApiEndpoint + "/machineryComponent/noPaginate";
        public const string MachineryComponentEndPoint = MachineryComponentsEndPoint + "/{id}";
        public const string MachineryComponentsEndPointDetail = ApiEndpoint + "/machineryComponent/Detail/{id}";
        public const string MachineryComponentsUpdateStatusEndPoint = MachineryComponentsEndPoint + "/updateStatus/{id}";

    }


    public static class Order
    {
        public const string OrdersEndPoint = ApiEndpoint + "/orders";
        public const string OrderEndPoint = OrdersEndPoint + "/{id}";
        public const string OrderHistoriesEndPoint = OrderEndPoint + "/orderHistory";
    }

    public static class Warranty
    {
        public const string WarrantiesEndPoint = ApiEndpoint + "/warranty";
        public const string WarrantiesEndPointNoPaginate = ApiEndpoint + "/warranty/noPaginate";
        public const string WarrantyEndPoint = WarrantiesEndPoint + "/{id}";
        public const string WarrantiesEndPointDetail = ApiEndpoint + "/warranty/Detail/{id}";
        public const string WarrantiesUpdateStatusEndPoint = WarrantiesEndPoint + "/updateStatus/{id}";

    }
    public static class WarrantyDetail
    {
        public const string WarrantyDetailsEndPoint = ApiEndpoint + "/warrantyDetail";
        public const string WarrantyDetailsEndPointNoPaginate = WarrantyDetailsEndPoint + "/noPaginate";
        public const string WarrantydetailEndPoint = WarrantyDetailsEndPoint + "/{id}";

    }
    public static class News
    {
        public const string NewsSEndPoint = ApiEndpoint + "/news";
        public const string NewsSEndPointNoPaginate = NewsSEndPoint + "/noPaginate";
        public const string NewsEndPoint = NewsSEndPoint + "/{id}";
        public const string NewsSEndPointDetail = NewsSEndPoint + "/detail/{id}";
        public const string NewsSUpdateStatusEndPoint = NewsSEndPoint + "/updateStatus/{id}";

    }
    public static class Rank
    {
        public const string RanksEndPoint = ApiEndpoint + "/rank";
        public const string RanksEndPointRankToAccount = RanksEndPoint + "/{id}/account";
        public const string RankEndPoint = RanksEndPoint + "/{id}";
        public const string RanksUpdateStatusEndPoint = RanksEndPoint + "/updateStatus/{id}";

    }
    public static class TaskManager
    {
        public const string TasksEndPoint = ApiEndpoint + "/task";
        public const string TaskEndPoint = TasksEndPoint + "/{id}";

    }
    public static class Discount
    {
        public const string DiscountsEndPoint = ApiEndpoint + "/discount";
        public const string DiscountEndPoint = DiscountsEndPoint + "/{id}";
        public const string DiscountsEndPointDetail = DiscountsEndPoint + "/detail/{id}";

    }
    public static class Payment
    {
        public const string PaymentEndpoint = ApiEndpoint + "/payments";
        public const string VnPayEndpoint = PaymentEndpoint + "/vnpay";
        public const string PaymentsEndpoint = PaymentEndpoint + "/{id}";

    }
    public static class Transaction
    {
        public const string TransactionsEndPoint = ApiEndpoint + "/transaction";
        public const string TransactionEndPoint = TransactionsEndPoint + "/{id}";
    }

}