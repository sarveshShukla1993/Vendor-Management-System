using System.Web.Mvc;

namespace VendorManagementSystem.Areas.PaymentGateway
{
    public class PaymentGatewayAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PaymentGateway";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PaymentGateway_default",
                "PaymentGateway/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}