using System;
using System.Collections.Generic;

namespace SUTUTWebApp.Models.Entities;

public partial class Atletskiklub
{
    public int AklubId { get; set; }

    public string Ime { get; set; } = null!;

    public string Grad { get; set; } = null!;

    public string Drzava { get; set; } = null!;

    public int? Osnovano { get; set; }

    public virtual ICollection<Trkac> Trkacs { get; set; } = new List<Trkac>();
}
