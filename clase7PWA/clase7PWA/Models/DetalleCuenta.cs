using System;
using System.Collections.Generic;

namespace clase7PWA.Models;

public partial class DetalleCuenta
{
    public int Iddetalle { get; set; }

    public int? NumeroCuenta { get; set; }

    public string? Descripcion { get; set; }

    public virtual Cuenta? NumeroCuentaNavigation { get; set; }
}

