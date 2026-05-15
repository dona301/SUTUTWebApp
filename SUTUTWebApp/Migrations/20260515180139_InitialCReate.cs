using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SUTUTWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCReate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ATLETSKIKLUB",
                columns: table => new
                {
                    AKlubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Grad = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Drzava = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Osnovano = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ATLETSKI__4E23C839BDF30C02", x => x.AKlubId);
                });

            migrationBuilder.CreateTable(
                name: "STATUSREZULTATA",
                columns: table => new
                {
                    StatusRezultataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__STATUSRE__E1B46CB0331E663B", x => x.StatusRezultataId);
                });

            migrationBuilder.CreateTable(
                name: "STATUSUTRKE",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__STATUSUT__C8EE20632978266A", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "TIPKATEGORIJE",
                columns: table => new
                {
                    TipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TIPKATEG__2DB1A1C81952CDB9", x => x.TipId);
                });

            migrationBuilder.CreateTable(
                name: "VRSTAORGANIZATOR",
                columns: table => new
                {
                    VrstaOrganizatoraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VRSTAORG__6B1325DB1F74A794", x => x.VrstaOrganizatoraId);
                });

            migrationBuilder.CreateTable(
                name: "TRKAC",
                columns: table => new
                {
                    TrkacId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Datum_rodenja = table.Column<DateOnly>(type: "date", nullable: true),
                    Spol = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Nacionalnost = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    AKlubId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TRKAC__1B133CCFD36A1F7E", x => x.TrkacId);
                    table.ForeignKey(
                        name: "FK__TRKAC__AKlubId__46136164",
                        column: x => x.AKlubId,
                        principalTable: "ATLETSKIKLUB",
                        principalColumn: "AKlubId");
                });

            migrationBuilder.CreateTable(
                name: "ORGANIZATOR",
                columns: table => new
                {
                    OrganizatorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Broj_mobitela = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    OIB = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    Web_stranica = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    Opis = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    VrstaOrganizatoraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ORGANIZA__7464266AB917FA53", x => x.OrganizatorId);
                    table.ForeignKey(
                        name: "FK__ORGANIZAT__Vrsta__3B95D2F1",
                        column: x => x.VrstaOrganizatoraId,
                        principalTable: "VRSTAORGANIZATOR",
                        principalColumn: "VrstaOrganizatoraId");
                });

            migrationBuilder.CreateTable(
                name: "TRENING",
                columns: table => new
                {
                    TreningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lokacija = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Trajanje = table.Column<TimeOnly>(type: "time", nullable: false),
                    Duljina = table.Column<double>(type: "float", nullable: false),
                    TrkacId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TRENING__3B04A8D3444F6F93", x => x.TreningId);
                    table.ForeignKey(
                        name: "FK__TRENING__TrkacId__5649C92D",
                        column: x => x.TrkacId,
                        principalTable: "TRKAC",
                        principalColumn: "TrkacId");
                });

            migrationBuilder.CreateTable(
                name: "UTRKA",
                columns: table => new
                {
                    UtrkaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Datum = table.Column<DateOnly>(type: "date", nullable: false),
                    Grad = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Drzava = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    OrganizatorId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UTRKA__038172C35314E55A", x => x.UtrkaId);
                    table.ForeignKey(
                        name: "FK__UTRKA__Organizat__405A880E",
                        column: x => x.OrganizatorId,
                        principalTable: "ORGANIZATOR",
                        principalColumn: "OrganizatorId");
                    table.ForeignKey(
                        name: "FK__UTRKA__StatusId__414EAC47",
                        column: x => x.StatusId,
                        principalTable: "STATUSUTRKE",
                        principalColumn: "StatusId");
                });

            migrationBuilder.CreateTable(
                name: "KATEGORIJA",
                columns: table => new
                {
                    KategorijaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Duljina = table.Column<double>(type: "float", nullable: false),
                    Max_broj_trkaca = table.Column<int>(type: "int", nullable: false),
                    Startnina = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Početak = table.Column<DateOnly>(type: "date", nullable: false),
                    UtrkaId = table.Column<int>(type: "int", nullable: false),
                    TipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KATEGORI__6C3B8FEEA9386602", x => x.KategorijaId);
                    table.ForeignKey(
                        name: "FK__KATEGORIJ__TipId__4BCC3ABA",
                        column: x => x.TipId,
                        principalTable: "TIPKATEGORIJE",
                        principalColumn: "TipId");
                    table.ForeignKey(
                        name: "FK__KATEGORIJ__Utrka__4AD81681",
                        column: x => x.UtrkaId,
                        principalTable: "UTRKA",
                        principalColumn: "UtrkaId");
                });

            migrationBuilder.CreateTable(
                name: "REZULTAT",
                columns: table => new
                {
                    RezultatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Finalno_vrijeme = table.Column<TimeOnly>(type: "time", nullable: true),
                    StatusRezultataId = table.Column<int>(type: "int", nullable: false),
                    TrkacId = table.Column<int>(type: "int", nullable: false),
                    KategorijaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__REZULTAT__447B09FB9915679D", x => x.RezultatId);
                    table.ForeignKey(
                        name: "FK__REZULTAT__Katego__536D5C82",
                        column: x => x.KategorijaId,
                        principalTable: "KATEGORIJA",
                        principalColumn: "KategorijaId");
                    table.ForeignKey(
                        name: "FK__REZULTAT__Status__51851410",
                        column: x => x.StatusRezultataId,
                        principalTable: "STATUSREZULTATA",
                        principalColumn: "StatusRezultataId");
                    table.ForeignKey(
                        name: "FK__REZULTAT__TrkacI__52793849",
                        column: x => x.TrkacId,
                        principalTable: "TRKAC",
                        principalColumn: "TrkacId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KATEGORIJA_TipId",
                table: "KATEGORIJA",
                column: "TipId");

            migrationBuilder.CreateIndex(
                name: "IX_KATEGORIJA_UtrkaId",
                table: "KATEGORIJA",
                column: "UtrkaId");

            migrationBuilder.CreateIndex(
                name: "IX_ORGANIZATOR_VrstaOrganizatoraId",
                table: "ORGANIZATOR",
                column: "VrstaOrganizatoraId");

            migrationBuilder.CreateIndex(
                name: "IX_REZULTAT_KategorijaId",
                table: "REZULTAT",
                column: "KategorijaId");

            migrationBuilder.CreateIndex(
                name: "IX_REZULTAT_StatusRezultataId",
                table: "REZULTAT",
                column: "StatusRezultataId");

            migrationBuilder.CreateIndex(
                name: "UQ_Trkac_Kategorija",
                table: "REZULTAT",
                columns: new[] { "TrkacId", "KategorijaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TRENING_TrkacId",
                table: "TRENING",
                column: "TrkacId");

            migrationBuilder.CreateIndex(
                name: "IX_TRKAC_AKlubId",
                table: "TRKAC",
                column: "AKlubId");

            migrationBuilder.CreateIndex(
                name: "IX_UTRKA_OrganizatorId",
                table: "UTRKA",
                column: "OrganizatorId");

            migrationBuilder.CreateIndex(
                name: "IX_UTRKA_StatusId",
                table: "UTRKA",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "REZULTAT");

            migrationBuilder.DropTable(
                name: "TRENING");

            migrationBuilder.DropTable(
                name: "KATEGORIJA");

            migrationBuilder.DropTable(
                name: "STATUSREZULTATA");

            migrationBuilder.DropTable(
                name: "TRKAC");

            migrationBuilder.DropTable(
                name: "TIPKATEGORIJE");

            migrationBuilder.DropTable(
                name: "UTRKA");

            migrationBuilder.DropTable(
                name: "ATLETSKIKLUB");

            migrationBuilder.DropTable(
                name: "ORGANIZATOR");

            migrationBuilder.DropTable(
                name: "STATUSUTRKE");

            migrationBuilder.DropTable(
                name: "VRSTAORGANIZATOR");
        }
    }
}
