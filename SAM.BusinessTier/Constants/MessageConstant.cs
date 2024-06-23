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
        public const string UserExisted = "User đã tồn tại trong hệ thống";
        public const string CreateFailedMessage = "Tạo mới user thất bại";
        public const string UserNotFoundMessage = "User không tồn tại trong hệ thống";
        public const string EmptyUserIdMessage = "UserId ko hợp lệ";
        public const string UpdateStatusSuccessMessage = "Cập nhật thông tin thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật thông tin thất bại";
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