using Microsoft.AspNetCore.Mvc.Rendering;
namespace SUTUTWebApp.Models.ViewModels;

public class UtrkaFormVM
{
    public int UtrkaId { get; set; }

    public string Naziv { get; set; } = "";
    public DateOnly Datum { get; set; }
    public string Grad { get; set; } = "";
    public string Drzava { get; set; } = "";

    public int OrganizatorId { get; set; }
    public int StatusId { get; set; }

    public List<SelectListItem> Organizatori { get; set; } = new();
    public List<SelectListItem> Statusi { get; set; } = new();
    public List<SelectListItem> TipoviKategorije { get; set; } = new();

    public List<KategorijaRowVM> Kategorije { get; set; } = new();
}