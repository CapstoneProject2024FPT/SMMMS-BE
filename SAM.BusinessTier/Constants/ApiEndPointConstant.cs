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
    public static class User
    {
        public const string UsersEndPoint = ApiEndpoint + "/users";
        public const string UserEndPoint = UsersEndPoint + "/{id}";
        public const string UserEndPointMember = UsersEndPoint + "/{id}";
        public const string UserToRankEndPoint = UsersEndPoint + "/{id}/addRank";

    }

    public static class Category
    {
        public const string CategoriesEndPoint = ApiEndpoint + "/categories";
        public const string CategoryEndPoint = CategoriesEndPoint + "/{id}";
    }
    public static class Origin
    {
        public const string OriginsEndPoint = ApiEndpoint + "/origin";
        public const string OriginEndPoint = OriginsEndPoint + "/{id}";
    }
    public static class Brand
    {
        public const string BrandsEndPoint = ApiEndpoint + "/brand";
        public const string BrandEndPoint = BrandsEndPoint + "/{id}";
    }
    public static class Inventory
    {
        public const string InventoriesEndPoint = ApiEndpoint + "/inventories";
        public const string InventoryEndPoint = InventoriesEndPoint + "/{id}";
    }
    public static class Product
    {
        public const string MachineriesEndPoint = ApiEndpoint + "/machinery";
        public const string MachineriesEndPointNoPaginate = ApiEndpoint + "/machinery/noPaginate";
        public const string MachineryEndPoint = MachineriesEndPoint + "/{id}";
        public const string MachineriesEndPointDetail = ApiEndpoint + "/machinery/Detail/{id}";
        public const string MachineriesUpdateStatusEndPoint = MachineriesEndPoint + "/updateStatus/{id}";

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
        public const string WarrantyDetailsEndPointNoPaginate = ApiEndpoint + "/warrantyDetail/noPaginate";
        public const string WarrantydetailEndPoint = WarrantyDetailsEndPoint + "/{id}";
        public const string WarrantyDetailsEndPointDetail = ApiEndpoint + "/warrantyDetail/Detail/{id}";
        public const string WarrantyDetailsUpdateStatusEndPoint = WarrantyDetailsEndPoint + "/updateStatus/{id}";

    }
    public static class News
    {
        public const string NewsSEndPoint = ApiEndpoint + "/news";
        public const string NewsSEndPointNoPaginate = ApiEndpoint + "/news/noPaginate";
        public const string NewsEndPoint = NewsSEndPoint + "/{id}";
        public const string NewsSEndPointDetail = ApiEndpoint + "/news/Detail/{id}";
        public const string NewsSUpdateStatusEndPoint = NewsSEndPoint + "/updateStatus/{id}";

    }
    public static class Rank
    {
        public const string RanksEndPoint = ApiEndpoint + "/rank";
        public const string RanksEndPointRankToAccount = RanksEndPoint + "/{id}/account";
        public const string RankEndPoint = RanksEndPoint + "/{id}";
        public const string RanksEndPointDetail = ApiEndpoint + "/rank/Detail/{id}";
        public const string RanksUpdateStatusEndPoint = RanksEndPoint + "/updateStatus/{id}";

    }
    public static class Discount
    {
        public const string DiscountsEndPoint = ApiEndpoint + "/discount";
        public const string DiscountsEndPointNoPaginate = ApiEndpoint + "/discount/noPaginate";
        public const string DiscountEndPoint = DiscountsEndPoint + "/{id}";
        public const string DiscountsEndPointDetail = ApiEndpoint + "/discount/Detail/{id}";
        public const string DiscountsUpdateStatusEndPoint = DiscountsEndPoint + "/updateStatus/{id}";

    }


}