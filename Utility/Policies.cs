using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Models
{
    public static class Policies
    {
        public static AuthorizationPolicy AdminPolicy()
        {
            var adminPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserRoles.Admin)
                .Build();
            return adminPolicy;
        }
        public static AuthorizationPolicy ManagerPolicy()
        {
            var managerPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserRoles.Manager)
                .Build();
            return managerPolicy;
        }
        public static AuthorizationPolicy CustomerPolicy()
        {
            var customerPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserRoles.Customer)
                .Build();
            return customerPolicy;
        }
    }
    public static class Policy
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Customer = "Customer";
    }
}
