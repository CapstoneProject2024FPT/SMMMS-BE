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
    public static class Status
    {
        public const string ExsitingValue = "Cần nhập giá trị cho các loại trạng thái";
    }
    public static class Notification
    {
        public const string UpdateNotificationSuccessMessage = "Update notification successfully.";
        public const string UpdateNotificationFailedMessage = "Update notification failed.";
        public const string MarkNotificationAsReadSuccessMessage = "Mark notification as read successfully.";
        public const string MarkNotificationAsReadFailedMessage = "Mark notification as read failed.";
        public const string RemoveNotificationSuccessMessage = "Remove notification successfully.";
        public const string RemoveNotificationFailedMessage = "Remove notification failed.";
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
        public const string CreateCategoryFailedMessage = "Tạo mới loại thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent loại ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Loại ko có trong hệ thống";
        public const string UpdateCategorySuccessMessage = "Loại được cập nhật thành công";
        public const string UpdateCategoryFailedMessage = "Loại cập nhật thất bại";
        public const string CategoryExistedMessage = "Loại đã tồn tại";
        public const string CategoryEmptyMessage = "Loại không hợp lệ";
    }
    public static class Favorite
    {
        public const string CreateFavoriteFailedMessage = "Thêm sản phẩm yêu thích thất bại";
        public const string NotFoundFailedMessage = "Sản phẩm yêu thích ko có trong hệ thống";
        public const string UpdateFavoriteSuccessMessage = "cập nhật thành công";
        public const string UpdateFavoriteFailedMessage = "cập nhật thất bại";
        public const string FavoriteExistedMessage = "Sản phẩm yêu thích đã tồn tại";
        public const string FavoriteEmptyMessage = "Sản phẩm yêu thích không hợp lệ";
        public const string DeleteSuccessMessage = "Xóa sản phẩm yêu thích thành công";
        public const string DeleteFailedMessage = "Xóa sản phẩm yêu thích thất bại";
    }
    public static class Origin
    {
        public const string CreateOriginFailedMessage = "Tạo mới xuất xứ thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent xuất xứ ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Xuất xứ ko có trong hệ thống";
        public const string UpdateOriginSuccessMessage = "Xuất xứ được cập nhật thành công";
        public const string UpdateOriginFailedMessage = "Xuất xứ cập nhật thất bại";
        public const string OriginExistedMessage = "Xuất xứ đã tồn tại";
        public const string OriginEmptyMessage = "Xuất xứ không hợp lệ";
    }
    public static class Transaction
    {
        public const string CreateTransactionFailedMessage = "Tạo mới giao dịch thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent giao dịch ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Giao dịch ko có trong hệ thống";
        public const string UpdateTransactionSuccessMessage = "Giao dịch được cập nhật thành công";
        public const string UpdateTransactionFailedMessage = "Giao dịch cập nhật thất bại";
        public const string TransactionExistedMessage = "Giao dịch đã tồn tại";
        public const string TransactionEmptyMessage = "Giao dịch không hợp lệ";
    }
    public static class Brand
    {
        public const string CreateBrandFailedMessage = "Tạo mới thương hiệu thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent thương hiệu ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Thương hiệu ko có trong hệ thống";
        public const string UpdateBrandSuccessMessage = "Thương hiệu được cập nhật thành công";
        public const string UpdateBrandFailedMessage = "Thương hiệu cập nhật thất bại";
        public const string BrandExistedMessage = "Thương hiệu đã tồn tại";
        public const string BrandEmptyMessage = "Thương hiệu không hợp lệ";
    }
    public static class Address
    {
        public const string CreateAddressFailedMessage = "Tạo mới địa chỉ thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent địa chỉ ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Địa chỉ ko có trong hệ thống";
        public const string UpdateAddressSuccessMessage = "Địa chỉ được cập nhật thành công";
        public const string UpdateAddressFailedMessage = "Địa chỉ cập nhật thất bại";
        public const string AddressExisteMessage = "Địa chỉ đã tồn tại";
        public const string AddressdEmptyMessage = "Địa chỉ không hợp lệ";
    }
    public static class City
    {
        public const string CreateCityFailedMessage = "Tạo mới thành phố thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent thành phố ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Thành phố ko có trong hệ thống";
        public const string UpdateCitySuccessMessage = "Thành phố được cập nhật thành công";
        public const string UpdateCityFailedMessage = "Thành phố cập nhật thất bại";
        public const string CityExistedMessage = "Thành phố đã tồn tại";
        public const string CityEmptyMessage = "Thành phố không hợp lệ";
    }
    public static class District
    {
        public const string CreateDistrictFailedMessage = "Tạo mới quận thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent quận ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Quận ko có trong hệ thống";
        public const string UpdateDistrictSuccessMessage = "Quận được cập nhật thành công";
        public const string UpdateDistrictFailedMessage = "Quận cập nhật thất bại";
        public const string DistrictExistedMessage = "Quận đã tồn tại";
        public const string DistrictEmptyMessage = "Quận không hợp lệ";
    }
    public static class Ward
    {
        public const string CreateWardFailedMessage = "Tạo mới phường thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent phường ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Phường ko có trong hệ thống";
        public const string UpdateWardSuccessMessage = "Phường được cập nhật thành công";
        public const string UpdateWardFailedMessage = "Phường cập nhật thất bại";
        public const string WardExistedMessage = "Phường đã tồn tại";
        public const string WardEmptyMessage = "Phường không hợp lệ";
    }
    public static class Inventory
    {
        public const string AlreadySoldMessage = "Không thể thêm bộ phận máy sản phẩm đã bán ra bên ngoài";
        public const string CreateInventoryFailedMessage = "Tạo mới mã sản phẩm thất bại";
        public const string Parent_NotFoundFailedMessage = "Parent mã sản phẩm ko có trong hệ thống";
        public const string NotFoundFailedMessage = "Mã sản phẩm ko có trong hệ thống";
        public const string UpdateInventorySuccessMessage = "Mã sản phẩm được cập nhật thành công";
        public const string UpdateInventoryFailedMessage = "Mã sản phẩm cập nhật thất bại";
        public const string InventoryExistedMessage = "Mã sản phẩm đã tồn tại";
        public const string InventoryEmptyMessage = "Mã sản phẩm không hợp lệ";
        public const string NotAvaliable = "Không còn Machinery nào sẵn sàng để bán";
    }
    public static class Machinery
    {
        public const string MachineryNameExisted = "Máy cơ khí đã tồn tại";
        public const string CreateNewMachineryFailedMessage = "Tạo mới Máy cơ khí thất bại";
        public const string UpdateMachinerytFailedMessage = "Cập nhật thông tin Máy cơ khí thất bại";
        public const string UpdateMachinerySuccessMessage = "Cập nhật thông tin Máy cơ khí thành công";
        public const string EmptyMachineryIdMessage = "Máy cơ khí Id không hợp lệ";
        public const string MachineryNotFoundMessage = "Máy cơ khí không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";

    }
    public static class Specification
    {
        public const string SpecificationNameExisted = "Thông số kỹ thuật đã tồn tại";
        public const string CreateNewSpecificationFailedMessage = "Tạo mới Thông số kỹ thuật thất bại";
        public const string UpdateSpecificationFailedMessage = "Cập nhật thông tin Thông số kỹ thuật thất bại";
        public const string UpdateSpecificationSuccessMessage = "Cập nhật thông tin Thông số kỹ thuật thành công";
        public const string EmptySpecificationIdMessage = "Thông số kỹ thuật Id không hợp lệ";
        public const string MachineryNotFoundMessage = "Thông số kỹ thuật không tồn tại trong hệ thống";
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
        public const string OrderNotFoundMessage = "Đơn hàng không tồn tại trong hệ thống";
        public const string CreateOrderFailedMessage = "Tạo mới Đơn hàng thất bại";
        public const string UpdateSuccessMessage = "Đơn hàng được cập nhật thành công";
        public const string UpdateFailedMessage = "Đơn hàng cập nhật thất bại";
    }

    public static class OrderDetail
    {
        public const string NotFoundMessage = "Đơn hàng không tồn tại trong hệ thống";
    }

    public static class User
    {
        public const string UserExisted = "Tài khoản đã tồn tại trong hệ thống";
        public const string CreateFailedMessage = "Tạo mới Tài khoản thất bại";
        public const string UserNotFoundMessage = "Tài khoản không tồn tại trong hệ thống";
        public const string EmptyUserIdMessage = "Số Tài khoản ko hợp lệ";
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
        public const string WarrantyNameExisted = "Bảo trì đã tồn tại";
        public const string WarrantyOrderCompletedExisted = "Không thể tạo yêu cầu bảo hành khi đơn hàng chưa được hoàn thành";
        public const string WarrantyCompletedExisted = "Phiếu yêu cần trước đó của máy này chưa hoàn thành, hãy đợi hoàn thành rồi mới tiếp tục tạo yêu cầu mới";
        public const string WarrantyHaveExisted = "Đã có phiếu bảo trì tương tự trong ngày.";
        public const string CreateNewWarrantyFailedMessage = "Tạo mới Bảo trì thất bại";
        public const string UpdateWarrantyFailedMessage = "Cập nhật thông tin Bảo trì thất bại";
        public const string UpdateWarrantySuccessMessage = "Cập nhật thông tin Bảo trì thành công";
        public const string EmptyWarrantyIdMessage = "Bảo trì Id không hợp lệ";
        public const string WarrantyNotFoundMessage = "Bảo trì không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
    public static class WarrantyDetail
    {
        public const string WarrantyDetailNameExisted = "Chi tiết bao trì đã tồn tại";
        public const string CreateNewWarrantyDetailFailedMessage = "Tạo mới Chi tiết bao trì Detail thất bại";
        public const string UpdateWarrantyDetailFailedMessage = "Cập nhật thông tin Chi tiết bao trì Detail thất bại";
        public const string UpdateWarrantyDetailSuccessMessage = "Cập nhật thông tin Chi tiết bao trì Detail thành công";
        public const string EmptyWarrantyDetailIdMessage = "Chi tiết bao trì Detail Id không hợp lệ";
        public const string WarrantyDetailNotFoundMessage = "Chi tiết bao trì Detail không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }

    public static class Rank
    {
        public const string RankNameExisted = "Cấp độ đã tồn tại";
        public const string CreateNewRankFailedMessage = "Tạo mới Cấp độ thất bại";
        public const string UpdateRankFailedMessage = "Cập nhật thông tin Cấp độ thất bại";
        public const string UpdateRankSuccessMessage = "Cập nhật thông tin Cấp độ thành công";
        public const string EmptyRankIdMessage = "Mã Cấp độ không hợp lệ";
        public const string RankNotFoundMessage = "Cấp độ không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
    public static class TaskManager
    {
        public const string TaskNameExisted = "Nhiệm vụ đã tồn tại";
        public const string CreateNewTaskFailedMessage = "Tạo mới Nhiệm vụ thất bại";
        public const string UpdateTaskFailedMessage = "Cập nhật thông tin Nhiệm vụ thất bại";
        public const string UpdateTaskSuccessMessage = "Cập nhật thông tin Nhiệm vụ thành công";
        public const string EmptyTaskIdMessage = "Nhiệm vụ Id không hợp lệ";
        public const string TaskNotFoundMessage = "Nhiệm vụ không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
        public const string FullTaskMessage = "Nhân viên đã có 3 nhiệm vụ trong ngày. Không thể giao thêm nhiệm vụ.";
    }
    public static class MachineryComponents
    {
        public const string MachineryComponentsNameExisted = "Bộ phận máy cơ khí đã tồn tại";
        public const string CreateNewMachineryComponentsFailedMessage = "Tạo mới Bộ phận máy cơ khí thất bại";
        public const string UpdateMachineryComponentsFailedMessage = "Cập nhật thông tin Bộ phận máy cơ khí thất bại";
        public const string UpdateMachineryComponentsSuccessMessage = "Cập nhật thông tin Bộ phận máy cơ khí thành công";
        public const string EmptyMachineryComponentsIdMessage = "Bộ phận máy cơ khí Id không hợp lệ";
        public const string MachineryComponentsNotFoundMessage = "Bộ phận máy cơ khí không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
    public static class News
    {
        public const string NewsNameExisted = "Tin tức đã tồn tại";
        public const string CreateNewNewsFailedMessage = "Tạo mới Tin tức thất bại";
        public const string UpdateNewsFailedMessage = "Cập nhật thông tin Tin tức thất bại";
        public const string UpdateNewsSuccessMessage = "Cập nhật thông tin Tin tức thành công";
        public const string EmptyNewsIdMessage = "Tin tức không hợp lệ";
        public const string NewsNotFoundMessage = "Tin tức không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
    public static class Discount
    {
        public const string DiscountNameExisted = "Khuyến mãi đã tồn tại";
        public const string CreateNewDiscountFailedMessage = "Tạo mới Khuyến mãi thất bại";
        public const string UpdateDiscountFailedMessage = "Cập nhật thông tin Khuyến mãi thất bại";
        public const string UpdateDiscountSuccessMessage = "Cập nhật thông tin Khuyến mãi thành công";
        public const string EmptyDiscountIdMessage = "Khuyến mãi Id không hợp lệ";
        public const string DiscountNotFoundMessage = "Khuyến mãi không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
        public const string ExceedQuantityMessage = "Số lượng vượt mức tồn kho";
    }
}