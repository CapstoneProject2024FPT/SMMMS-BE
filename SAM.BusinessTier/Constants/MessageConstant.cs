using System.Data;
using System.Net.NetworkInformation;

namespace SAM.BusinessTier.Constants;

public static class MessageConstant
{
    public static class LoginMessage
    {
        public const string InvalidUsernameOrPassword = "Tên đăng nhập hoặc mật khẩu không chính xác";
        public const string DeactivatedAccount = "Tài khoản đang bị vô hiệu hoá";
    }

    public static class Account
    {
        public const string AccountExisted = "Tài khoản đã tồn tại";
        public const string CreateAccountFailed = "Tạo tài khoản thất bại";
        public const string UpdateAccountSuccessMessage = "Cập nhật tài khoản thành công";
        public const string UpdateAccountFailedMessage = "Cập nhật tài khoản thất bại";
        public const string NotFoundFailedMessage = "Tài khoản ko có trong hệ thống";
    }

    public static class Category
    {
        public const string CreateCategoryFailedMessage = "Tạo mới category thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent Category ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Category ko có trong hệ thống";
        public const string UpdateCategorySuccessMessage = "Category được cập nhật thành công";
        public const string UpdateCategoryFailedMessage = "Category cập nhật thất bại";
        public const string CategoryExistedMessage = "Category đã tồn tại";
        public const string CategoryEmptyMessage = "Category không hợp lệ";
    }
    public static class Origin
    {
        public const string CreateOriginFailedMessage = "Tạo mới Origin thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent Origin ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Origin ko có trong hệ thống";
        public const string UpdateOriginSuccessMessage = "Origin được cập nhật thành công";
        public const string UpdateOriginFailedMessage = "Origin cập nhật thất bại";
        public const string OriginExistedMessage = "Origin đã tồn tại";
        public const string OriginEmptyMessage = "Origin không hợp lệ";
    }
    public static class Brand
    {
        public const string CreateBrandFailedMessage = "Tạo mới Brand thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent Brand ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Brand ko có trong hệ thống";
        public const string UpdateBrandSuccessMessage = "Brand được cập nhật thành công";
        public const string UpdateBrandFailedMessage = "Brand cập nhật thất bại";
        public const string BrandExistedMessage = "Brand đã tồn tại";
        public const string BrandEmptyMessage = "Brand không hợp lệ";
    }
    public static class Address
    {
        public const string CreateAddressFailedMessage = "Tạo mới Address thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent Address ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Address ko có trong hệ thống";
        public const string UpdateAddressSuccessMessage = "Address được cập nhật thành công";
        public const string UpdateAddressFailedMessage = "Address cập nhật thất bại";
        public const string AddressExisteMessage = "Address đã tồn tại";
        public const string AddressdEmptyMessage = "Address không hợp lệ";
    }
    public static class City
    {
        public const string CreateCityFailedMessage = "Tạo mới City thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent City ko có trong hệ thống";
        public const string NotFoundFailedMessage = "City ko có trong hệ thống";
        public const string UpdateCitySuccessMessage = "City được cập nhật thành công";
        public const string UpdateCityFailedMessage = "City cập nhật thất bại";
        public const string CityExistedMessage = "City đã tồn tại";
        public const string CityEmptyMessage = "City không hợp lệ";
    }
    public static class District
    {
        public const string CreateDistrictFailedMessage = "Tạo mới District thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent District ko có trong hệ thống";
        public const string NotFoundFailedMessage = "District ko có trong hệ thống";
        public const string UpdateDistrictSuccessMessage = "District được cập nhật thành công";
        public const string UpdateDistrictFailedMessage = "District cập nhật thất bại";
        public const string DistrictExistedMessage = "District đã tồn tại";
        public const string DistrictEmptyMessage = "District không hợp lệ";
    }
    public static class Ward
    {
        public const string CreateWardFailedMessage = "Tạo mới Ward thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent Ward ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Ward ko có trong hệ thống";
        public const string UpdateWardSuccessMessage = "Ward được cập nhật thành công";
        public const string UpdateWardFailedMessage = "Ward cập nhật thất bại";
        public const string WardExistedMessage = "Ward đã tồn tại";
        public const string WardEmptyMessage = "Ward không hợp lệ";
    }
    public static class Inventory
    {
        public const string CreateInventoryFailedMessage = "Tạo mới Inventory thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent Inventory ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Inventory ko có trong hệ thống";
        public const string UpdateInventorySuccessMessage = "Inventory được cập nhật thành công";
        public const string UpdateInventoryFailedMessage = "Inventory cập nhật thất bại";
        public const string InventoryExistedMessage = "Inventory đã tồn tại";
        public const string InventoryEmptyMessage = "Inventory không hợp lệ";
        public const string NotAvaliable = "Không còn Machinery nào sẵn sàng để bán";
    }
    public static class Machinery
    {
        public const string MachineryNameExisted = "Machinery đã tồn tại";
        public const string CreateNewMachineryFailedMessage = "Tạo mới machinery thất bại";
        public const string UpdateMachinerytFailedMessage = "Cập nhật thông tin machinery thất bại";
        public const string UpdateMachinerySuccessMessage = "Cập nhật thông tin machinery thành công";
        public const string EmptyMachineryIdMessage = "Machinery Id không hợp lệ";
        public const string MachineryNotFoundMessage = "Machinery không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";

    }
    public static class Specification
    {
        public const string SpecificationNameExisted = "Specification đã tồn tại";
        public const string CreateNewSpecificationFailedMessage = "Tạo mới Specification thất bại";
        public const string UpdateSpecificationFailedMessage = "Cập nhật thông tin Specification thất bại";
        public const string UpdateSpecificationSuccessMessage = "Cập nhật thông tin Specification thành công";
        public const string EmptySpecificationIdMessage = "Specification Id không hợp lệ";
        public const string MachineryNotFoundMessage = "Specification không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
    
