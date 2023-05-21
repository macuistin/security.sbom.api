using Microsoft.Sbom.Parsers.Spdx22SbomParser.Entities;
using System.Text.Json.Serialization;

namespace security.sbom.Entities;

public class SPDX22Document
{
    /// <summary>
    /// Gets or sets reference number for the version to understand how to parse and interpret the format.
    /// </summary>
    [JsonPropertyName("spdxVersion")]
    public string SPDXVersion { get; set; }

    /// <summary>
    /// Gets or sets license for compliance with the SPDX specification.
    /// </summary>
    [JsonPropertyName("dataLicense")]
    public string DataLicense { get; set; }

    /// <summary>
    /// Gets or sets unique Identifier for elements in SPDX document.
    /// </summary>
    [JsonPropertyName("SPDXID")]
    public string SpdxId { get; set; }

    /// <summary>
    /// Gets or sets identify name of this document as designated by creator.
    /// </summary>
    [JsonPropertyName("name")]
    public string DocumentName { get; set; }

    /// <summary>
    /// Gets or sets sPDX document specific namespace as a URI.
    /// </summary>
    [JsonPropertyName("documentNamespace")]
    public string DocumentNamespace { get; set; }

    /// <summary>
    /// Gets or sets provides the necessary information for forward and backward compatibility for processing tools.
    /// </summary>
    [JsonPropertyName("creationInfo")]
    public CreationInfo CreationInfo { get; set; }

    /// <summary>
    /// Gets or sets files referenced in the SPDX document.
    /// </summary>
    [JsonPropertyName("files")]
    public List<SPDXFile> Files { get; set; }

    /// <summary>
    /// Gets or sets packages referenced in the SPDX document.
    /// </summary>
    [JsonPropertyName("packages")]
    public List<SPDXPackage> Packages { get; set; }


}