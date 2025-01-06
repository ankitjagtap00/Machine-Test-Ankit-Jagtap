using ProductInventory.Data;
using ProductInventory.Services;
using System.Web.Mvc;
using Unity.AspNet.Mvc;
using Unity;


namespace ProductInventory.App_Start
{
    public class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<ApplicationDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IProductService, ProductService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
