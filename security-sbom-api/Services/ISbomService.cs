using security.sbom.Entities;

namespace security.sbom.Services;

public interface ISbomService
{
    Task SaveSpbxAsync(SPDX22Document spbxDocument);
}