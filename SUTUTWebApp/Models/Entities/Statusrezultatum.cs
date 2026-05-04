using System;
using System.Collections.Generic;

namespace SUTUTWebApp.Models.Entities;

public partial class Statusrezultatum
{
    public int StatusRezultataId { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Rezultat> Rezultats { get; set; } = new List<Rezultat>();
}
