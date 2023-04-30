using System;
using System.Collections.Generic;

namespace clase7PWA.Models;

public partial class Cuenta
{
    public int NumeroCuenta { get; set; }

    public decimal? Saldo { get; set; }

    public virtual ICollection<DetalleCuenta> DetalleCuenta { get; set; } = new List<DetalleCuenta>();
}
