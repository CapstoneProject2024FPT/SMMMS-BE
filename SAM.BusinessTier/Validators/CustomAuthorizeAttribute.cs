using Microsoft.AspNetCore.Authorization;
using SAM.BusinessTier.Utils;
using SAM.BusinessTier.Enums;

namespace SAM.BusinessTier.Validators;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
	public CustomAuthorizeAttribute(params RoleEnum[] roleEnums)
	{
		var allowedRolesAsString = roleEnums.Select(x => x.GetDescriptionFromEnum());
		Roles = string.Join(",", allowedRolesAsString);
	}
}