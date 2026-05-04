using System;
using System.Collections.Generic;

namespace SUTUTWebApp.Models.Entities;

public partial class Tipkategorije
{
    public int TipId { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Kategorija> Kategorijas { get; set; } = new List<Kategorija>();
}
