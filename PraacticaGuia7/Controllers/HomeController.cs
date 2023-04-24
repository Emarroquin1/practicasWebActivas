using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PraacticaGuia7.Models;
using System.Diagnostics;

namespace PraacticaGuia7.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        NorthwindContext contexto = new NorthwindContext();

        public class ProductoMasVendido
        {
            public string NombreProducto { get; set; }
            public int Cantidad { get; set; }
        }
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var productoMasVendido = contexto.OrderDetails
         .Join(contexto.Products,
             orderDetail => orderDetail.ProductId,
             product => product.ProductId,
             (orderDetail, product) => new { orderDetail, product })
         .GroupBy(x => x.product.ProductName)
         .OrderByDescending(g => g.Count())
         .Select(g => new { NombreProducto = g.Key, CantidadVentas = g.Count()})
         .FirstOrDefault();

            ViewBag.productoMasVendido= productoMasVendido;

            //TOP 5 prodcutos
            var topProductosBaratos = contexto.Products
             .OrderBy(p => p.UnitPrice)
             .Take(5)
             .ToList();

            ViewBag.TopProductosBaratos = topProductosBaratos;
            //BEBIDAS ONLY
            var productosBebidas = contexto.Products
           .Where(p => p.CategoryId == 1)
           .Select(p => new { p.ProductName, p.UnitPrice,Categoria = p.Category.CategoryName })
           .ToList();


            ViewBag.ProductosBebidas = productosBebidas;

            //provistos por paris
            var productosParis = contexto.Products
             .Where(p => p.SupplierId == 18)
              .Select(p => new { p.ProductName, p.UnitPrice,Supplier = p.Supplier.City })
             .ToList();

            ViewBag.productosParis = productosParis;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}