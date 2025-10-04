public sealed class ActivationEditDto
{
    public int Id { get; set; }
    public string ActivationKey { get; set; } = string.Empty;

    public int ClientId { get; set; }
    public ClientLiteDto? Client { get; set; }

    public DateTime? LastCheckedAt { get; set; }
    public DateTime? ActivatedAt { get; set; }

    public int ValidityDays { get; set; }
    public bool IsActive { get; set; }

    public string? Notes { get; set; }

    public bool ReactivatedBySupport { get; set; }
    public DateTime? SupportReactivatedAt { get; set; }
    public DateTime? ProvisionalValidityUntil { get; set; }

    public bool IsDuplicate { get; set; }

    // Admin-only (se usar no front)
    public bool? SiteAdmin { get; set; }

    public List<PdvEditDto> PdVs { get; set; } = new();

    // opcional: HTML calculado para exibir na listagem
    public string? LicensesHtml { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public sealed class ClientLiteDto
{
    public int Id { get; set; }
    public int ResellerId { get; set; }

    // campos de leitura (GET) para preencher a tela; no PUT não são necessários
    public string? Name { get; set; }
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

public sealed class PdvEditDto
{
    public int Id { get; set; } 
    public int ActivationId { get; set; } 
    public int? InstallationPdvId { get; set; }

    public int PdvTypeId { get; set; }
    public PdvTypeLiteDto? PdvType { get; set; }

    public string? Name { get; set; }
    public string? HardwareKey { get; set; }
    public string? Version { get; set; }
    public bool IsActive { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public sealed class PdvTypeLiteDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
