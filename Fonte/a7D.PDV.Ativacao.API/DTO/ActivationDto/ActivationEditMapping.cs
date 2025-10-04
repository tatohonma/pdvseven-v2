using a7D.PDV.Ativacao.API.Model;

public static class ActivationEditMapping
{
    public static ActivationEditDto ToEditDto(Activation a)
        => new ActivationEditDto
        {
            Id = a.Id,
            ActivationKey = a.ActivationKey,
            ClientId = a.ClientId,
            Client = a.Client == null ? null : new ClientLiteDto
            {
                Id = a.Client.Id,
                ResellerId = a.Client.ResellerId,
                Name = a.Client.Name,
                CompanyName = a.Client.CompanyName,
                CpfCnpj = a.Client.CpfCnpj,
                Street = a.Client.Street,
                Number = a.Client.Number,
                AdditionalInfo = a.Client.AdditionalInfo,
                City = a.Client.City,
                State = a.Client.State,
                Phone = a.Client.Phone,
                TinyId = a.Client.TinyId,
                CreatedAt = a.Client.CreatedAt,
                UpdatedAt = a.Client.UpdatedAt
            },
            LastCheckedAt = a.LastCheckedAt,
            ActivatedAt = a.ActivatedAt,
            ValidityDays = a.ValidityDays,
            IsActive = a.IsActive,
            Notes = a.Notes,
            ReactivatedBySupport = a.ReactivatedBySupport,
            SupportReactivatedAt = a.SupportReactivatedAt,
            ProvisionalValidityUntil = a.ProvisionalValidityUntil,
            IsDuplicate = a.IsDuplicate,
            SiteAdmin = a.SiteAdmin,
            PdVs = a.PDVs?.Select(p => new PdvEditDto
            {
                Id = p.Id,
                ActivationId = p.ActivationId,
                InstallationPdvId = p.InstallationPdvId,
                PdvTypeId = p.PdvTypeId,
                PdvType = p.PdvType == null ? null : new PdvTypeLiteDto
                {
                    Id = p.PdvType.Id,
                    Name = p.PdvType.Name
                },
                Name = p.Name,
                HardwareKey = p.HardwareKey,
                Version = p.Version,
                IsActive = p.IsActive,
                UpdatedAt = p.LastUpdatedAt
            }).ToList() ?? new List<PdvEditDto>(),
            LicensesHtml = a.LicensesHtml,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        };

    public static Activation ToEntity(ActivationEditDto d)
        => new Activation
        {
            Id = d.Id,
            ActivationKey = d.ActivationKey,

            ClientId = d.ClientId,
            Client = null,

            LastCheckedAt = d.LastCheckedAt,
            ActivatedAt = d.ActivatedAt,
            ValidityDays = d.ValidityDays,
            IsActive = d.IsActive,
            Notes = d.Notes,
            ReactivatedBySupport = d.ReactivatedBySupport,
            SupportReactivatedAt = d.SupportReactivatedAt,
            ProvisionalValidityUntil = d.ProvisionalValidityUntil,
            IsDuplicate = d.IsDuplicate,
            SiteAdmin = d.SiteAdmin,

            PDVs = d.PdVs?.Select(p => new Pdv
            {
                Id = p.Id,
                ActivationId = d.Id, 
                InstallationPdvId = p.InstallationPdvId,

                PdvTypeId = p.PdvTypeId,
                PdvType = null,

                Name = p.Name,
                HardwareKey = p.HardwareKey,
                Version = p.Version,
                IsActive = p.IsActive,
                LastUpdatedAt = p.UpdatedAt
            }).ToList() ?? new List<Pdv>()
        };
}
