
public sealed class ClientResponseDto
{
    public int Id { get; set; }
    public int ResellerId { get; set; }
    public object? Reseller { get; set; }
    public string Name { get; set; } = "";
    public string? CompanyName { get; set; }
    public string? CpfCnpj { get; set; }
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? AdditionalInfo { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Phone { get; set; }
    public string? TinyId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public sealed class PdvResponseDto
{
    public int Id { get; set; }
    public int ActivationId { get; set; }           
    public int? InstallationPdvId { get; set; }
    public int PdvTypeId { get; set; }
    public string? Name { get; set; }
    public string? HardwareKey { get; set; }
    public string? Version { get; set; }
    public bool IsActive { get; set; }
    public DateTime? UpdatedAt { get; set; }     
}

public sealed class ActivationResponseDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public ClientResponseDto Client { get; set; } = null!;
    public string ActivationKey { get; set; } = "";
    public DateTime? LastCheckedAt { get; set; }
    public DateTime? ActivatedAt { get; set; }
    public int ValidityDays { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public bool ReactivatedBySupport { get; set; }
    public DateTime? SupportReactivatedAt { get; set; }
    public DateTime? ProvisionalValidityUntil { get; set; }
    public bool IsDuplicate { get; set; }
    public bool? SiteAdmin { get; set; }
    public List<PdvResponseDto>? PdVs { get; set; }
    public string LicensesHtml { get; set; } = "";
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
