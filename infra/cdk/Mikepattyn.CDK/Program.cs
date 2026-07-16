using Amazon.CDK;

namespace Mikepattyn.CDK;

sealed class Program
{
    public static void Main(string[] args)
    {
        var app = new App();
        StackComposition.Build(app);
        app.Synth();
    }
}
