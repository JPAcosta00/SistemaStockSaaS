namespace Application.DTOs;

public class ProductImportResultDto
{
    public int TotalRows { get; set; }
    public int SuccessfulRows { get; set; }
    public int FailedRows { get; set; }
    public List<RowErrorDto> Errors { get; set; } = new();
}

public class RowErrorDto
{
    public int RowNumber { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}

//estas clases lo que hacen es: si se ingresa un excel con 500 productos y el numero 40 tiene un error cualquiera
//entonces se procesan los productos correctos y devuelven un reporte con detalles en las filas que fallaron.