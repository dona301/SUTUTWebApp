using System;
using System.Collections.Generic;

namespace SUTUTWebApp.Models.Entities;

public partial class Vrstaorganizator
{
    public int VrstaOrganizatoraId { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Organizator> Organizators { get; set; } = new List<Organizator>();
}
