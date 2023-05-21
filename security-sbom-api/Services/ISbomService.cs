using security.sbom.Entities;

namespace security.sbom.Services;

public interface ISbomService
{
    Task SaveSpbxAsync(string type, SPDX22Document spbxDocument);
}