using System;
using System.Collections.Generic;

namespace SUTUTWebApp.Models.Entities;

public partial class Utrka
{
    public int UtrkaId { get; set; }

    public string Naziv { get; set; } = null!;

    public DateOnly Datum { get; set; }

    public string Grad { get; set; } = null!;

    public string Drzava { get; set; } = null!;

    public int OrganizatorId { get; set; }

    public int StatusId { get; set; }

    public virtual ICollection<Kategorija> Kategorijas { get; set; } = new List<Kategorija>();

    public virtual Organizator Organizator { get; set; } = null!;

    public virtual Statusutrke Status { get; set; } = null!;
}
