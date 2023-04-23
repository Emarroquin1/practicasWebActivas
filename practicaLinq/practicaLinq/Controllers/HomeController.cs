using Microsoft.AspNetCore.Mvc;
using practicaLinq.Models;
using System.Diagnostics;

namespace practicaLinq.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        //crear contexto
        NorthwindContext contexto = new NorthwindContext();
		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{

			//Forma lineal
            //sumar
            Func<int, int> cuadrado = x => x * x;
            ViewBag.cuadrado= cuadrado(5);
            //raiz
            Func<double, double> raiz = x => Math.Sqrt(x);
            ViewBag.raiz = raiz(5);

			//Forma 2 de bloque
			//Raiz int
			Func<int, int> raizBloque = x =>
			{
				return (int) Math.Sqrt(x);
			};
            ViewBag.raizBloque = raizBloque(10);
            //Suma 2 numeros

            Func<int,int, int> suma =(x,y) => x + y;

			ViewBag.suma = suma(10,10);

            //Suma 2 numeros bloque

            Func<int, int, int> sumaBloque = (x, y)=> {
			
			return x + y;
         }; 
            ViewBag.sumaBloque = sumaBloque(20, 20);
            //fin sumaBloque

            //Cuadratica
            Func<double, double,double, string> cuadratica = (a, b,c) => {

                double discriminante = Math.Pow(b, 2) - 4 * a * c;
                String respuesta = "";
                if (discriminante < 0)
                {
                    Console.WriteLine("La ecuación cuadrática no tiene solución real.");
                }
                else if (discriminante == 0)
                {
                    double x = -b / (2 * a);
                    respuesta="La solución única es: x = " + x;
                }
                else
                {
                    double x1 = (-b + Math.Sqrt(discriminante)) / (2 * a);
                    double x2 = (-b - Math.Sqrt(discriminante)) / (2 * a);
                    respuesta="Las soluciones son: x1 = " + x1 + " y x2 = " + x2;
                }
                return respuesta; 
            };

            ViewBag.cuadratica=cuadratica(1,2,-15);
            //fin cuadratica

            /*LINQ 
            Listar todos los productos cero en existencia

            */
            var ceroStock = contexto.Products.Where(x=>x.UnitsInStock==0).ToList();

            ViewBag.ceroStock = ceroStock;
            //fin listar 0 existencia

            /*LINQ 
            Listar todos los productos que valen mas de 100 dolares
            */
            var cienStock = contexto.Products.Where(x => x.UnitPrice > 100).ToList();

            ViewBag.cienStock = cienStock;
            //fin listar 100

            //FORMA 2 LINQ
            var cienStock2 = from prod in contexto.Products where prod.UnitPrice > 100 select new Product {ProductName=prod.ProductName, UnitPrice=prod.UnitPrice };
            ViewBag.cienStock2 = cienStock2;
            /*LISTAR TODOS LOS PRODUCTOS CON SU PROVEEDOR*/
            var queryProveedores = from prod in contexto.Products where prod.UnitPrice > 100
               join Supplier in contexto.Suppliers on prod.SupplierId equals Supplier.SupplierId select new
            {
                 prod.ProductName,
                 prod.UnitPrice,
                 Supplier.CompanyName
            };
            ViewBag.queryProveedores=queryProveedores;
            //Fin LISTAR TODOS LOS PRODUCTOS CON SU PROVEEDOR

            //Cargar productos en una ListView
            var productos = contexto.Products.ToList();
            ViewBag.productos = productos;
            //fin listView
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