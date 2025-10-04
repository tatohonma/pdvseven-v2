using a7D.PDV.Ativacao.API.Model;

public static class ActivationCreateMapping
{
    public static Activation ToEntity(ActivationCreateDto d)
        => new Activation
        {
            ActivationKey = d.ActivationKey,

            ClientId = d.ClientId != 0 ? d.ClientId : (d.Client?.Id ?? 0),
            Client = null, 

            LastCheckedAt = null,
            ActivatedAt = null,

            ValidityDays = d.ValidityDays,
            IsActive = d.IsActive,
            Notes = d.Notes,

            ReactivatedBySupport = d.ReactivatedBySupport,
            SupportReactivatedAt = null,
            ProvisionalValidityUntil = null,

            IsDuplicate = false,
            SiteAdmin = null,

            PDVs = d.PdVs?.Select(p => new Pdv
            {
                PdvTypeId = p.PdvTypeId,
                PdvType = null,

                Name = p.Name,
                HardwareKey = p.HardwareKey,
                Version = p.Version,
                IsActive = p.IsActive,

                InstallationPdvId = null,

                LastUpdatedAt = DateTime.UtcNow
            }).ToList() ?? new List<Pdv>()
        };
}
