﻿using System.Net.NetworkInformation;

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
    }

    public static class Category
    {
        public const string CategoriesEndPoint = ApiEndpoint + "/categories";
        public const string CategoryEndPoint = CategoriesEndPoint + "/{id}";
    }
    public static class Product
    {
        public const string MachinerysEndPoint = ApiEndpoint + "/Machinery";
        public const string MachinerysEndPointNoPaginate = ApiEndpoint + "/Machinery/noPaginate";
        public const string MachineryEndPoint = MachinerysEndPoint + "/{id}";
        public const string MachinerysEndPointDetail = ApiEndpoint + "/Machinery/Detail/{id}";
        public const string MachineriesUpdateStatusEndPoint = MachinerysEndPoint + "/updateStatus/{id}";

    }
   
    
    public static class Order
    {
        public const string OrdersEndPoint = ApiEndpoint + "/orders";
        public const string OrderEndPoint = OrdersEndPoint + "/{id}";
        public const string OrderHistoriesEndPoint = OrderEndPoint + "/orderHistory";
    }

}