using Microsoft.Sbom.Parsers.Spdx22SbomParser.Entities;
using Newtonsoft.Json;

namespace security.sbom.Entities;

[JsonObject]
public class Package : Vertex
{
    public Package(string name, string version, string spdxid, IList<ExternalReference> externalReferences)
    {
        Id = $"{name}@{version}";
        Label = Id;
        Name = name;
        Version = version;
        Locator = externalReferences?.FirstOrDefault()?.Locator;
        SPDXID = spdxid;
    }

    public string SPDXID { get; set; }

    public string? Locator { get; set; }
}
