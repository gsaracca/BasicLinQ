using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicLinq.Models;

[Table("clientes")]
public class Cliente
{
    [Key]
    [Column("CODIGO")]
    public int Codigo { get; set; }

    [Column("ALTA")]
    public DateTime? Alta { get; set; }

    [Column("RS")]
    public string? RazonSocial { get; set; }

    [Column("NF")]
    public string? NombreFantasia { get; set; }

    [Column("CONTACTO")]
    public string? Contacto { get; set; }

    [Column("EMAIL")]
    public string? Email { get; set; }

    [Column("GRUPO")]
    public string? Grupo { get; set; }

    [Column("CLASIFICACION")]
    public string? Clasificacion { get; set; }

    [Column("DOMICILIO")]
    public string? Domicilio { get; set; }

    [Column("CIUDAD")]
    public string? Ciudad { get; set; }

    [Column("CP")]
    public string? CodigoPostal { get; set; }

    [Column("CUIT")]
    public string? Cuit { get; set; }

    [Column("IVA")]
    public string? Iva { get; set; }

    [Column("IIBB")]
    public string? Iibb { get; set; }

    [Column("GANANCIAS")]
    public string? Ganancias { get; set; }

    // Booleanos con prefijo IF (tinyint — nullable en BD)
    [Column("IFCTACTE")]
    public bool? IfCtaCte { get; set; }

    [Column("CONDICIONVENTA")]
    public int? CondicionVenta { get; set; }

    [Column("LIMITECREDITO1")]
    public decimal? LimiteCredito1 { get; set; }

    [Column("LIMITECREDITO2")]
    public decimal? LimiteCredito2 { get; set; }

    [Column("IFCONSOLIDADO")]
    public bool? IfConsolidado { get; set; }

    [Column("CONSOLIDADO")]
    public string? Consolidado { get; set; }

    [Column("IFVENDEDOR")]
    public bool? IfVendedor { get; set; }

    [Column("VENDEDOR")]
    public int? Vendedor { get; set; }

    [Column("IFTRANSPORTE")]
    public bool? IfTransporte { get; set; }

    [Column("TRANSPORTE")]
    public string? Transporte { get; set; }

    [Column("OBSERVACIONES")]
    public string? Observaciones { get; set; }

    [Column("TIPO")]
    public string? Tipo { get; set; }

    [Column("MARK")]
    public int? Mark { get; set; }

    [Column("LIMITECREDITO3")]
    public decimal? LimiteCredito3 { get; set; }

    [Column("LIMITECREDITO4")]
    public decimal? LimiteCredito4 { get; set; }

    [Column("DOM_NUMERO")]
    public int? DomNumero { get; set; }

    // Booleanos con prefijo IF_ (int — nullable en BD)
    [Column("IF_NO_FAC")]
    public bool? IfNoFac { get; set; }

    [Column("IF_CF")]
    public bool? IfCf { get; set; }

    [Column("IF_DEL")]
    public bool? IfDel { get; set; }

    [Column("EOR")]
    public int? Eor { get; set; }

    [Column("IF_SEND_MAIL")]
    public bool? IfSendMail { get; set; }
}
