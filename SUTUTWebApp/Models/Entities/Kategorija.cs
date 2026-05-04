using System;
using System.Collections.Generic;

namespace SUTUTWebApp.Models.Entities;

public partial class Kategorija
{
    public int KategorijaId { get; set; }

    public string Naziv { get; set; } = null!;

    public double Duljina { get; set; }

    public int MaxBrojTrkaca { get; set; }

    public decimal Startnina { get; set; }

    public DateOnly Početak { get; set; }

    public int UtrkaId { get; set; }

    public int TipId { get; set; }

    public virtual ICollection<Rezultat> Rezultats { get; set; } = new List<Rezultat>();

    public virtual Tipkategorije Tip { get; set; } = null!;

    public virtual Utrka Utrka { get; set; } = null!;
}