    public static class ProductRetail
    {
        public const string CreateFailedMessage = "Tạo mới product thất bại";
        public const string NotFoundMessage = "Product không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
    }

    public static class Order
    {
        public const string OrderNotFoundMessage = "Order không tồn tại trong hệ thống";
        public const string CreateOrderFailedMessage = "Tạo mới order thất bại";
        public const string UpdateSuccessMessage = "Order được cập nhật thành công";
        public const string UpdateFailedMessage = "Order cập nhật thất bại";
    }

    public static class OrderDetail
    {
        public const string NotFoundMessage = "Order không tồn tại trong hệ thống";
    }

    public static class User
    {
        public const string UserExisted = "Account đã tồn tại trong hệ thống";
        public const string CreateFailedMessage = "Tạo mới Account thất bại";
        public const string UserNotFoundMessage = "Account không tồn tại trong hệ thống";
        public const string EmptyUserIdMessage = "AccountId ko hợp lệ";
        public const string UpdateStatusSuccessMessage = "Cập nhật thông tin thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật thông tin thất bại";
        public const string CheckPasswordFailed = "Mật khẩu cũ không đúng";
        public const string ChangePasswordToFailed = "Đổi mật khẩu thất bại";
        public const string ChangePasswordToSuccess = "Đổi mật khẩu thành công";
    }

    public static class ProductReview
    {
        public const string CreateFailedMessage = "Tạo mới review thất bại";

    }
    public static class Warranty
    {
        public const string WarrantyNameExisted = "Warranty đã tồn tại";
        public const string CreateNewWarrantyFailedMessage = "Tạo mới Warranty thất bại";
        public const string UpdateWarrantyFailedMessage = "Cập nhật thông tin Warranty thất bại";
        public const string UpdateWarrantySuccessMessage = "Cập nhật thông tin Warranty thành công";
        public const string EmptyWarrantyIdMessage = "Warranty Id không hợp lệ";
        public const string WarrantyNotFoundMessage = "Warranty không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
    public static class WarrantyDetail
    {
        public const string WarrantyDetailNameExisted = "Warranty Detail đã tồn tại";
        public const string CreateNewWarrantyDetailFailedMessage = "Tạo mới Warranty Detail thất bại";
        public const string UpdateWarrantyDetailFailedMessage = "Cập nhật thông tin Warranty Detail thất bại";
        public const string UpdateWarrantyDetailSuccessMessage = "Cập nhật thông tin Warranty Detail thành công";
        public const string EmptyWarrantyDetailIdMessage = "Warranty Detail Id không hợp lệ";
        public const string WarrantyDetailNotFoundMessage = "Warranty Detail không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }

    public static class Rank
    {
        public const string RankNameExisted = "Rank đã tồn tại";
        public const string CreateNewRankFailedMessage = "Tạo mới Rank thất bại";
        public const string UpdateRankFailedMessage = "Cập nhật thông tin Rank thất bại";
        public const string UpdateRankSuccessMessage = "Cập nhật thông tin Rank thành công";
        public const string EmptyRankIdMessage = "Rank Id không hợp lệ";
        public const string RankNotFoundMessage = "Rank không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
    public static class TaskManager
    {
        public const string TaskNameExisted = "Task đã tồn tại";
        public const string CreateNewTaskFailedMessage = "Tạo mới Task thất bại";
        public const string UpdateTaskFailedMessage = "Cập nhật thông tin Task thất bại";
        public const string UpdateTaskSuccessMessage = "Cập nhật thông tin Task thành công";
        public const string EmptyTaskIdMessage = "Task Id không hợp lệ";
        public const string TaskNotFoundMessage = "Task không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
    public static class MachineryComponents
    {
        public const string MachineryComponentsNameExisted = "MachineryComponents đã tồn tại";
        public const string CreateNewMachineryComponentsFailedMessage = "Tạo mới MachineryComponents thất bại";
        public const string UpdateMachineryComponentsFailedMessage = "Cập nhật thông tin MachineryComponents thất bại";
        public const string UpdateMachineryComponentsSuccessMessage = "Cập nhật thông tin MachineryComponents thành công";
        public const string EmptyMachineryComponentsIdMessage = "MachineryComponents Id không hợp lệ";
        public const string MachineryComponentsNotFoundMessage = "MachineryComponents không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
    public static class News
    {
        public const string NewsNameExisted = "News đã tồn tại";
        public const string CreateNewNewsFailedMessage = "Tạo mới News thất bại";
        public const string UpdateNewsFailedMessage = "Cập nhật thông tin News thất bại";
        public const string UpdateNewsSuccessMessage = "Cập nhật thông tin News thành công";
        public const string EmptyNewsIdMessage = "News Id không hợp lệ";
        public const string NewsNotFoundMessage = "News không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
    public static class Discount
    {
        public const string DiscountNameExisted = "Discount đã tồn tại";
        public const string CreateNewDiscountFailedMessage = "Tạo mới Discount thất bại";
        public const string UpdateDiscountFailedMessage = "Cập nhật thông tin Discount thất bại";
        public const string UpdateDiscountSuccessMessage = "Cập nhật thông tin Discount thành công";
        public const string EmptyDiscountIdMessage = "Discount Id không hợp lệ";
        public const string DiscountNotFoundMessage = "Discount không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
}