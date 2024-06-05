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

    public static class Mechinery
    {
        public const string MechineryNameExisted = "Mechinery đã tồn tại";
        public const string CreateNewMechineryFailedMessage = "Tạo mới mechinery thất bại";
        public const string UpdateMechinerytFailedMessage = "Cập nhật thông tin mechinery thất bại";
        public const string UpdateMechinerySuccessMessage = "Cập nhật thông tin mechinery thành công";
        public const string EmptyMechineryIdMessage = "Mechinery Id không hợp lệ";
        public const string MechineryNotFoundMessage = "Mechinery không tồn tại trong hệ thống";
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
}