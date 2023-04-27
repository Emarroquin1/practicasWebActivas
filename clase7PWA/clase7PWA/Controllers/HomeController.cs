using clase7PWA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace clase7PWA.Controllers
{
    public class HomeController : Controller
    {

        Ds39aContext contexto = new Ds39aContext(); 
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {


            decimal monto = 120;
            /*transaccion implicita

           

            var cta = contexto.Cuentas.FirstOrDefault(x => x.NumeroCuenta==123);

            var cta2 = contexto.Cuentas.FirstOrDefault(x => x.NumeroCuenta==456);

            cta.Saldo -= monto;
            cta2.Saldo += monto;

            contexto.SaveChanges();

            ViewBag.cta = cta.Saldo;
            ViewBag.cta2 = cta2.Saldo;
            */

            //transacciones explicitas

            //otra forma de utilizar el contexto con using

            using (var contexto2  = new Ds39aContext()) {
            
                using(var transaccion = contexto2.Database.BeginTransaction())
                {
                    try {

                        var cta = contexto2.Cuentas.FirstOrDefault(x => x.NumeroCuenta == 123);
                        var cta2 = contexto2.Cuentas.FirstOrDefault(x => x.NumeroCuenta == 456);

                        cta.Saldo -= monto;
                        cta2.Saldo += monto;

                        contexto.SaveChanges();
                        transaccion.Commit();
                        ViewBag.cta = cta.Saldo;
                        ViewBag.cta2 = cta2.Saldo;
                    }catch(Exception ex)
                    {
                        transaccion.Rollback();
                    }
                }
            
            
            }



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