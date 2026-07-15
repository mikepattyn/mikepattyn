using System.Diagnostics.CodeAnalysis;

namespace Mikepattyn.CDK.Constructs;

/// <summary>
/// Aspect that validates a construct only contains imported resources and doesn't create new AWS resources.
/// </summary>
[ExcludeFromCodeCoverage]
public class ImportedOnlyConstructAspect : DeputyBase, IAspect
{
    private readonly string _constructName;

    public ImportedOnlyConstructAspect(string constructName)
    {
        _constructName = constructName;
    }

    public void Visit(IConstruct node)
    {
        if (node is Construct construct && construct.Node.Id == _constructName)
        {
            ValidateImportedOnly(construct);
        }
    }

    private static void ValidateImportedOnly(Construct construct)
    {
        foreach (var child in construct.Node.Children)
        {
            if (IsCreatingNewResource(child))
            {
                throw new InvalidOperationException(
                    $"Construct '{construct.Node.Id}' should only contain imported resources. "
                        + $"Found resource-creating child: {child.GetType().Name} with ID: {child.Node.Id}"
                );
            }
        }
    }

    private static bool IsCreatingNewResource(IConstruct child)
    {
        var importedResourceTypes = new[]
        {
            "Amazon.CDK.AWS.Route53.HostedZone",
            "Amazon.CDK.AWS.CertificateManager.Certificate",
        };

        var childType = child.GetType().FullName;
        if (importedResourceTypes.Any(type => childType == type))
        {
            return false;
        }

        foreach (var constructor in child.GetType().GetConstructors())
        {
            if (
                constructor
                    .GetParameters()
                    .Any(p => p.Name?.Contains("Props") == true || p.Name?.Contains("Options") == true)
            )
            {
                return true;
            }
        }

        return false;
    }
}
