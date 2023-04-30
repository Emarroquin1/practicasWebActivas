using clase7PWA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


            decimal monto = 100;
            /*transaccion implicita

           

            var cta = contexto.Cuentas.FirstOrDefault(x => x.NumeroCuenta==123);

            var cta2 = contexto.Cuentas.FirstOrDefault(x => x.NumeroCuenta==456);

            cta.Saldo -= monto;
            cta2.Saldo += monto;

            contexto.SaveChanges();

            ViewBag.cta = cta.Saldo;
            ViewBag.cta2 = cta2.Saldo;
            */

            //transacciones explicitas con rollback

            //otra forma de utilizar el contexto con using


            using (var contexto2  = new Ds39aContext()) {
            
                using(var transaccion = contexto2.Database.BeginTransaction())
                {
                    try {

                        
                        

                        var cta = contexto2.Cuentas.FirstOrDefault(x => x.NumeroCuenta == 123);
                        var cta2 = contexto2.Cuentas.FirstOrDefault(x => x.NumeroCuenta == 456);

                        

                        cta.Saldo -= monto;
                        cta2.Saldo += monto;


                        if (cta.Saldo == 0)
                        {
                            cta.Saldo += monto;
                        }
                        else
                        {
                             contexto2.SaveChanges();
                          
                        }
                    
                        ViewBag.cta = cta.Saldo;
                        ViewBag.cta2 = cta2.Saldo;
                        transaccion.Commit();
                   
                    }catch(Exception ex)
                    {
                        transaccion.Rollback();
                       
                        ViewBag.error = "No se puede realizar la transferencia";
                    }
                }
                /*El ejercicio consta de realizar una eliminación de la cuenta con NumeroCuenta= 123 usando
                    transacciones implícitas y explicitas.
                    borrar
                
                 
                /* forma 1 basica

                var entidad = contexto.DetalleCuentas.Find(1);

                contexto.DetalleCuentas.Remove(entidad);

                var entidad2 = contexto.DetalleCuentas.Find(2);

                contexto.DetalleCuentas.Remove(entidad2);


                var entidadCuenta = contexto.Cuentas.Find(456);

                contexto.Cuentas.Remove(entidadCuenta);

                contexto.SaveChanges();*/

                // forma 2  
             

                int numeroCuentaABorrar = 123; // Reemplaza con el número de cuenta que deseas eliminar

                var cuentaABorrar = contexto.Cuentas.Include(c => c.DetalleCuenta)
                    .FirstOrDefault(c => c.NumeroCuenta == numeroCuentaABorrar);

                if (cuentaABorrar != null)
                {
                    contexto.DetalleCuentas.RemoveRange(cuentaABorrar.DetalleCuenta);
                    contexto.Cuentas.Remove(cuentaABorrar);
                    contexto.SaveChanges();
                }




            }


            /*transacciones explicitas con save points

            //otra forma de utilizar el contexto con using

            using (var contexto2 = new Ds39aContext())
            {

                using (var transaccion = contexto2.Database.BeginTransaction())
                {
                    try
                    {

                      


                        var cta = contexto2.Cuentas.FirstOrDefault(x => x.NumeroCuenta == 123);
                        var cta2 = contexto2.Cuentas.FirstOrDefault(x => x.NumeroCuenta == 456);

                        cta.Saldo -= monto;
                        cta2.Saldo += monto;

                        contexto2.SaveChanges();
                        transaccion.Commit();
                        ViewBag.cta = cta.Saldo;
                        ViewBag.cta2 = cta2.Saldo;
                    }
                    catch (Exception ex)
                    {
                        transaccion.RollbackToSavepoint("Por las dudas");
                        ViewBag.error = "No se puede realizar la transferencia";
                    }
                }


            }

            */

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