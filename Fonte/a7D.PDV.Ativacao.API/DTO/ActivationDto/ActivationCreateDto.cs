using System.ComponentModel.DataAnnotations;

public sealed class ActivationCreateDto
{
    [Required]
    public int ClientId { get; set; }

    public ClientLiteDto? Client { get; set; }

    [Required, MinLength(5)]
    public string ActivationKey { get; set; } = string.Empty;

    [Range(0, 3650)]
    public int ValidityDays { get; set; } = 0;

    public bool IsActive { get; set; } = false;

    public string? Notes { get; set; }

    public bool ReactivatedBySupport { get; set; } = false;

    public List<PdvCreateDto> PdVs { get; set; } = new();
}

public sealed class PdvCreateDto
{
    [Required]
    public int PdvTypeId { get; set; }

    public string? Name { get; set; }
    public string? HardwareKey { get; set; }
    public string? Version { get; set; }

    public bool IsActive { get; set; } = true;

}
