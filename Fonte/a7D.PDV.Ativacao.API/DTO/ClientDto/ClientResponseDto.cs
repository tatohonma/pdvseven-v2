namespace a7D.PDV.Ativacao.API.DTO.ClientDto;

public class ClientResponseDto
{
    public int Id { get; set; }
    public int ResellerId { get; set; }
    public string? ResellerName { get; set; }
    public string Name { get; set; } = null!;
    public string? CompanyName { get; set; }
    public string? CpfCnpj { get; set; }
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? AdditionalInfo { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Phone { get; set; }
    public string? TinyId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}